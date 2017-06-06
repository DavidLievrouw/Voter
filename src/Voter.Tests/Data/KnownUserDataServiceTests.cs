using DavidLievrouw.Utils.ForTesting.FakeItEasy;
using DavidLievrouw.Utils.ForTesting.FluentAssertions;
using DavidLievrouw.Voter.Data.Dapper;
using FluentAssertions;
using NUnit.Framework;

namespace DavidLievrouw.Voter.Data {
  [TestFixture]
  public class KnownUserDataServiceTests {
    IQueryExecutor _queryExecutor;
    KnownUserDataService _sut;

    [SetUp]
    public virtual void SetUp() {
      _queryExecutor = _queryExecutor.Fake();
      _sut = new KnownUserDataService(_queryExecutor);
    }

    [TestFixture]
    public class Construction : KnownUserDataServiceTests {
      [Test]
      public void HasExactlyOneConstructor_WithNoOptionalParameters() {
        _sut.Should().HaveExactlyOneConstructorWithoutOptionalParameters();
      }
    }
  }
}