using System.Threading.Tasks;
using DavidLievrouw.Utils;
using DavidLievrouw.Utils.ForTesting.FakeItEasy;
using DavidLievrouw.Utils.ForTesting.FluentAssertions;
using DavidLievrouw.Voter.Api.Users.Models;
using DavidLievrouw.Voter.Domain.DTO;
using DavidLievrouw.Voter.Security;
using FakeItEasy;
using FluentAssertions;
using NUnit.Framework;

namespace DavidLievrouw.Voter.Api.Users.Handlers {
  [TestFixture]
  public class GetCurrentUserHandlerTests {
    GetCurrentUserHandler _sut;
    IAdapter<User, Api.Models.User> _userAdapter;
    ISecurityContext _securityContext;

    [SetUp]
    public void SetUp() {
      _userAdapter = _userAdapter.Fake();
      _sut = new GetCurrentUserHandler(_userAdapter);
      _securityContext = _securityContext.Fake();
    }

    [TestFixture]
    public class Construction : GetCurrentUserHandlerTests {
      [Test]
      public void HasExactlyOneConstructor_WithNoOptionalParameters() {
        _sut.Should().HaveExactlyOneConstructorWithoutOptionalParameters();
      }
    }

    [TestFixture]
    public class Handle : GetCurrentUserHandlerTests {
      [Test]
      public async Task GivenContextWithUser_ReturnsAdaptedUser() {
        var user = new User {Login = new Login {Value = "JohnD"}, LastName = "Doe"};
        ConfigureSecurityContext_ToReturn(user);
        var request = new GetCurrentUserRequest {
          SecurityContext = _securityContext
        };

        var expected = new Api.Models.User { LastName = "Doe" };
        A.CallTo(() => _userAdapter.Adapt(user)).Returns(expected);

        var actual = await _sut.Handle(request);

        actual.ShouldBeEquivalentTo(expected);
      }
    }

    void ConfigureSecurityContext_ToReturn(User user) {
      A.CallTo(() => _securityContext.GetAuthenticatedUser())
       .Returns(user);
    }
  }
}