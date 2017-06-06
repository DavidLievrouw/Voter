using System;
using System.Threading.Tasks;
using DavidLievrouw.Utils;
using DavidLievrouw.Voter.Api.Users.Models;
using DavidLievrouw.Voter.Domain;

namespace DavidLievrouw.Voter.Api.Users.Handlers {
  public class ActivateGooglePlusUserHandler : IHandler<ActivateGooglePlusUserRequest, bool> {
    readonly IUserService _userService;

    public ActivateGooglePlusUserHandler(IUserService userService) {
      _userService = userService ?? throw new ArgumentNullException(nameof(userService));
    }

    public async Task<bool> Handle(ActivateGooglePlusUserRequest request) {
      if (request.SecurityContext.GetAuthenticatedUser() != null) {
        // The user is already logged in
        return true;
      }
      var user = await _userService.ActivateGooglePlusUser(request.AccessToken);
      request.SecurityContext.SetAuthenticatedUser(user);
      return true;
    }
  }
}