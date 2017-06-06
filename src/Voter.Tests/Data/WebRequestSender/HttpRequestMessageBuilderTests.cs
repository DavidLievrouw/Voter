using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace DavidLievrouw.Voter.Data.WebRequestSender {
  [TestFixture]
  public class HttpRequestMessageBuilderTests {
    HttpRequestMessageBuilder CreateSut(HttpMethod httpMethod, Uri uri) {
      return new HttpRequestMessageBuilder(httpMethod, uri);
    }

    [Test]
    public void GivenNullMethod_Throws() {
      Action act = () => CreateSut(null, new Uri("http://ultragenda.com"));
      act.ShouldThrow<ArgumentNullException>();
    }

    [Test]
    public void GivenNullUri_Throws() {
      Action act = () => CreateSut(HttpMethod.Get, null);
      act.ShouldThrow<ArgumentNullException>();
    }

    [Test]
    public void NewRequestShouldAddUrlAndHttpMethod() {
      var request = CreateSut(HttpMethod.Post, new Uri("http://ultragenda.com")).Build();
      request.Method.Should().Be(HttpMethod.Post);
      request.RequestUri.Should().Be(new Uri("http://ultragenda.com"));
    }

    [Test]
    public void BuildShouldCreateANewMessageEveryTime() {
      var builder = CreateSut(HttpMethod.Post, new Uri("http://ultragenda.com"))
        .WithStringContent(new StringContent("abc", Encoding.UTF8, "text/plain"));
      var request1 = builder.Build();
      var request2 = builder.Build();
      Assert.That(request1, Is.Not.EqualTo(request2));
    }

    [Test]
    public async Task WithStringContentShouldSetTheContent() {
      var request = CreateSut(HttpMethod.Post, new Uri("http://ultragenda.com"))
        .WithStringContent(new StringContent("abc", Encoding.UTF8, "text/plain"))
        .Build();
      request.Content.Headers.ContentType.MediaType.Should().Be("text/plain");
      request.Content.Headers.ContentType.CharSet.Should().Be(Encoding.UTF8.WebName);
      var actual = await request.Content.ReadAsStringAsync();
      actual.Should().Be("abc");
    }

    [Test]
    public async Task WithStreamContentShouldSetTheContent() {
      var content = new MemoryStream(new byte[] {1, 2, 3});
      var request = CreateSut(HttpMethod.Post, new Uri("http://ultragenda.com"))
        .WithStreamContent(content)
        .Build();
      var actual = await request.Content.ReadAsByteArrayAsync();
      actual.ShouldBeEquivalentTo(content.ToArray());
    }

    [Test]
    public void WithHeadersShouldCorrectlyChangeHeaders() {
      var request = CreateSut(HttpMethod.Post, new Uri("http://ultragenda.com"))
        .WithHeaders(headers => headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json")))
        .Build();
      request.Headers.Accept.Single().MediaType.Should().Be("application/json");
    }
  }
}