using DavidLievrouw.Voter.Domain.DTO;

namespace DavidLievrouw.Voter.Security {
  public interface ISecurityContext {
    void SetAuthenticatedUser(User user);
    User GetAuthenticatedUser();
  }
}