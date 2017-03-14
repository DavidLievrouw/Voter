using System.Net;
using System.Threading.Tasks;
using DavidLievrouw.Utils;
using DavidLievrouw.Voter.Api.Users.Models;
using DavidLievrouw.Voter.Domain.DTO;
using Google.Apis.Auth.OAuth2.Responses;

namespace DavidLievrouw.Voter.Api.Users.Handlers {
  public class DisconnectGooglePlusUserHandler : IHandler<DisconnectGooglePlusUserRequest, bool> {
    public Task<bool> Handle(DisconnectGooglePlusUserRequest request) {
      // Get rid of the token, if there is one
      var user = request.SecurityContext.GetAuthenticatedUser();
      if (user.Type == UserType.GooglePlus) {
        var token = user.Environment.Get<TokenResponse>("GoogleToken");
        if (token != null) {
          var tokenToRevoke = token.RefreshToken ?? token.AccessToken;
          var webRequest = WebRequest.Create("https://accounts.google.com/o/oauth2/revoke?token=" + tokenToRevoke);
          webRequest.GetResponse();
        }
      }

      request.SecurityContext.SetAuthenticatedUser(null);

      // ToDo: Remove correlation from database

      return Task.FromResult(true);
    }
  }
}