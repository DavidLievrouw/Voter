using Nancy;

namespace DavidLievrouw.Voter.Security.Nancy.SessionHijacking {
  public interface ISecureSessionCookieReader {
    SecureSessionCookie Read(Request request, string cookieName);
  }
}