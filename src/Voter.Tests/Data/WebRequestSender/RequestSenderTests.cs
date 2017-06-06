using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using DavidLievrouw.Utils.ForTesting.FakeItEasy;
using DavidLievrouw.Utils.ForTesting.FluentAssertions;
using FakeItEasy;
using FluentAssertions;
using NUnit.Framework;

namespace DavidLievrouw.Voter.Data.WebRequestSender {
  [TestFixture]
  public class RequestSenderTests {
    IHttpClientFactory _httpClientFactory;
    RequestSender _sut;

    [SetUp]
    public virtual void SetUp() {
      _httpClientFactory = _httpClientFactory.Fake();
      _sut = new RequestSender(_httpClientFactory);
    }

    [TestFixture]
    public class Construction : RequestSenderTests {
      [Test]
      public void ConstructorTests() {
        _sut.Should().HaveExactlyOneConstructorWithoutOptionalParameters();
      }
    }

    [TestFixture]
    public class SendAsyncWithoutTimeout : RequestSenderTests {
      IHttpClient _httpClient;

      [SetUp]
      public override void SetUp() {
        base.SetUp();
        _httpClient = A.Fake<IHttpClient>();
        A.CallTo(() => _httpClientFactory.Create()).Returns(_httpClient);
      }

      [Test]
      public async Task GivenNoTimeout_WhenResponseIsReceived_ShouldReturnIt() {
        var httpResponseMessage = A.Dummy<HttpResponseMessage>();
        A.CallTo(() => _httpClient.SendAsync(A<HttpRequestMessage>._)).Returns(httpResponseMessage);
        var actual = await _sut.SendAsync(A.Dummy<HttpRequestMessage>(), TimeSpan.Zero);
        Assert.That(actual, Is.EqualTo(httpResponseMessage));
      }

      [Test]
      public async Task GivenNegativeTimeout_WhenResponseIsReceived_ShouldReturnIt() {
        var httpResponseMessage = A.Dummy<HttpResponseMessage>();
        A.CallTo(() => _httpClient.SendAsync(A<HttpRequestMessage>._)).Returns(httpResponseMessage);
        var actual = await _sut.SendAsync(A.Dummy<HttpRequestMessage>(), TimeSpan.MinValue);
        Assert.That(actual, Is.EqualTo(httpResponseMessage));
      }
    }

    [TestFixture]
    public class SendAsyncWithTimeout : RequestSenderTests {
      IHttpClient _httpClient;
      TimeSpan _timeout;

      [SetUp]
      public override void SetUp() {
        base.SetUp();
        _httpClient = A.Fake<IHttpClient>();
        _timeout = TimeSpan.FromSeconds(10);
        A.CallTo(() => _httpClientFactory.Create()).Returns(_httpClient);
      }

      [Test]
      public async Task WhenResponseIsReceived_ShouldReturnIt() {
        var httpResponseMessage = A.Dummy<HttpResponseMessage>();
        A.CallTo(() => _httpClient.SendAsync(A<HttpRequestMessage>._, A<CancellationToken>._)).Returns(httpResponseMessage);
        var actual = await _sut.SendAsync(A.Dummy<HttpRequestMessage>(), _timeout);
        Assert.That(actual, Is.EqualTo(httpResponseMessage));
      }

      [Test]
      public void WhenTimeoutExpires_ShouldThrowOperationCanceledException() {
        A.CallTo(() => _httpClient.SendAsync(A<HttpRequestMessage>._, A<CancellationToken>._)).Throws(new OperationCanceledException());
        Func<Task> action = () => _sut.SendAsync(A.Dummy<HttpRequestMessage>(), _timeout);
        action.ShouldThrow<OperationCanceledException>();
      }
    }
  }
}