using System;
using DavidLievrouw.Utils.ForTesting.FluentAssertions;
using FakeItEasy;
using FluentAssertions;
using Microsoft.Owin;
using Nancy;
using NUnit.Framework;

namespace DavidLievrouw.Voter.App.ApplicationInfo {
  [TestFixture]
  public class UrlInfoFromRequestProviderTests {
    UrlInfoFromRequestProvider _sut;

    [SetUp]
    public virtual void SetUp() {
      _sut = new UrlInfoFromRequestProvider();
    }

    [TestFixture]
    public class Construction : UrlInfoFromRequestProviderTests {
      [Test]
      public void HasExactlyOneConstructor_WithNoOptionalParameters() {
        _sut.Should().HaveExactlyOneConstructorWithoutOptionalParameters();
      }
    }

    [TestFixture]
    public class FromNancyRequest : UrlInfoFromRequestProviderTests {
      Request _nancyRequest;
      Uri _requestUri;

      [SetUp]
      public override void SetUp() {
        base.SetUp();
        _requestUri = new Uri(new Uri("http://www.voter.com/api/", UriKind.Absolute), new Uri("patient/default.aspx", UriKind.Relative));
        var url = new Url(_requestUri.ToString()) {
          BasePath = "/api",
          Path = "/patient/default.aspx"
        };
        _nancyRequest = new Request("GET", url);
      }

      [Test]
      public void GivenNullRequest_Throws() {
        Assert.Throws<ArgumentNullException>(() => _sut.Provide((Request) null));
      }

      [Test]
      public void SetsCorrectBaseUrl() {
        _sut.Provide(_nancyRequest).BaseUrl.ShouldBeEquivalentTo("/api");
      }

      [Test]
      public void SetsCorrectPath() {
        _sut.Provide(_nancyRequest).Path.ShouldBeEquivalentTo("/patient/default.aspx");
      }

      [Test]
      public void SetsCorrectSiteUrl() {
        _sut.Provide(_nancyRequest).SiteUrl.ShouldBeEquivalentTo("http://www.voter.com:80");
      }
    }

    [TestFixture]
    public class FromOwinRequest : UrlInfoFromRequestProviderTests {
      IOwinRequest _owinRequest;

      [SetUp]
      public override void SetUp() {
        base.SetUp();
        _owinRequest = A.Fake<IOwinRequest>();
        A.CallTo(() => _owinRequest.PathBase).Returns(new PathString("/api"));
        A.CallTo(() => _owinRequest.Path).Returns(new PathString("/patient/default.aspx"));
        A.CallTo(() => _owinRequest.Uri).Returns(new Uri("http://www.voter.com:80"));
      }

      [Test]
      public void GivenNullRequest_Throws() {
        Assert.Throws<ArgumentNullException>(() => _sut.Provide((IOwinRequest) null));
      }

      [Test]
      public void SetsCorrectBaseUrl() {
        _sut.Provide(_owinRequest).BaseUrl.ShouldBeEquivalentTo("/api");
      }

      [Test]
      public void WhenNoBaseUrlIsGiven_SetsBaseUrlToEmpty() {
        A.CallTo(() => _owinRequest.PathBase).Returns(new PathString());
        _sut.Provide(_owinRequest).BaseUrl.ShouldBeEquivalentTo(string.Empty);
      }

      [Test]
      public void SetsCorrectPath() {
        _sut.Provide(_owinRequest).Path.ShouldBeEquivalentTo("/patient/default.aspx");
      }

      [Test]
      public void WhenNoPathIsGiven_SetsPathToEmpty() {
        A.CallTo(() => _owinRequest.Path).Returns(new PathString());
        _sut.Provide(_owinRequest).Path.ShouldBeEquivalentTo(string.Empty);
      }

      [Test]
      public void GivenHostName_SetsCorrectSiteUrl() {
        A.CallTo(() => _owinRequest.Uri).Returns(new Uri("http://www.voter.com:80"));
        _sut.Provide(_owinRequest).SiteUrl.ShouldBeEquivalentTo("http://www.voter.com:80");
      }

      [Test]
      public void GivenHostIpv4Address_SetsCorrectSiteUrl() {
        A.CallTo(() => _owinRequest.Uri).Returns(new Uri("http://127.0.0.1:80"));
        _sut.Provide(_owinRequest).SiteUrl.ShouldBeEquivalentTo("http://127.0.0.1:80");
      }

      [Test]
      public void GivenHostIpv6Address_SetsCorrectSiteUrl() {
        A.CallTo(() => _owinRequest.Uri).Returns(new Uri("http://[fe80::d5f4:c98b:fe9a:faa4%5]:80"));
        _sut.Provide(_owinRequest).SiteUrl.ShouldBeEquivalentTo("http://[fe80::d5f4:c98b:fe9a:faa4]:80");
      }

      [Test]
      public void GivenNoPortSpecified_SetsCorrectSiteUrl() {
        A.CallTo(() => _owinRequest.Uri).Returns(new Uri("http://www.voter.com"));
        _sut.Provide(_owinRequest).SiteUrl.ShouldBeEquivalentTo("http://www.voter.com:80");
      }

      [Test]
      public void GivenPortSpecified_SetsCorrectSiteUrl() {
        A.CallTo(() => _owinRequest.Uri).Returns(new Uri("http://www.voter.com:8080"));
        _sut.Provide(_owinRequest).SiteUrl.ShouldBeEquivalentTo("http://www.voter.com:8080");
      }
    }
  }
}