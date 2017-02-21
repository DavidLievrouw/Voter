using System;
using System.Threading.Tasks;
using DavidLievrouw.Voter.Data.Records;

namespace DavidLievrouw.Voter.Data {
  public interface IUserDataService {
    Task<UserRecord> GetUserById(Guid uniqueId);
  }
}