using System;
using System.Collections.Generic;
using System.Data;
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

    public async Task AddOrUpdateKnownUser(KnownUserRecord knownUser) {
      if (knownUser == null) throw new ArgumentNullException(nameof(knownUser));

      await _queryExecutor
        .NewQuery(@"
MERGE [security].[User] AS [TARGET]
USING @AddedOrUpdatedUsers AS [SOURCE]
ON ([TARGET].[UniqueId] = [SOURCE].[UniqueId] OR ([TARGET].[Type] = [SOURCE].[Type] AND [TARGET].[ExternalCorrelationId] IS NOT NULL AND [TARGET].[ExternalCorrelationId] = [SOURCE].[ExternalCorrelationId]))
WHEN MATCHED THEN
  UPDATE 
  SET [UniqueId] = [SOURCE].[UniqueId]
      ,[Login] = [SOURCE].[Login]
      ,[Password] = [SOURCE].[Password]
      ,[Salt] = [SOURCE].[Salt]
      ,[FirstName] = [SOURCE].[FirstName]
      ,[LastName] = [SOURCE].[LastName]
      ,[LastNamePrefix] = [SOURCE].[LastNamePrefix]
      ,[Type] = [SOURCE].[Type]
      ,[ExternalCorrelationId] = [SOURCE].[ExternalCorrelationId]
WHEN NOT MATCHED BY TARGET THEN 
  INSERT
      ([UniqueId]
      ,[Login]
      ,[Password]
      ,[Salt]
      ,[FirstName]
      ,[LastName]
      ,[LastNamePrefix]
      ,[Type]
      ,[ExternalCorrelationId])
  VALUES
      ([SOURCE].[UniqueId]
      ,[SOURCE].[Login]
      ,[SOURCE].[Password]
      ,[SOURCE].[Salt]
      ,[SOURCE].[FirstName]
      ,[SOURCE].[LastName]
      ,[SOURCE].[LastNamePrefix]
      ,[SOURCE].[Type]
      ,[SOURCE].[ExternalCorrelationId]);")
        .WithCommandType(CommandType.Text)
        .WithParameters(new {
          AddedOrUpdatedReferrals = new[] {knownUser}.AsTableValuedParameter("security.T_User",
            new[] {
              "UniqueId",
              "Login",
              "Password",
              "Salt",
              "FirstName",
              "LastName",
              "LastNamePrefix",
              "Type",
              "ExternalCorrelationId"
            })
        })
        .ExecuteAsync()
        .ConfigureAwait(false);
    }
  }
}