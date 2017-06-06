using System;
using DavidLievrouw.Utils.ForTesting.FluentAssertions;
using DavidLievrouw.Voter.Api.Users.Models.Adapters;
using DavidLievrouw.Voter.Domain.DTO;
using FluentAssertions;
using NUnit.Framework;

namespace DavidLievrouw.Voter.Api.Users.Models {
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
      User _dtoUser;

      [SetUp]
      public override void SetUp() {
        base.SetUp();
        _dtoUser = new User {
          FirstName = "David",
          LastName = "Lievrouw",
          UniqueId = Guid.NewGuid(),
          LastNamePrefix = "K."
        };
      }

      [Test]
      public void GivenInputNull_ReturnsNull() {
        _sut.Adapt(null).Should().BeNull();
      }

      [Test]
      public void AdaptsFirstName() {
        _sut.Adapt(_dtoUser).FirstName.Should().Be(_dtoUser.FirstName);
      }

      [Test]
      public void AdaptsUniqueId() {
        _sut.Adapt(_dtoUser).UniqueId.Should().Be(_dtoUser.UniqueId);
      }

      [Test]
      public void AdaptsLastName() {
        _sut.Adapt(_dtoUser).LastName.Should().Be(_dtoUser.LastName);
      }

      [Test]
      public void AdaptsLastNamePrefix() {
        _sut.Adapt(_dtoUser).LastNamePrefix.Should().Be(_dtoUser.LastNamePrefix);
      }
    }
  }
}