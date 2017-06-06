using DavidLievrouw.Utils.ForTesting.DotNet;
using DavidLievrouw.Voter.Api.Users.Models;
using DavidLievrouw.Voter.Security;
using FakeItEasy;
using NUnit.Framework;

namespace DavidLievrouw.Voter.Api.Users.Handlers {
  [TestFixture]
  public class LogoutHandlerTests {
    LogoutHandler _sut;

    [SetUp]
    public void SetUp() {
      _sut = new LogoutHandler();
    }

    [TestFixture]
    public class Construction : LogoutHandlerTests {
      [Test]
      public void ConstructorTests() {
        Assert.That(_sut.NoDependenciesAreOptional());
      }
    }

    [TestFixture]
    public class Handle : LogoutHandlerTests {
      [Test]
      public void DelegatesControlToAuthenticatedUserApplyer() {
        var securityContext = A.Fake<ISecurityContext>();
        var command = new LogoutRequest {
          SecurityContext = securityContext
        };

        _sut.Handle(command).Wait();

        A.CallTo(() => securityContext.SetAuthenticatedUser(null))
         .MustHaveHappened();
      }
    }
  }
}