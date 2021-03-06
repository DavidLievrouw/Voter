﻿using System.Net;
using System.Threading.Tasks;
using DavidLievrouw.Utils;
using DavidLievrouw.Voter.Api.Users.Models;
using DavidLievrouw.Voter.Domain.DTO;

namespace DavidLievrouw.Voter.Api.Users.Handlers {
  public class LogoutHandler : IHandler<LogoutRequest, bool> {
    public Task<bool> Handle(LogoutRequest request) {
      // Get rid of the token, if there is one
      var user = request.SecurityContext.GetAuthenticatedUser();
      if (user.Type == UserType.GooglePlus) {
        var tokenToRevoke = user.OAuthToken?.Value;
        if (tokenToRevoke != null) {
          var webRequest = WebRequest.Create("https://accounts.google.com/o/oauth2/revoke?token=" + tokenToRevoke);
          webRequest.GetResponse();
        }
      }

      request.SecurityContext.SetAuthenticatedUser(null);

      return Task.FromResult(true);
    }
  }
}