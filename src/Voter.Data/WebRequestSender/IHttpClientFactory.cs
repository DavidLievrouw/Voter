namespace DavidLievrouw.Voter.Data.WebRequestSender {
  public interface IHttpClientFactory {
    IHttpClient Create();
  }
}