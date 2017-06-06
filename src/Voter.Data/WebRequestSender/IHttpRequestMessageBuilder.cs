using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;

namespace DavidLievrouw.Voter.Data.WebRequestSender {
  public interface IHttpRequestMessageBuilder {
    IHttpRequestMessageBuilder WithStreamContent(Stream stream);
    IHttpRequestMessageBuilder WithStringContent(StringContent stringContent);
    IHttpRequestMessageBuilder WithHeaders(Action<HttpRequestHeaders> headerChanger);
    HttpRequestMessage Build();
  }
}