using DavidLievrouw.Voter.Api.Users.Models;
using DavidLievrouw.Voter.Security;
using FakeItEasy;
using NUnit.Framework;

namespace DavidLievrouw.Voter.Api.Users.Validation {
  [TestFixture]
  public class LoginLocalUserRequestValidatorTests {
    LoginLocalUserRequestValidator _sut;

    [SetUp]
    public void SetUp() {
      _sut = new LoginLocalUserRequestValidator();
    }

    [Test]
    public void NullValue_IsInvalid() {
      var actualResult = _sut.Validate((LoginLocalUserRequest) null);
      Assert.That(actualResult.IsValid, Is.False);
    }

    [Test]
    public void NullLogin_IsInvalid() {
      var input = new LoginLocalUserRequest {
        Login = null,
        Password = "ThePassword",
        SecurityContext = A.Dummy<ISecurityContext>()
      };
      var actualResult = _sut.Validate(input);
      Assert.That(actualResult.IsValid, Is.False);
    }

    [Test]
    public void NullSecurityContext_IsInvalid() {
      var input = new LoginLocalUserRequest {
        Login = "TheLogin",
        Password = "ThePassword",
        SecurityContext = null
      };
      var actualResult = _sut.Validate(input);
      Assert.That(actualResult.IsValid, Is.False);
    }

    [Test]
    public void NullPassword_IsInvalid() {
      var input = new LoginLocalUserRequest {
        Login = "TheLogin",
        Password = null,
        SecurityContext = A.Dummy<ISecurityContext>()
      };
      var actualResult = _sut.Validate(input);
      Assert.That(actualResult.IsValid, Is.False);
    }

    [Test]
    public void EmptyLogin_IsValid() {
      var input = new LoginLocalUserRequest {
        Login = string.Empty,
        Password = "ThePassword",
        SecurityContext = A.Dummy<ISecurityContext>()
      };
      var actualResult = _sut.Validate(input);
      Assert.That(actualResult.IsValid, Is.True);
    }

    [Test]
    public void EmptyPassword_IsValid() {
      var input = new LoginLocalUserRequest {
        Login = "TheLogin",
        Password = string.Empty,
        SecurityContext = A.Dummy<ISecurityContext>()
      };
      var actualResult = _sut.Validate(input);
      Assert.That(actualResult.IsValid, Is.True);
    }

    [Test]
    public void ValidCommand_IsValid() {
      var input = new LoginLocalUserRequest {
        Login = "TheLogin",
        Password = "ThePassword",
        SecurityContext = A.Dummy<ISecurityContext>()
      };
      var actualResult = _sut.Validate(input);
      Assert.That(actualResult.IsValid, Is.True);
    }
  }
}