using DavidLievrouw.Voter.Security;

namespace DavidLievrouw.Voter.Api.Users.Models {
  public class LogoutRequest {
    public ISecurityContext SecurityContext { get; set; }
  }
}