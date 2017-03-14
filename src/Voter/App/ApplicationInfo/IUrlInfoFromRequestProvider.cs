using Microsoft.Owin;
using Nancy;

namespace DavidLievrouw.Voter.App.ApplicationInfo {
  public interface IUrlInfoFromRequestProvider {
    UrlInfo Provide(Request request);
    UrlInfo Provide(IOwinRequest request);
  }
}
