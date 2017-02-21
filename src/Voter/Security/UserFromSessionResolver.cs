using DavidLievrouw.Voter.Common;
using DavidLievrouw.Voter.Domain.DTO;

namespace DavidLievrouw.Voter.Security {
  public class UserFromSessionResolver : IUserFromSessionResolver {
    public User ResolveUser(ISession session) {
      return session == null
        ? null
        : session[Constants.SessionKeyForUser] as User;
    }
  }
}