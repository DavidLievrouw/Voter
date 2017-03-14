using DavidLievrouw.Voter.Security;

namespace DavidLievrouw.Voter.Api.Users.Models {
  public class ActivateGooglePlusUserRequest {
    public string AccessToken { get; set; }
    public ISecurityContext SecurityContext { get; set; }
  }
}