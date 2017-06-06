using System;
using System.Threading.Tasks;
using DavidLievrouw.Voter.Domain.DTO;

namespace DavidLievrouw.Voter.Domain {
  public interface IUserService {
    // Violation of CQS, but we are not master of the Google identifiers...
    Task<User> ActivateGooglePlusUser(string accessToken);
    Task<User> GetKnownUserById(Guid uniqueId);
  }
}