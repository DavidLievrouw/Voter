using Nancy;

namespace DavidLievrouw.Voter.Security.Nancy.SessionHijacking {
  public interface ISessionAntiHijackHashGenerator {
    string GenerateHash(Request request);
  }
}