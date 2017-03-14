using System;
using DavidLievrouw.Voter.Api.Users.Models;
using DavidLievrouw.Voter.Domain.DTO;
using DavidLievrouw.Voter.Security;
using FakeItEasy;
using NUnit.Framework;

namespace DavidLievrouw.Voter.Api.Users.Validation {
  [TestFixture]
  public class DisconnectGooglePlusUserRequestValidatorTests {
    DisconnectGooglePlusUserRequestValidator _sut;

    [SetUp]
    public void SetUp() {
      _sut = new DisconnectGooglePlusUserRequestValidator();
    }

    [Test]
    public void NullValue_IsInvalid() {
      var actualResult = _sut.Validate((DisconnectGooglePlusUserRequest) null);
      Assert.That(actualResult.IsValid, Is.False);
    }

    [Test]
    public void NullSecurityContext_IsInvalid() {
      var input = new DisconnectGooglePlusUserRequest {
        SecurityContext = null
      };
      var actualResult = _sut.Validate(input);
      Assert.That(actualResult.IsValid, Is.False);
    }

    [Test]
    public void SecurityContextWithNonGoogleUser_IsInvalid() {
      var securityContext = A.Fake<ISecurityContext>();
      var localUser = new User {UniqueId = Guid.NewGuid(), Type = UserType.Local};
      A.CallTo(() => securityContext.GetAuthenticatedUser()).Returns(localUser);

      var input = new DisconnectGooglePlusUserRequest {
        SecurityContext = securityContext
      };

      var actualResult = _sut.Validate(input);
      Assert.That(actualResult.IsValid, Is.False);
    }

    [Test]
    public void ValidRequest_IsValid() {
      var securityContext = A.Fake<ISecurityContext>();
      var googleUser = new User {UniqueId = Guid.NewGuid(), Type = UserType.GooglePlus};
      A.CallTo(() => securityContext.GetAuthenticatedUser()).Returns(googleUser);

      var input = new DisconnectGooglePlusUserRequest {
        SecurityContext = securityContext
      };
      var actualResult = _sut.Validate(input);
      Assert.That(actualResult.IsValid, Is.True);
    }
  }
}