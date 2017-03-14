using System;
using System.Web;
using System.Web.SessionState;
using Microsoft.Owin.Extensions;
using Owin;

namespace DavidLievrouw.Voter {
  public static partial class Extensions {
    public static IAppBuilder RequireAspNetSession(this IAppBuilder app) {
      if (app == null) throw new ArgumentNullException(nameof(app));
      return app.Use((context, next) => {
        context.Get<HttpContextBase>().SetSessionStateBehavior(SessionStateBehavior.Required);
        return next();
      }).UseStageMarker(PipelineStage.PostAuthorize); // SetSessionStateBehavior must be called before AcquireState
    }
  }
}