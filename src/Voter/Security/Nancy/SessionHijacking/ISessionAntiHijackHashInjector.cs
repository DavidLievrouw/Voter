using Nancy;

namespace DavidLievrouw.Voter.Security.Nancy.SessionHijacking {
  public interface ISessionAntiHijackHashInjector {
    void InjectHashInCookie(NancyContext context);
  }
}