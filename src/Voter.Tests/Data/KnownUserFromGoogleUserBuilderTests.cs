using System;
using DavidLievrouw.Utils.ForTesting.FluentAssertions;
using DavidLievrouw.Voter.Data.Records;
using FluentAssertions;
using NUnit.Framework;

namespace DavidLievrouw.Voter.Data {
  [TestFixture]
  public class KnownUserFromGoogleUserBuilderTests {
    KnownUserFromGoogleUserBuilder _sut;

    [SetUp]
    public virtual void SetUp() {
      _sut = new KnownUserFromGoogleUserBuilder();
    }

    [TestFixture]
    public class Construction : KnownUserFromGoogleUserBuilderTests {
      [Test]
      public void HasExactlyOneConstructor_WithNoOptionalParameters() {
        _sut.Should().HaveExactlyOneConstructorWithoutOptionalParameters();
      }
    }

    [TestFixture]
    public class BuildKnownUser : KnownUserFromGoogleUserBuilderTests {
      GoogleUserDataRecord _googleUser;

      [SetUp]
      public override void SetUp() {
        base.SetUp();
        _googleUser = new GoogleUserDataRecord {
          Id = "G000123",
          Given_name = "Pol",
          Family_name = "Tak"
        };
      }

      [Test]
      public void GivenNullGoogleUser_Throws() {
        Action act = () => _sut.BuildKnownUser(null);
        act.ShouldThrow<ArgumentNullException>();
      }

      [Test]
      public void GivenGoogleUserWithoutCorrelationId_Throws() {
        _googleUser.Id = null;
        Action act = () => _sut.BuildKnownUser(_googleUser);
        act.ShouldThrow<InvalidOperationException>();
      }

      [Test]
      public void GivenGoogleUserWithEmptyCorrelationId_Throws() {
        _googleUser.Id = string.Empty;
        Action act = () => _sut.BuildKnownUser(_googleUser);
        act.ShouldThrow<InvalidOperationException>();
      }

      [Test]
      public void CreatesNewGoogleUserWithUniqueIdEveryTime() {
        var u1 = _sut.BuildKnownUser(_googleUser);
        var u2 = _sut.BuildKnownUser(_googleUser);
        u1.UniqueId.Should().NotBe(u2.UniqueId);
      }

      [Test]
      public void SetsKnownDataFromGoogleUserInNewKnownUser() {
        var actual = _sut.BuildKnownUser(_googleUser);
        actual.UniqueId.Should().NotBeEmpty();
        actual.ExternalCorrelationId.Should().Be(_googleUser.Id);
        actual.FirstName.Should().Be(_googleUser.Given_name);
        actual.LastName.Should().Be(_googleUser.Family_name);
      }
    }
  }
}