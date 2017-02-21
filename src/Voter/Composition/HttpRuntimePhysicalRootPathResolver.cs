using System.Web;

namespace DavidLievrouw.Voter.Composition {
  public class HttpRuntimePhysicalRootPathResolver : IPhysicalRootPathResolver {
    public string ResolvePhysicalRootPath() {
      return HttpRuntime.AppDomainAppPath;
    }
  }
}