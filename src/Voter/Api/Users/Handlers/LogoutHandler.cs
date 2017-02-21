using System.Threading.Tasks;
using DavidLievrouw.Utils;
using DavidLievrouw.Voter.Api.Users.Models;

namespace DavidLievrouw.Voter.Api.Users.Handlers {
  public class LogoutHandler : IHandler<LogoutRequest, bool> {
    public Task<bool> Handle(LogoutRequest request) {
      request.SecurityContext.SetAuthenticatedUser(null);
      return Task.FromResult(true);
    }
  }
}