using Nancy;

namespace DavidLievrouw.Voter.Security.Nancy.SessionHijacking {
  public interface ISessionHijackDetector {
    bool IsSessionHijacked(Request request);
  }
}