using System.Threading.Tasks;
using DavidLievrouw.Utils;
using DavidLievrouw.Voter.Api.Users.Models;
using DavidLievrouw.Voter.Domain.DTO;

namespace DavidLievrouw.Voter.Api.Users.Handlers {
  public class GetCurrentUserHandler : IHandler<GetCurrentUserRequest, User> {
    public Task<User> Handle(GetCurrentUserRequest request) {
      return Task.FromResult(request.SecurityContext.GetAuthenticatedUser());
    }
  }
}