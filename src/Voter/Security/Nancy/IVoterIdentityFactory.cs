using DavidLievrouw.Voter.Domain.DTO;
using Nancy.Security;

namespace DavidLievrouw.Voter.Security.Nancy {
  public interface IVoterIdentityFactory {
    IUserIdentity Create(User user);
  }
}