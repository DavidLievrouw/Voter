using Nancy;

namespace DavidLievrouw.Voter.App.ApplicationInfo {
  public interface IApplicationInfoProvider {
    ApplicationInfo GetApplicationInfo(NancyContext context);
  }
}