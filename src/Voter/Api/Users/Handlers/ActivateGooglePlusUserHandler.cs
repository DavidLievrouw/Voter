using System.IO;
using System.Net;
using System.Threading.Tasks;
using DavidLievrouw.Utils;
using DavidLievrouw.Voter.Api.Users.Models;
using DavidLievrouw.Voter.Domain.DTO;
using Newtonsoft.Json;

namespace DavidLievrouw.Voter.Api.Users.Handlers {
  public class ActivateGooglePlusUserHandler : IHandler<ActivateGooglePlusUserRequest, bool> {
    public async Task<bool> Handle(ActivateGooglePlusUserRequest request) {
      if (request.SecurityContext.GetAuthenticatedUser() != null) {
        // The user is already logged in
        return true;
      }
     
      var responseJson = "";
      using (var memoryStream = new MemoryStream()) {
        var webRequest = WebRequest.Create("https://www.googleapis.com/oauth2/v1/userinfo?alt=json&access_token=" + request.AccessToken);
        var response = webRequest.GetResponse();
        response.GetResponseStream().CopyTo(memoryStream);
        memoryStream.Seek(0, SeekOrigin.Begin);
        using (var reader = new StreamReader(memoryStream)) {
          responseJson = reader.ReadToEnd();
        }
      }

      var gResponse = JsonConvert.DeserializeObject<GoogleUserResponse>(responseJson);

      var user = new User {
        FirstName = gResponse.Given_name,
        LastName = gResponse.Family_name,
        ExternalCorrelationId = new ExternalCorrelationId {Value = gResponse.Id},
        Type = UserType.GooglePlus
      };

      request.SecurityContext.SetAuthenticatedUser(user);
      return true;
    }

    public class GoogleUserResponse {
      public string Id { get; set; }
      public string Given_name { get; set; }
      public string Family_name { get; set; }
    }
  }
}