using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace DavidLievrouw.Voter.Data.WebRequestSender {
  public interface IHttpClient : IDisposable {
    Task<HttpResponseMessage> SendAsync(HttpRequestMessage request);
    Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken);
  }
}