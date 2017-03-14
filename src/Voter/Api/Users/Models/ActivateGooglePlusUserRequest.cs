using DavidLievrouw.Voter.Security;

namespace DavidLievrouw.Voter.Api.Users.Models {
  public class ActivateGooglePlusUserRequest {
    public string IdToken { get; set; }
    public ISecurityContext SecurityContext { get; set; }
  }
}