using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DavidLievrouw.Voter.Data.Dapper;
using DavidLievrouw.Voter.Data.Records;

namespace DavidLievrouw.Voter.Data {
  public class KnownUserDataService : IKnownUserDataService {
    readonly IQueryExecutor _queryExecutor;

    public KnownUserDataService(IQueryExecutor queryExecutor) {
      _queryExecutor = queryExecutor ?? throw new ArgumentNullException(nameof(queryExecutor));
    }

    public async Task<KnownUserRecord> GetKnownUserById(Guid uniqueId) {
      return (await _queryExecutor
          .NewQuery("SELECT * FROM [security].[User] WHERE [UniqueId]=@UniqueId;")
          .WithParameters(new {
            UniqueId = uniqueId
          })
          .ExecuteAsync<KnownUserRecord>())
        .Single();
    }

    public async Task<IEnumerable<KnownUserRecord>> FindKnownUserByCorrelationId(char type, string externalCorrelationId) {
      return await _queryExecutor
        .NewQuery("SELECT * FROM [security].[User] WHERE [ExternalCorrelationId]=@ExternalCorrelationId AND [Type]=@type;")
        .WithParameters(new {
          ExternalCorrelationId = externalCorrelationId,
          Type = type
        })
        .ExecuteAsync<KnownUserRecord>();
    }
  }
}