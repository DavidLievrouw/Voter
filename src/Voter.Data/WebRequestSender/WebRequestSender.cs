using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace DavidLievrouw.Voter.Data.WebRequestSender {
  public class WebRequestSender : IWebRequestSender {
    readonly IRequestSender _requestSender;

    public WebRequestSender(IRequestSender requestSender) {
      _requestSender = requestSender ?? throw new ArgumentNullException(nameof(requestSender));
    }

    public async Task<HttpResponseMessage> SendRequestAsync(HttpRequestMessage request) {
      if (request == null) throw new ArgumentNullException(nameof(request));
      return await _requestSender.SendAsync(request, TimeSpan.Zero).ConfigureAwait(false);
    }

    public async Task<HttpResponseMessage> SendRequestAsync(HttpRequestMessage request, TimeSpan timeout) {
      if (request == null) throw new ArgumentNullException(nameof(request));
      return await _requestSender.SendAsync(request, timeout).ConfigureAwait(false);
    }

    public IHttpRequestMessageBuilder NewRequest(HttpMethod httpMethod, Uri uri) {
      if (httpMethod == null) throw new ArgumentNullException(nameof(httpMethod));
      if (uri == null) throw new ArgumentNullException(nameof(uri));
      return new HttpRequestMessageBuilder(httpMethod, uri);
    }
  }
}