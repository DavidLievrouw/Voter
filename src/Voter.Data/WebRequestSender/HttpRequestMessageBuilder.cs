using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;

namespace DavidLievrouw.Voter.Data.WebRequestSender {
  public class HttpRequestMessageBuilder : IHttpRequestMessageBuilder {
    readonly HttpRequestMessageBuilder _innerHttpRequestMessageBuilder;
    readonly Func<HttpRequestMessage, HttpRequestMessage> _buildStep;

    internal HttpRequestMessageBuilder(HttpMethod httpMethod, Uri uri) {
      if (httpMethod == null) throw new ArgumentNullException(nameof(httpMethod));
      if (uri == null) throw new ArgumentNullException(nameof(uri));
      _buildStep = r => {
        r.Method = httpMethod;
        r.RequestUri = uri;
        return r;
      };
      _innerHttpRequestMessageBuilder = null;
    }

    HttpRequestMessageBuilder(HttpRequestMessageBuilder innerHttpRequestMessageBuilder, Func<HttpRequestMessage, HttpRequestMessage> buildStep) {
      _innerHttpRequestMessageBuilder = innerHttpRequestMessageBuilder ?? throw new ArgumentNullException(nameof(innerHttpRequestMessageBuilder));
      _buildStep = buildStep ?? throw new ArgumentNullException(nameof(buildStep));
    }

    public IHttpRequestMessageBuilder WithStreamContent(Stream stream) {
      return new HttpRequestMessageBuilder(this,
        r => {
          r.Content = new StreamContent(stream);
          return r;
        });
    }

    public IHttpRequestMessageBuilder WithStringContent(StringContent stringContent) {
      return new HttpRequestMessageBuilder(this,
        r => {
          r.Content = stringContent;
          return r;
        });
    }

    public IHttpRequestMessageBuilder WithHeaders(Action<HttpRequestHeaders> headerChanger) {
      return new HttpRequestMessageBuilder(this,
        r => {
          headerChanger(r.Headers);
          return r;
        });
    }

    public HttpRequestMessage Build() {
      return ApplyBuildSteps(new HttpRequestMessage());
    }

    HttpRequestMessage ApplyBuildSteps(HttpRequestMessage message) {
      if (_innerHttpRequestMessageBuilder != null) message = _innerHttpRequestMessageBuilder.ApplyBuildSteps(message);
      return _buildStep(message);
    }
  }
}