using DavidLievrouw.Voter.Security;

namespace DavidLievrouw.Voter.Api.Users.Models {
  public class GetCurrentUserRequest {
    public ISecurityContext SecurityContext { get; set; }
  }
}