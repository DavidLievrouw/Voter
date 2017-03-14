using System.Threading.Tasks;
using DavidLievrouw.Utils;
using DavidLievrouw.Voter.Api.Models;
using DavidLievrouw.Voter.Api.Users.Models;

namespace DavidLievrouw.Voter.Api.Users.Handlers {
  public class GetCurrentUserHandler : IHandler<GetCurrentUserRequest, User> {
    public Task<User> Handle(GetCurrentUserRequest request) {
      var authenticatedUser = request.SecurityContext.GetAuthenticatedUser();
      return Task.FromResult(authenticatedUser == null
        ? null
        : new User {
          FirstName = authenticatedUser.FirstName,
          LastNamePrefix = authenticatedUser.LastNamePrefix,
          UniqueId = authenticatedUser.UniqueId,
          LastName = authenticatedUser.LastName
        });
    }
  }
}