using DavidLievrouw.Voter.Security;

namespace DavidLievrouw.Voter.Api.Users.Models {
  public class LoginRequest {
    public string Login { get; set; }
    public string Password { get; set; }
    public ISecurityContext SecurityContext { get; set; }
  }
}