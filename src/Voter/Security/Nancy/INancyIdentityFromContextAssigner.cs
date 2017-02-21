using Nancy;

namespace DavidLievrouw.Voter.Security.Nancy {
  public interface INancyIdentityFromContextAssigner {
    void AssignNancyIdentityFromContext(NancyContext nancyContext);
  }
}