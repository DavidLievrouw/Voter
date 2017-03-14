using DavidLievrouw.Utils;
using DavidLievrouw.Utils.ForTesting.DotNet;
using DavidLievrouw.Utils.ForTesting.FakeItEasy;
using DavidLievrouw.Voter.Api.Users.Models;
using DavidLievrouw.Voter.Configuration;
using DavidLievrouw.Voter.Domain.DTO;
using DavidLievrouw.Voter.Security;
using DavidLievrouw.Voter.Security.Nancy;
using FakeItEasy;
using Nancy;
using Nancy.Responses.Negotiation;
using Nancy.Testing;
using NUnit.Framework;

namespace DavidLievrouw.Voter.Api.Users {
  [TestFixture]
  public partial class UsersModuleTests {
    AppBootstrapper _bootstrapper;
    IHandler<GetCurrentUserRequest, User> _getCurrentUserHandler;
    IHandler<LoginLocalUserRequest, bool> _loginLocalUserHandler;
    IHandler<LoginGooglePlusUserRequest, bool> _loginGooglePlusUserHandler;
    IHandler<LogoutRequest, bool> _logoutHandler;
    INancySecurityContextFactory _nancySecurityContextFactory;
    Browser _browser;
    UsersModule _sut;
    User _authenticatedUser;

    [SetUp]
    public virtual void SetUp() {
      _getCurrentUserHandler = _getCurrentUserHandler.Fake();
      _loginLocalUserHandler = _loginLocalUserHandler.Fake();
      _loginGooglePlusUserHandler = _loginGooglePlusUserHandler.Fake();
      _logoutHandler = _logoutHandler.Fake();
      _nancySecurityContextFactory = _nancySecurityContextFactory.Fake();
      _sut = new UsersModule(_getCurrentUserHandler, _loginLocalUserHandler, _loginGooglePlusUserHandler, _logoutHandler, _nancySecurityContextFactory);
      _bootstrapper = new AppBootstrapper(with => {
        with.Module(_sut);
        with.RootPathProvider(new VoterRootPathProvider());
      });
      _browser = new Browser(_bootstrapper, to => to.Accept(new MediaRange("application/json")));

      _authenticatedUser = new User {
        FirstName = "John",
        LastName = "Doe",
        Login = new Login {Value = "JDoe"},
        Password = new Password {
          Value = "P@$$w0rd",
          IsEncrypted = false
        }
      };
    }

    [TestFixture]
    public class Construction : UsersModuleTests {
      [Test]
      public void ConstuctorTests() {
        Assert.That(_sut.NoDependenciesAreOptional());
      }
    }

    void ConfigureSecurityContextFactory_ToReturn(ISecurityContext securityContext) {
      A.CallTo(() => _nancySecurityContextFactory.Create(A<NancyContext>._))
       .Returns(securityContext);
    }
  }
}