using DavidLievrouw.Utils.ForTesting.FakeItEasy;
using DavidLievrouw.Utils.ForTesting.FluentAssertions;
using DavidLievrouw.Voter.App.ApplicationInfo;
using DavidLievrouw.Voter.Configuration;
using FluentAssertions;
using NUnit.Framework;

namespace DavidLievrouw.Voter.App.Login.Handlers {
  [TestFixture]
  public class LoginViewModelHandlerTests {
    IApplicationInfoProvider _applicationInfoProvider;
    ICustomJsonSerializer _customJsonSerializer;
    LoginViewModelHandler _sut;

    [SetUp]
    public virtual void SetUp() {
      _applicationInfoProvider = _applicationInfoProvider.Fake();
      _customJsonSerializer = _customJsonSerializer.Fake();
      _sut = new LoginViewModelHandler(_applicationInfoProvider, _customJsonSerializer);
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