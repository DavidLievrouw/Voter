using FluentAssertions;
using Nancy;
using NUnit.Framework;

namespace DavidLievrouw.Voter {
  [TestFixture]
  public class PassThroughDeciderTests {
    [TestFixture]
    public class ConfigureAndPerformPassThroughIfNeeded : PassThroughDeciderTests {
      NancyContext _nancyContext;

      [SetUp]
      public void SetUp() {
        _nancyContext = new NancyContext {
          Response = new Response { StatusCode = HttpStatusCode.OK }
        };
      }

      [Test]
      public void GivenNullContext_ReturnsFalse() {
        PassThroughDecider.ConfigureAndPerformPassThroughIfNeeded(null).Should().BeFalse();
      }

      [Test]
      public void GivenContextWithoutResponse_ReturnsFalse() {
        _nancyContext.Response = null;
        PassThroughDecider.ConfigureAndPerformPassThroughIfNeeded(_nancyContext).Should().BeFalse();
      }

      [Test]
      public void GivenContextWith404Response_ReturnsTrue() {
        _nancyContext.Response.StatusCode = HttpStatusCode.NotFound;
        PassThroughDecider.ConfigureAndPerformPassThroughIfNeeded(_nancyContext).Should().BeTrue();
      }

      [Test]
      public void GivenContextWithNon404Response_ReturnsFalse() {
        _nancyContext.Response.StatusCode = HttpStatusCode.OK;
        PassThroughDecider.ConfigureAndPerformPassThroughIfNeeded(_nancyContext).Should().BeFalse();
      }
    }
  }
}