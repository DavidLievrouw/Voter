using System;
using System.Threading.Tasks;
using DavidLievrouw.Utils;
using DavidLievrouw.Voter.Api.Models;
using DavidLievrouw.Voter.Api.Users.Models;

namespace DavidLievrouw.Voter.Api.Users.Handlers {
  public class GetCurrentUserHandler : IHandler<GetCurrentUserRequest, User> {
    readonly IAdapter<Domain.DTO.User, User> _userAdapter;

    public GetCurrentUserHandler(IAdapter<Domain.DTO.User, User> userAdapter) {
      _userAdapter = userAdapter ?? throw new ArgumentNullException(nameof(userAdapter));
    }

    public Task<User> Handle(GetCurrentUserRequest request) {
      var authenticatedUser = request.SecurityContext.GetAuthenticatedUser();
      return Task.FromResult(_userAdapter.Adapt(authenticatedUser));
    }
  }
}