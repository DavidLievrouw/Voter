using DavidLievrouw.Voter.Domain.DTO;
using Nancy.Security;

namespace DavidLievrouw.Voter.Security.Nancy {
  public class VoterIdentityFactory : IVoterIdentityFactory {
    public IUserIdentity Create(User user) {
      return user == null
        ? null
        : new VoterIdentity(user);
    }
  }
}