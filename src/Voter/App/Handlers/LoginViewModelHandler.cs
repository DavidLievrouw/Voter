using System;
using System.Linq;
using System.Threading.Tasks;
using DavidLievrouw.Utils;
using DavidLievrouw.Voter.App.Models;
using DavidLievrouw.Voter.Data.Dapper;
using DavidLievrouw.Voter.Domain.DTO;

namespace DavidLievrouw.Voter.App.Handlers {
  public class LoginViewModelHandler : IHandler<LoginViewModel> {
    readonly IQueryExecutor _queryExecutor;

    public LoginViewModelHandler(IQueryExecutor queryExecutor) {
      if (queryExecutor == null) throw new ArgumentNullException(nameof(queryExecutor));
      _queryExecutor = queryExecutor;
    }

    public async Task<LoginViewModel> Handle() {
      var dataSourceResult = (await _queryExecutor
        .NewQuery("SELECT * FROM [security].[User] WHERE [UniqueId]=@UniqueId;")
        .WithParameters(new {
          UniqueId = Guid.Parse("85BCCB04-901C-407F-9C3B-6B91A955D42B")
        })
        .ExecuteWithAnonymousResultAsync(new {
          FirstName = (string)null,
          LastName = (string)null,
          LastNamePrefix = (string)null,
          Login = (string)null,
          Password = (string)null,
          Salt = (string)null,
          UniqueId = Guid.Empty
        }))
        .Single();

      return new LoginViewModel {
        User = new User {
          FirstName = dataSourceResult.FirstName,
          LastName = dataSourceResult.LastName,
          LastNamePrefix = dataSourceResult.LastNamePrefix,
          Login = new Login {Value = dataSourceResult.Login },
          Password = new Password { Value = dataSourceResult.Password, IsEncrypted = dataSourceResult.Salt != null, Salt = dataSourceResult.Salt },
          UniqueId = dataSourceResult.UniqueId
        }
      };
    }
  }
}