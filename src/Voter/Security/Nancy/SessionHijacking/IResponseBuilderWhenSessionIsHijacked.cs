using Nancy;

namespace DavidLievrouw.Voter.Security.Nancy.SessionHijacking {
  public interface IResponseBuilderWhenSessionIsHijacked {
    Response BuildHijackedResponse();
  }
}