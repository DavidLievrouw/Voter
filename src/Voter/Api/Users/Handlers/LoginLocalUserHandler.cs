using System.Threading.Tasks;
using DavidLievrouw.Utils;
using DavidLievrouw.Voter.Api.Users.Models;
using DavidLievrouw.Voter.Domain.DTO;

namespace DavidLievrouw.Voter.Api.Users.Handlers {
  public class LoginLocalUserHandler : IHandler<LoginLocalUserRequest, bool> {
    public Task<bool> Handle(LoginLocalUserRequest request) {
      // Authorise user: ToDo
      var user = new User {
        FirstName = "John",
        LastName = "Doe",
        Login = new Login {Value = "JDoe"},
        Password = new Password {
          Value = "P@$$w0rd",
          IsEncrypted = false
        }
      };

      request.SecurityContext.SetAuthenticatedUser(user);

      return Task.FromResult(true);
    }
  }
}