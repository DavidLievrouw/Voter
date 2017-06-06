using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace DavidLievrouw.Voter.Data.WebRequestSender {
  public interface IWebRequestSender {
    Task<HttpResponseMessage> SendRequestAsync(HttpRequestMessage request);
    Task<HttpResponseMessage> SendRequestAsync(HttpRequestMessage request, TimeSpan timeout);
    IHttpRequestMessageBuilder NewRequest(HttpMethod httpMethod, Uri uri);
  }
}