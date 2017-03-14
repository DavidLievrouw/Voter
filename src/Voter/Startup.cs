using DavidLievrouw.Voter;
using Microsoft.Owin;
using Microsoft.Owin.Extensions;
using Nancy;
using Owin;

[assembly: OwinStartup(typeof(Startup))]

namespace DavidLievrouw.Voter {
  public class Startup {
    public void Configuration(IAppBuilder app) {
      app
        .RequireAspNetSession()
        .UseNancy(
          options => {
            options.Bootstrapper = new Bootstrapper();
            options.PerformPassThrough = nancyContext => nancyContext.Response != null && nancyContext.Response.StatusCode == HttpStatusCode.NotFound;
          })
        .UseStageMarker(PipelineStage.PostAcquireState);
    }
  }
}