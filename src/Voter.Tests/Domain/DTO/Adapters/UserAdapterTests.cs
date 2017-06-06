using System;
using DavidLievrouw.Utils.ForTesting.FluentAssertions;
using DavidLievrouw.Voter.Data.Records;
using FluentAssertions;
using NUnit.Framework;

namespace DavidLievrouw.Voter.Domain.DTO.Adapters {
  [TestFixture]
  public class UserAdapterTests {
    UserAdapter _sut;

    [SetUp]
    public virtual void SetUp() {
      _sut = new UserAdapter();
    }

    [TestFixture]
    public class Construction : UserAdapterTests {
      [Test]
      public void HasExactlyOneConstructor_WithNoOptionalParameters() {
        _sut.Should().HaveExactlyOneConstructorWithoutOptionalParameters();
      }
    }

    [TestFixture]
    public class Adapt : UserAdapterTests {
      KnownUserRecord _knownUserRecord;

      [SetUp]
      public override void SetUp() {
        base.SetUp();
        _knownUserRecord = new KnownUserRecord {
          FirstName = "David",
          LastName = "Lievrouw",
          UniqueId = Guid.NewGuid(),
          LastNamePrefix = "K.",
          ExternalCorrelationId = "ABC123",
          Password = "P@ss",
          Login = "Administrator1",
          Type = 'L',
          Salt = "A little bit of pepper"
        };
      }

      [Test]
      public void GivenInputNull_ReturnsNull() {
        _sut.Adapt(null).Should().BeNull();
      }

      [Test]
      public void AdaptsFirstName() {
        _sut.Adapt(_knownUserRecord).FirstName.Should().Be(_knownUserRecord.FirstName);
      }

      [Test]
      public void AdaptsUniqueId() {
        _sut.Adapt(_knownUserRecord).UniqueId.Should().Be(_knownUserRecord.UniqueId);
      }

      [Test]
      public void AdaptsLastName() {
        _sut.Adapt(_knownUserRecord).LastName.Should().Be(_knownUserRecord.LastName);
      }

      [Test]
      public void AdaptsLastNamePrefix() {
        _sut.Adapt(_knownUserRecord).LastNamePrefix.Should().Be(_knownUserRecord.LastNamePrefix);
      }

      [Test]
      public void AdaptsExternalCorrelationId() {
        _sut.Adapt(_knownUserRecord).ExternalCorrelationId.ShouldBeEquivalentTo(new ExternalCorrelationId {
          Value = _knownUserRecord.ExternalCorrelationId
        });
      }

      [Test]
      public void AdaptsPassword() {
        _sut.Adapt(_knownUserRecord).Password.ShouldBeEquivalentTo(new Password {
          Value = _knownUserRecord.Password,
          Salt = _knownUserRecord.Salt,
          IsEncrypted = true
        });
      }

      [Test]
      public void AdaptsLogin() {
        _sut.Adapt(_knownUserRecord).Login.ShouldBeEquivalentTo(new Login {
          Value = _knownUserRecord.Login
        });
      }

      [TestCase('L', UserType.Local)]
      [TestCase('G', UserType.GooglePlus)]
      public void AdaptsType(char dataRecordValue, UserType expected) {
        _knownUserRecord.Type = dataRecordValue;
        _sut.Adapt(_knownUserRecord).Type.Should().Be(expected);
      }

      [Test]
      public void GivenInvalidUserType_Throws() {
        _knownUserRecord.Type = '\"';
        Action act = () => _sut.Adapt(_knownUserRecord);
        act.ShouldThrow<NotSupportedException>();
      }
    }
  }
}