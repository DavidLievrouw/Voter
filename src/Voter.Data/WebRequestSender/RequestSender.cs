using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace DavidLievrouw.Voter.Data.WebRequestSender {
  public class RequestSender : IRequestSender {
    readonly IHttpClientFactory _httpClientFactory;

    public RequestSender(IHttpClientFactory httpClientFactory) {
      _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
    }

    public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, TimeSpan timeout) {
      using (var client = _httpClientFactory.Create()) {
        if (timeout <= TimeSpan.Zero) {
          return await client.SendAsync(request).ConfigureAwait(false);
        }
        var cancellationToken = new CancellationTokenSource(timeout).Token;
        return await client.SendAsync(request, cancellationToken).ConfigureAwait(false);
      }
    }
  }
}