using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DavidLievrouw.Utils;
using DavidLievrouw.Voter.Api.Users.Models;
using DavidLievrouw.Voter.Domain.DTO;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Oauth2.v2;
using Google.Apis.Plus.v1;
using Google.Apis.Plus.v1.Data;

namespace DavidLievrouw.Voter.Api.Users.Handlers {
  public class LoginGooglePlusUserHandler : IHandler<LoginGooglePlusUserRequest, bool> {
    // These come from the APIs console: https://code.google.com/apis/console
    public static ClientSecrets secrets = new ClientSecrets {
      ClientId = "34588642309-ogg3uciumd17cfu6l1m6an3hhto90543.apps.googleusercontent.com",
      ClientSecret = "jSRGYMwQqKVVfoOY_uAccNi8"
    };
    public static string[] Scopes = { PlusService.Scope.PlusLogin };

    public async Task<bool> Handle(LoginGooglePlusUserRequest request) {
      if (request.SecurityContext.GetAuthenticatedUser() != null) {
        // The user is already connected
        return true;
      }

      // Use the code exchange flow to get an access and refresh token.
      IAuthorizationCodeFlow flow =
        new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer {
          ClientSecrets = secrets,
          Scopes = Scopes
        });

      var token = await flow.ExchangeCodeForTokenAsync(
        "",
        request.Code,
        "postmessage",
        CancellationToken.None);

      // Get tokeninfo for the access token if you want to verify.
      var service = new Oauth2Service(
        new Google.Apis.Services.BaseClientService.Initializer());
      var oauthRequest = service.Tokeninfo();
      oauthRequest.AccessToken = token.AccessToken;

      var info = oauthRequest.Execute();

      // Register the authenticator and construct the Plus service
      // for performing API calls on behalf of the user.
      var credential = new UserCredential(flow, "me", token);
      var success = await credential.RefreshTokenAsync(CancellationToken.None);
      token = credential.Token;
      
      var plusService = new PlusService(
          new Google.Apis.Services.BaseClientService.Initializer()
          {
            ApplicationName = "DLVoter",
            HttpClientInitializer = credential
          });

      var peopleFeed = plusService.People.List("me", PeopleResource.ListRequest.CollectionEnum.Visible).Execute();
      var me = peopleFeed.Items.First();
      
      var user = new User {
        FirstName = me.Name.GivenName,
        LastName = me.Name.FamilyName,
        ExternalCorrelationId = new ExternalCorrelationId {Value = info.UserId},
        Type = UserType.GooglePlus,
        Environment = {["GoogleToken"] = token}
      };

      request.SecurityContext.SetAuthenticatedUser(user);

      return true;
    }
  }
}