using DavidLievrouw.Voter;
using Microsoft.Owin;
using Microsoft.Owin.Extensions;
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
            // When Nancy returns 404 - Not Found, attempt to pass it through to any HttpHandlers that might still be able to handle the request
            options.PerformPassThrough = PassThroughDecider.ConfigureAndPerformPassThroughIfNeeded;
          })
        .UseStageMarker(PipelineStage.PostAcquireState);
    }
  }
}