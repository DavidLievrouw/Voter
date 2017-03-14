using System;
using DavidLievrouw.Utils;
using DavidLievrouw.Voter.Api.Users.Models;
using DavidLievrouw.Voter.Domain.DTO;
using DavidLievrouw.Voter.Security.Nancy;
using Nancy;
using Nancy.Extensions;
using Nancy.Security;

namespace DavidLievrouw.Voter.Api.Users {
  public class UsersModule : NancyModule {
    public UsersModule(
      IHandler<GetCurrentUserRequest, User> getCurrentUserHandler,
      IHandler<LoginLocalUserRequest, bool> loginLocalUserHandler,
      IHandler<LoginGooglePlusUserRequest, bool> loginGooglePlusUserHandler,
      IHandler<LogoutRequest, bool> logoutHandler,
      INancySecurityContextFactory nancySecurityContextFactory) {
      if (getCurrentUserHandler == null) throw new ArgumentNullException(nameof(getCurrentUserHandler));
      if (loginLocalUserHandler == null) throw new ArgumentNullException(nameof(loginLocalUserHandler));
      if (loginGooglePlusUserHandler == null) throw new ArgumentNullException(nameof(loginGooglePlusUserHandler));
      if (logoutHandler == null) throw new ArgumentNullException(nameof(logoutHandler));
      if (nancySecurityContextFactory == null) throw new ArgumentNullException(nameof(nancySecurityContextFactory));

      Get["api/user", true] = async (parameters, cancellationToken) => {
        this.RequiresAuthentication();
        return await getCurrentUserHandler.Handle(this.Bind(() =>
          new GetCurrentUserRequest {
            SecurityContext = nancySecurityContextFactory.Create(Context)
          }));
      };

      Post["api/user/login/local", true] = async (parameters, cancellationToken) => await loginLocalUserHandler.Handle(this.Bind(() => {
        var loginRequest = this.Bind<LoginLocalUserRequest>();
        return new LoginLocalUserRequest {
          SecurityContext = nancySecurityContextFactory.Create(Context),
          Login = loginRequest?.Login,
          Password = loginRequest?.Password
        };
      }));

      Post["api/user/login/googleplus", true] = async (parameters, cancellationToken) =>
        await loginGooglePlusUserHandler.Handle(this.Bind(() => new LoginGooglePlusUserRequest {
          SecurityContext = nancySecurityContextFactory.Create(Context),
          Code = Context.Request.Body.AsString()
        }));

      Post["api/user/logout", true] = async (parameters, cancellationToken) => {
        this.RequiresAuthentication();
        return await logoutHandler.Handle(this.Bind(() =>
          new LogoutRequest {
            SecurityContext = nancySecurityContextFactory.Create(Context)
          }));
      };
    }
  }
}