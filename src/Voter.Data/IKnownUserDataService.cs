using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DavidLievrouw.Voter.Data.Records;

namespace DavidLievrouw.Voter.Data {
  public interface IKnownUserDataService {
    Task<KnownUserRecord> GetKnownUserById(Guid uniqueId);
    Task<IEnumerable<KnownUserRecord>> FindKnownUserByCorrelationId(char type, string externalCorrelationId);
  }
}