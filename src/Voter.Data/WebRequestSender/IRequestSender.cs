using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace DavidLievrouw.Voter.Data.WebRequestSender {
  public interface IRequestSender {
    Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, TimeSpan timeout);
  }
}