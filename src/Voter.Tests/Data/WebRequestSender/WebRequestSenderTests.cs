using System;
using System.Net.Http;
using System.Threading.Tasks;
using DavidLievrouw.Utils.ForTesting.FakeItEasy;
using DavidLievrouw.Utils.ForTesting.FluentAssertions;
using FakeItEasy;
using FakeItEasy.Configuration;
using FluentAssertions;
using NUnit.Framework;

namespace DavidLievrouw.Voter.Data.WebRequestSender {
  [TestFixture]
  public class WebRequestSenderTests {
    IRequestSender _requestSender;
    WebRequestSender _sut;

    [SetUp]
    public virtual void SetUp() {
      _requestSender = _requestSender.Fake();
      _sut = new WebRequestSender(_requestSender);
    }

    [TestFixture]
    public class Construction : WebRequestSenderTests {
      [Test]
      public void HasExactlyOneConstructor_WithNoOptionalParameters() {
        _sut.Should().HaveExactlyOneConstructorWithoutOptionalParameters();
      }
    }

    [TestFixture]
    public class NewRequest : WebRequestSenderTests {
      HttpMethod _method;
      Uri _uri;

      [SetUp]
      public override void SetUp() {
        base.SetUp();
        _method = HttpMethod.Post;
        _uri = new Uri("http://localhost");
      }

      [Test]
      public void GivenNullMethod_Throws() {
        Assert.Throws<ArgumentNullException>(() => _sut.NewRequest(null, _uri));
      }

      [Test]
      public void GivenNullUri_Throws() {
        Assert.Throws<ArgumentNullException>(() => _sut.NewRequest(_method, null));
      }

      [Test]
      public void ReturnsResultFromMessageBuilderFactory() {
        var actual = _sut.NewRequest(_method, _uri);
        actual.Should().NotBeNull().And.BeAssignableTo<HttpRequestMessageBuilder>();
      }
    }

    [TestFixture]
    public class SendRequestAsyncWithoutTimeout : WebRequestSenderTests {
      HttpRequestMessage _dummyRequest;

      [SetUp]
      public override void SetUp() {
        base.SetUp();
        _dummyRequest = A.Dummy<HttpRequestMessage>();
      }

      IReturnValueArgumentValidationConfiguration<Task<HttpResponseMessage>> ACallToSendAsync() {
        return A.CallTo(() => _requestSender.SendAsync(_dummyRequest, TimeSpan.Zero));
      }

      Func<Task<HttpResponseMessage>> DoSendRequestAsync() {
        return async () => await _sut.SendRequestAsync(_dummyRequest);
      }

      [Test]
      public void GivenNullRequest_Throws() {
        Func<Task> act = () => _sut.SendRequestAsync(null);
        act.ShouldThrow<ArgumentNullException>();
      }

      [Test]
      public async Task ReturnsResponseFromRequestSender() {
        var httpResponseMessage = A.Dummy<HttpResponseMessage>();
        ACallToSendAsync().Returns(httpResponseMessage);
        var response = await DoSendRequestAsync().Invoke();
        Assert.That(response, Is.EqualTo(httpResponseMessage));
      }
    }

    [TestFixture]
    public class SendRequestAsyncWithTimeout : WebRequestSenderTests {
      HttpRequestMessage _dummyRequest;
      TimeSpan _requestTimeOut;

      [SetUp]
      public override void SetUp() {
        base.SetUp();
        _dummyRequest = A.Dummy<HttpRequestMessage>();
        _requestTimeOut = TimeSpan.FromSeconds(20);
      }

      IReturnValueArgumentValidationConfiguration<Task<HttpResponseMessage>> ACallToSendAsync() {
        return A.CallTo(() => _requestSender.SendAsync(_dummyRequest, _requestTimeOut));
      }

      Func<Task<HttpResponseMessage>> DoSendRequestAsync() {
        return async () => await _sut.SendRequestAsync(_dummyRequest, _requestTimeOut);
      }

      [Test]
      public void GivenNullRequest_Throws() {
        Func<Task> act = () => _sut.SendRequestAsync(null, _requestTimeOut);
        act.ShouldThrow<ArgumentNullException>();
      }

      [Test]
      public async Task ReturnsResponseFromRequestSender() {
        var httpResponseMessage = A.Dummy<HttpResponseMessage>();
        ACallToSendAsync().Returns(httpResponseMessage);
        var response = await DoSendRequestAsync().Invoke();
        Assert.That(response, Is.EqualTo(httpResponseMessage));
      }
    }
  }
}