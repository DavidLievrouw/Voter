using DavidLievrouw.Utils.ForTesting.FakeItEasy;
using DavidLievrouw.Utils.ForTesting.FluentAssertions;
using DavidLievrouw.Voter.Data.Dapper;
using FluentAssertions;
using NUnit.Framework;

namespace DavidLievrouw.Voter.App.Handlers {
  [TestFixture]
  public class LoginViewModelHandlerTests {
    IQueryExecutor _queryExecutor;
    LoginViewModelHandler _sut;

    [SetUp]
    public virtual void SetUp() {
      _queryExecutor = _queryExecutor.Fake();
      _sut = new LoginViewModelHandler(_queryExecutor);
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