using Nancy;

namespace DavidLievrouw.Voter.Security.Nancy.SessionHijacking {
  public interface ISessionAntiHijackHashStripper {
    void StripHashFromCookie(Request request);
  }
}