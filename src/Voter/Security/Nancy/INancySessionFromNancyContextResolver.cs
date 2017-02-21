using Nancy;

namespace DavidLievrouw.Voter.Security.Nancy {
  public interface INancySessionFromNancyContextResolver {
    ISession ResolveSession(NancyContext nancyContext);
  }
}