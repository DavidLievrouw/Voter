using DavidLievrouw.Voter.Security;

namespace DavidLievrouw.Voter.Api.Users.Models {
  public class DisconnectGooglePlusUserRequest {
    public ISecurityContext SecurityContext { get; set; }
  }
}