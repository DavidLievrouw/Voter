using System;
using System.Threading.Tasks;
using DavidLievrouw.Utils;
using DavidLievrouw.Voter.App.Models;
using DavidLievrouw.Voter.Data;
using DavidLievrouw.Voter.Domain.DTO;

namespace DavidLievrouw.Voter.App.Handlers {
  public class LoginViewModelHandler : IHandler<LoginViewModel> {
    readonly IUserDataService _userDataService;

    public LoginViewModelHandler(IUserDataService userDataService) {
      if (userDataService == null) throw new ArgumentNullException(nameof(userDataService));
      _userDataService = userDataService;
    }

    public async Task<LoginViewModel> Handle() {
      var dataSourceResult = await _userDataService.GetUserById(Guid.Parse("85BCCB04-901C-407F-9C3B-6B91A955D42B"));

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