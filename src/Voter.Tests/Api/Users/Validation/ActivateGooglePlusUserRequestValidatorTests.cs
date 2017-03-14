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
    public void NullIdToken_IsInvalid() {
      var input = new ActivateGooglePlusUserRequest {
        IdToken = null,
        SecurityContext = A.Dummy<ISecurityContext>()
      };
      var actualResult = _sut.Validate(input);
      Assert.That(actualResult.IsValid, Is.False);
    }

    [Test]
    public void NullSecurityContext_IsInvalid() {
      var input = new ActivateGooglePlusUserRequest {
        IdToken = "TheIdToken",
        SecurityContext = null
      };
      var actualResult = _sut.Validate(input);
      Assert.That(actualResult.IsValid, Is.False);
    }

    [Test]
    public void EmptyIdToken_IsInvalid() {
      var input = new ActivateGooglePlusUserRequest {
        IdToken = string.Empty,
        SecurityContext = A.Dummy<ISecurityContext>()
      };
      var actualResult = _sut.Validate(input);
      Assert.That(actualResult.IsValid, Is.False);
    }

    [Test]
    public void ValidCommand_IsValid() {
      var input = new ActivateGooglePlusUserRequest {
        IdToken = "TheIdToken",
        SecurityContext = A.Dummy<ISecurityContext>()
      };
      var actualResult = _sut.Validate(input);
      Assert.That(actualResult.IsValid, Is.True);
    }
  }
}