using DavidLievrouw.Utils.ForTesting.FakeItEasy;
using DavidLievrouw.Utils.ForTesting.FluentAssertions;
using DavidLievrouw.Voter.Data;
using FluentAssertions;
using NUnit.Framework;

namespace DavidLievrouw.Voter.App.Handlers {
  [TestFixture]
  public class LoginViewModelHandlerTests {
    IUserDataService _userDataService;
    LoginViewModelHandler _sut;

    [SetUp]
    public virtual void SetUp() {
      _userDataService = _userDataService.Fake();
      _sut = new LoginViewModelHandler(_userDataService);
    }

    [TestFixture]
    public class Construction : LoginViewModelHandlerTests {
      [Test]
      public void HasExactlyOneConstructor_WithNoOptionalParameters() {
        _sut.Should().HaveExactlyOneConstructorWithoutOptionalParameters();
      }
    }

    [TestFixture]
    public class Handle : LoginViewModelHandlerTests {
      // ToDo: Real implementation, get rid of the spike
    }
  }
}