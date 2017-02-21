using Nancy;

namespace DavidLievrouw.Voter.Security.Nancy {
  public interface INancySecurityContextFactory {
    ISecurityContext Create(NancyContext nancyContext);
  }
}