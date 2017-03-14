using Nancy;
using NUnit.Framework;

namespace DavidLievrouw.Voter.App.Validation {
  [TestFixture]
  public class NancyContextValidatorTests {
    NancyContextValidator _sut;

    [SetUp]
    public void SetUp() {
      _sut = new NancyContextValidator();
    }

    [Test]
    public void NullValue_IsInvalid() {
      var actualResult = _sut.Validate((NancyContext) null);
      Assert.That(actualResult.IsValid, Is.False);
    }

    [Test]
    public void ValidRequest_IsValid() {
      var input = new NancyContext();
      var actualResult = _sut.Validate(input);
      Assert.That(actualResult.IsValid, Is.True);
    }
  }
}