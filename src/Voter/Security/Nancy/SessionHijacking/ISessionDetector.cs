using Nancy;

namespace DavidLievrouw.Voter.Security.Nancy.SessionHijacking {
  public interface ISessionDetector {
    bool IsInSession(Request request);
  }
}