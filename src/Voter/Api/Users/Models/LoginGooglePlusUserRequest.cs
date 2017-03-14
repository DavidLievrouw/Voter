using DavidLievrouw.Voter.Security;

namespace DavidLievrouw.Voter.Api.Users.Models {
  public class LoginGooglePlusUserRequest {
    public string Code { get; set; }
    public ISecurityContext SecurityContext { get; set; }
  }
}