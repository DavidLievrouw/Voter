using DavidLievrouw.Utils.ForTesting.FluentAssertions;
using FluentAssertions;
using NUnit.Framework;

namespace DavidLievrouw.Voter.Data.WebRequestSender {
  [TestFixture]
  public class HttpClientFactoryTests {
    HttpClientFactory _sut;

    [SetUp]
    public virtual void SetUp() {
      _sut = new HttpClientFactory();
    }

    [TestFixture]
    public class Construction : HttpClientFactoryTests {
      [Test]
      public void HasExactlyOneConstructor_WithNoOptionalParameters() {
        _sut.Should().HaveExactlyOneConstructorWithoutOptionalParameters();
      }
    }

    [TestFixture]
    public class Create : HttpClientFactoryTests {
      [Test]
      public void CreatesNewRealHttpClient() {
        var actual = _sut.Create();
        actual.Should().NotBeNull().And.BeAssignableTo<IHttpClient>();
      }
    }
  }
}