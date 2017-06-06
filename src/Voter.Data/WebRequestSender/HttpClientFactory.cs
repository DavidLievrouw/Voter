namespace DavidLievrouw.Voter.Data.WebRequestSender {
  public class HttpClientFactory : IHttpClientFactory {
    public IHttpClient Create() {
      return new HttpClient(new System.Net.Http.HttpClient());
    }
  }
}