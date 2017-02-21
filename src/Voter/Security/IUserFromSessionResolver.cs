using DavidLievrouw.Voter.Domain.DTO;

namespace DavidLievrouw.Voter.Security {
  public interface IUserFromSessionResolver {
    User ResolveUser(ISession session);
  }
}