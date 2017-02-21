using System;
using System.Linq;
using System.Threading.Tasks;
using DavidLievrouw.Voter.Data.Dapper;
using DavidLievrouw.Voter.Data.Records;

namespace DavidLievrouw.Voter.Data {
  public class UserDataService : IUserDataService {
    readonly IQueryExecutor _queryExecutor;

    public UserDataService(IQueryExecutor queryExecutor) {
      if (queryExecutor == null) throw new ArgumentNullException(nameof(queryExecutor));
      _queryExecutor = queryExecutor;
    }

    public async Task<UserRecord> GetUserById(Guid uniqueId) {
      return (await _queryExecutor
          .NewQuery("SELECT * FROM [security].[User] WHERE [UniqueId]=@UniqueId;")
          .WithParameters(new {
            UniqueId = uniqueId
          })
          .ExecuteAsync<UserRecord>())
        .Single();
    }
  }
}