using DavidLievrouw.Voter.Api.Users.Models;
using DavidLievrouw.Voter.Security;
using FakeItEasy;
using NUnit.Framework;

namespace DavidLievrouw.Voter.Api.Users.Validation {
  [TestFixture]
  public class ActivateGooglePlusUserRequestValidatorTests {
    ActivateGooglePlusUserRequestValidator _sut;

    [SetUp]
    public void SetUp() {
      _sut = new ActivateGooglePlusUserRequestValidator();
    }

    [Test]
    public void NullValue_IsInvalid() {
      var actualResult = _sut.Validate((ActivateGooglePlusUserRequest) null);
      Assert.That(actualResult.IsValid, Is.False);
    }

    [Test]
    public void NullAccessToken_IsInvalid() {
      var input = new ActivateGooglePlusUserRequest {
        AccessToken = null,
        SecurityContext = A.Dummy<ISecurityContext>()
      };
      var actualResult = _sut.Validate(input);
      Assert.That(actualResult.IsValid, Is.False);
    }

    [Test]
    public void NullSecurityContext_IsInvalid() {
      var input = new ActivateGooglePlusUserRequest {
        AccessToken = "TheAccessToken",
        SecurityContext = null
      };
      var actualResult = _sut.Validate(input);
      Assert.That(actualResult.IsValid, Is.False);
    }

    [Test]
    public void EmptyAccessToken_IsInvalid() {
      var input = new ActivateGooglePlusUserRequest {
        AccessToken = string.Empty,
        SecurityContext = A.Dummy<ISecurityContext>()
      };
      var actualResult = _sut.Validate(input);
      Assert.That(actualResult.IsValid, Is.False);
    }

    [Test]
    public void ValidCommand_IsValid() {
      var input = new ActivateGooglePlusUserRequest {
        AccessToken = "TheAccessToken",
        SecurityContext = A.Dummy<ISecurityContext>()
      };
      var actualResult = _sut.Validate(input);
      Assert.That(actualResult.IsValid, Is.True);
    }
  }
}