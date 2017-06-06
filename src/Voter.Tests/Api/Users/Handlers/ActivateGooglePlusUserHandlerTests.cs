using System;
using System.Threading.Tasks;
using DavidLievrouw.Utils.ForTesting.FakeItEasy;
using DavidLievrouw.Utils.ForTesting.FluentAssertions;
using DavidLievrouw.Voter.Api.Users.Models;
using DavidLievrouw.Voter.Domain;
using DavidLievrouw.Voter.Domain.DTO;
using DavidLievrouw.Voter.Security;
using FakeItEasy;
using FluentAssertions;
using NUnit.Framework;

namespace DavidLievrouw.Voter.Api.Users.Handlers {
  [TestFixture]
  public class ActivateGooglePlusUserHandlerTests {
    IUserService _userService;
    ActivateGooglePlusUserHandler _sut;

    [SetUp]
    public virtual void SetUp() {
      _userService = _userService.Fake();
      _sut = new ActivateGooglePlusUserHandler(_userService);
    }

    [TestFixture]
    public class Construction : ActivateGooglePlusUserHandlerTests {
      [Test]
      public void HasExactlyOneConstructor_WithNoOptionalParameters() {
        _sut.Should().HaveExactlyOneConstructorWithoutOptionalParameters();
      }
    }

    [TestFixture]
    public class Handle : ActivateGooglePlusUserHandlerTests {
      ActivateGooglePlusUserRequest _request;

      [SetUp]
      public override void SetUp() {
        base.SetUp();
        _request = new ActivateGooglePlusUserRequest {
          AccessToken = "AT_001",
          SecurityContext = new FakeSecurityContext()
        };
      }

      [Test]
      public async Task WhenUserIsAlreadyLoggedIn_ReturnsTrue_DoesNotDoAnythingElse() {
        var authenticatedUser = new User {UniqueId = Guid.NewGuid()};
        _request.SecurityContext.SetAuthenticatedUser(authenticatedUser);

        var actual = await _sut.Handle(_request);

        actual.Should().BeTrue();
        A.CallTo(() => _userService.ActivateGooglePlusUser(A<string>._)).MustNotHaveHappened();
      }

      [Test]
      public async Task WhenUserIsNotYetLoggedIn_ActivatesUser() {
        await _sut.Handle(_request);
        A.CallTo(() => _userService.ActivateGooglePlusUser(_request.AccessToken)).MustHaveHappened();
      }

      [Test]
      public async Task WhenUserIsNotYetLoggedIn_SetsActivatedUserAsLoggedInUser() {
        var activatedUser = new User {UniqueId = Guid.NewGuid(), ExternalCorrelationId = new ExternalCorrelationId {Value = "XYZ "}};
        A.CallTo(() => _userService.ActivateGooglePlusUser(_request.AccessToken)).Returns(activatedUser);

        var actual = await _sut.Handle(_request);

        actual.Should().BeTrue();
        _request.SecurityContext.GetAuthenticatedUser().Should().Be(activatedUser);
      }
    }
  }
}