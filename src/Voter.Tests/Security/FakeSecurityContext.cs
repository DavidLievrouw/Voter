using DavidLievrouw.Voter.Domain.DTO;

namespace DavidLievrouw.Voter.Security {
  public class FakeSecurityContext : ISecurityContext {
    User _authenticatedUser;

    public void SetAuthenticatedUser(User user) {
      _authenticatedUser = user;
    }

    public User GetAuthenticatedUser() {
      return _authenticatedUser;
    }
  }
}