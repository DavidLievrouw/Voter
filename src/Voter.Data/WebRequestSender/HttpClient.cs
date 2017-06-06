using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace DavidLievrouw.Voter.Data.WebRequestSender {
  public class HttpClient : IHttpClient {
    readonly System.Net.Http.HttpClient _realClient;

    public HttpClient(System.Net.Http.HttpClient realClient) {
      _realClient = realClient ?? throw new ArgumentNullException(nameof(realClient));
    }

    public Task<HttpResponseMessage> SendAsync(HttpRequestMessage request) {
      return _realClient.SendAsync(request);
    }

    public Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken) {
      return _realClient.SendAsync(request, cancellationToken);
    }

    public void Dispose() {
      _realClient.Dispose();
    }
  }
}