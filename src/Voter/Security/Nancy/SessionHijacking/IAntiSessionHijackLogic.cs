using Nancy;

namespace DavidLievrouw.Voter.Security.Nancy.SessionHijacking {
  public interface IAntiSessionHijackLogic {
    Response InterceptHijackedSession(Request request);
    void ProtectResponseFromSessionHijacking(NancyContext context);
  }
}