using System;
using System.Linq;
using System.Security;
using System.Threading.Tasks;
using DavidLievrouw.Utils;
using DavidLievrouw.Utils.ForTesting.FakeItEasy;
using DavidLievrouw.Utils.ForTesting.FluentAssertions;
using DavidLievrouw.Voter.Data;
using DavidLievrouw.Voter.Data.Records;
using DavidLievrouw.Voter.Domain.DTO;
using FakeItEasy;
using FluentAssertions;
using NUnit.Framework;

namespace DavidLievrouw.Voter.Domain {
  [TestFixture]
  public class UserServiceTests {
    IAdapter<KnownUserRecord, User> _userAdapter;
    IKnownUserDataService _knownUserDataService;
    IGoogleUserDataService _googleUserDataService;
    UserService _sut;

    [SetUp]
    public virtual void SetUp() {
      _userAdapter = _userAdapter.Fake();
      _knownUserDataService = _knownUserDataService.Fake();
      _googleUserDataService = _googleUserDataService.Fake();
      _sut = new UserService(_knownUserDataService, _googleUserDataService, _userAdapter);
    }

    [TestFixture]
    public class Construction : UserServiceTests {
      [Test]
      public void HasExactlyOneConstructor_WithNoOptionalParameters() {
        _sut.Should().HaveExactlyOneConstructorWithoutOptionalParameters();
      }
    }

    [TestFixture]
    public class GetKnownUserById : UserServiceTests {
      Guid _uniqueId;

      [SetUp]
      public override void SetUp() {
        base.SetUp();
        _uniqueId = Guid.NewGuid();
      }

      [Test]
      public void WhenNoUserIsFound_Throws() {
        A.CallTo(() => _knownUserDataService.GetKnownUserById(_uniqueId)).Returns((KnownUserRecord) null);
        Func<Task> act = () => _sut.GetKnownUserById(_uniqueId);
        act.ShouldThrow<InvalidOperationException>();
      }

      [Test]
      public async Task ReturnsAdaptedDataRecord() {
        var knownUserRecord = new KnownUserRecord {UniqueId = _uniqueId};
        A.CallTo(() => _knownUserDataService.GetKnownUserById(_uniqueId)).Returns(knownUserRecord);
        var domainUser = new User {UniqueId = _uniqueId};
        A.CallTo(() => _userAdapter.Adapt(knownUserRecord)).Returns(domainUser);

        var actual = await _sut.GetKnownUserById(_uniqueId);

        actual.Should().Be(domainUser);
      }
    }

    [TestFixture]
    public class ActivateGooglePlusUser : UserServiceTests {
      string _accessToken;

      [SetUp]
      public override void SetUp() {
        base.SetUp();
        _accessToken = "AT_002";
      }

      [Test]
      public void GivenNullAccessToken_Throws() {
        Func<Task> act = () => _sut.ActivateGooglePlusUser(null);
        act.ShouldThrow<ArgumentNullException>();
      }

      [Test]
      public void WhenActivatedUserCannotBeFound_ThrowsSecurityException() {
        A.CallTo(() => _knownUserDataService.FindKnownUserByCorrelationId(A<char>._, A<string>._)).Returns(Enumerable.Empty<KnownUserRecord>());
        Func<Task> act = () => _sut.ActivateGooglePlusUser(_accessToken);
        act.ShouldThrow<SecurityException>();
      }

      [Test]
      public void WhenMultipleMatchingUsersAreFound_ThrowsSecurityException() {
        var multipleMatches = new[] {
          new KnownUserRecord(),
          new KnownUserRecord()
        };
        A.CallTo(() => _knownUserDataService.FindKnownUserByCorrelationId(A<char>._, A<string>._)).Returns(multipleMatches);
        Func<Task> act = () => _sut.ActivateGooglePlusUser(_accessToken);
        act.ShouldThrow<SecurityException>();
      }

      [Test]
      public async Task ReturnsActivatedUser() {
        var correlationIdFromGoogle = "987654321";
        A.CallTo(() => _googleUserDataService.ActivateGooglePlusUser(_accessToken)).Returns(correlationIdFromGoogle);
        var knownUser = new KnownUserRecord {UniqueId = Guid.NewGuid(), Type = 'G'};
        A.CallTo(() => _knownUserDataService.FindKnownUserByCorrelationId('G', correlationIdFromGoogle)).Returns(knownUser.AsEnumerable());
        var domainUser = new User {UniqueId = knownUser.UniqueId};
        A.CallTo(() => _userAdapter.Adapt(knownUser)).Returns(domainUser);

        var actual = await _sut.ActivateGooglePlusUser(_accessToken);

        actual.Should().Be(domainUser);
      }
    }
  }
}