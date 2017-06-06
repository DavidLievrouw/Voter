using System;
using System.Net.Http;
using System.Security;
using System.Threading.Tasks;
using System.Web;
using DavidLievrouw.Utils.ForTesting.FakeItEasy;
using DavidLievrouw.Utils.ForTesting.FluentAssertions;
using DavidLievrouw.Voter.Common;
using DavidLievrouw.Voter.Data.Records;
using DavidLievrouw.Voter.Data.WebRequestSender;
using FakeItEasy;
using FluentAssertions;
using Newtonsoft.Json;
using NUnit.Framework;

namespace DavidLievrouw.Voter.Data {
  [TestFixture]
  public class GoogleUserDataServiceTests {
    IJsonSerializer _jsonSerializer;
    IWebRequestSender _webRequestSender;
    GoogleUserDataService _sut;

    [SetUp]
    public virtual void SetUp() {
      _jsonSerializer = _jsonSerializer.Fake();
      _webRequestSender = _webRequestSender.Fake();
      _sut = new GoogleUserDataService(_webRequestSender, _jsonSerializer);
    }

    [TestFixture]
    public class Construction : GoogleUserDataServiceTests {
      [Test]
      public void HasExactlyOneConstructor_WithNoOptionalParameters() {
        _sut.Should().HaveExactlyOneConstructorWithoutOptionalParameters();
      }
    }

    [TestFixture]
    public class ActivateGooglePlusUser : GoogleUserDataServiceTests {
      string _accessToken;

      [SetUp]
      public override void SetUp() {
        base.SetUp();
        _accessToken = "ABC123";
      }

      [Test]
      public void GivenNullAccessToken_Throws() {
        Func<Task> act = () => _sut.ActivateGooglePlusUser(null);
        act.ShouldThrow<ArgumentNullException>();
      }

      [Test]
      public async Task SendsToTheCorrectGoogleApiUrl() {
        var googleUser = new GoogleUserDataRecord { Id = "CorrelationId123" };
        A.CallTo(() => _jsonSerializer.Deserialize<GoogleUserDataRecord>(A<string>._)).Returns(googleUser);
        var expectedUri = new Uri("https://www.googleapis.com/oauth2/v1/userinfo?alt=json&access_token=" + _accessToken, UriKind.Absolute);
        await _sut.ActivateGooglePlusUser(_accessToken);
        A.CallTo(() => _webRequestSender.NewRequest(HttpMethod.Get, expectedUri)).MustHaveHappened();
      }

      [Test]
      public void WhenGoogleRequestFails_ThrowsSecurityException() {
        var requestBuilder = A.Fake<IHttpRequestMessageBuilder>();
        A.CallTo(() => _webRequestSender.NewRequest(A<HttpMethod>._, A<Uri>._)).Returns(requestBuilder);
        var httpRequestMessage = new HttpRequestMessage();
        A.CallTo(() => requestBuilder.Build()).Returns(httpRequestMessage);
        A.CallTo(() => _webRequestSender.SendRequestAsync(httpRequestMessage)).Throws(new HttpException());

        Func<Task> act = () => _sut.ActivateGooglePlusUser(_accessToken);
        act.ShouldThrow<SecurityException>();
      }

      [Test]
      public void WhenGoogleResponseCannotBeSerialized_ThrowsSecurityException() {
        var requestBuilder = A.Fake<IHttpRequestMessageBuilder>();
        A.CallTo(() => _webRequestSender.NewRequest(A<HttpMethod>._, A<Uri>._)).Returns(requestBuilder);
        var httpRequestMessage = new HttpRequestMessage();
        A.CallTo(() => requestBuilder.Build()).Returns(httpRequestMessage);
        var googleUserJson = "{I am the user}";
        var httpResponseMessage = new HttpResponseMessage {
          Content = new StringContent(googleUserJson)
        };
        A.CallTo(() => _webRequestSender.SendRequestAsync(httpRequestMessage)).Returns(httpResponseMessage);
        A.CallTo(() => _jsonSerializer.Deserialize<GoogleUserDataRecord>(googleUserJson)).Throws(new JsonSerializationException());

        Func<Task> act = () => _sut.ActivateGooglePlusUser(_accessToken);
        act.ShouldThrow<SecurityException>();
      }

      [Test]
      public void WhenGoogleResponseDoesNotContainAValidUser_ThrowsSecurityException() {
        var requestBuilder = A.Fake<IHttpRequestMessageBuilder>();
        A.CallTo(() => _webRequestSender.NewRequest(A<HttpMethod>._, A<Uri>._)).Returns(requestBuilder);
        var httpRequestMessage = new HttpRequestMessage();
        A.CallTo(() => requestBuilder.Build()).Returns(httpRequestMessage);
        var googleUserJson = "{I am the user}";
        var httpResponseMessage = new HttpResponseMessage {
          Content = new StringContent(googleUserJson)
        };
        A.CallTo(() => _webRequestSender.SendRequestAsync(httpRequestMessage)).Returns(httpResponseMessage);
        A.CallTo(() => _jsonSerializer.Deserialize<GoogleUserDataRecord>(googleUserJson)).Returns(null);

        Func<Task> act = () => _sut.ActivateGooglePlusUser(_accessToken);
        act.ShouldThrow<SecurityException>();
      }

      [Test]
      public async Task UponSuccess_ReturnsCorrelationIdForGoogleUser() {
        var requestBuilder = A.Fake<IHttpRequestMessageBuilder>();
        A.CallTo(() => _webRequestSender.NewRequest(A<HttpMethod>._, A<Uri>._)).Returns(requestBuilder);
        var httpRequestMessage = new HttpRequestMessage();
        A.CallTo(() => requestBuilder.Build()).Returns(httpRequestMessage);
        var googleUserJson = "{I am the user}";
        var httpResponseMessage = new HttpResponseMessage {
          Content = new StringContent(googleUserJson)
        };
        A.CallTo(() => _webRequestSender.SendRequestAsync(httpRequestMessage)).Returns(httpResponseMessage);
        var googleUser = new GoogleUserDataRecord {Id = "CorrelationId123"};
        A.CallTo(() => _jsonSerializer.Deserialize<GoogleUserDataRecord>(googleUserJson)).Returns(googleUser);

        var actual = await _sut.ActivateGooglePlusUser(_accessToken);

        actual.Should().Be(googleUser.Id);
      }
    }
  }
}