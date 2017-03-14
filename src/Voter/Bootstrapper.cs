using System.Collections.Generic;
using System.IO;
using Autofac;
using DavidLievrouw.Voter.Common;
using DavidLievrouw.Voter.Composition;
using DavidLievrouw.Voter.Security.Nancy;
using DavidLievrouw.Voter.Security.Nancy.SessionHijacking;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.Bootstrappers.Autofac;
using Nancy.Conventions;
using Nancy.Session.InProc;

namespace DavidLievrouw.Voter {
  public class Bootstrapper : AutofacNancyBootstrapper {
    static readonly Dictionary<string, string[]> StaticContentPaths = new Dictionary<string, string[]> {
      {"static", new[] {"png"}},
      {"app/login", new[] {"png", "js", "css"}}
    };

    byte[] _favIcon;
    readonly IContainer _container;
    readonly IPhysicalRootPathResolver _rootPathResolver;

    public Bootstrapper() {
      _rootPathResolver = new HttpRuntimePhysicalRootPathResolver();
      _container = CompositionRoot.Compose(_rootPathResolver);
    }

    protected override ILifetimeScope GetApplicationContainer() {
      return _container;
    }

    protected override void ApplicationStartup(ILifetimeScope container, IPipelines pipelines) {
      StaticConfiguration.DisableErrorTraces = false;

      // Enable memory sessions, and secure them against session hijacking
      pipelines.EnableInProcSessions();
      /*pipelines.BeforeRequest.AddItemToStartOfPipeline(ctx => {
        var antiSessionHijackLogic = container.Resolve<IAntiSessionHijackLogic>();
        return antiSessionHijackLogic.InterceptHijackedSession(ctx.Request);
      });
      pipelines.AfterRequest.AddItemToEndOfPipeline(ctx => {
        var antiSessionHijackLogic = container.Resolve<IAntiSessionHijackLogic>();
        antiSessionHijackLogic.ProtectResponseFromSessionHijacking(ctx);
      });*/

      // Load the user from the AspNet session. If one is found, create a Nancy identity and assign it.
      pipelines.BeforeRequest.AddItemToEndOfPipeline(ctx => {
        var identityAssigner = container.Resolve<INancyIdentityFromContextAssigner>();
        identityAssigner.AssignNancyIdentityFromContext(ctx);
        return null;
      });

      // Auto return response when one of the known exceptions is thrown
      pipelines.OnError = pipelines.OnError
                          + ErrorPipelines.HandleModelBindingException()
                          + ErrorPipelines.HandleRequestValidationException()
                          + ErrorPipelines.HandleSecurityException();

      base.ApplicationStartup(container, pipelines);
    }

    protected override void ConfigureConventions(NancyConventions conventions) {
      base.ConfigureConventions(conventions);

      // Treat every directory in StaticContent dictionary as static content directory
      StaticContentPaths.ForEach(staticContentPath =>
        conventions.StaticContentsConventions.Add(
          StaticContentConventionBuilder.AddDirectory(requestedPath: staticContentPath.Key, allowedExtensions: staticContentPath.Value)
        ));
    }

    protected override byte[] FavIcon => _favIcon ?? (_favIcon = LoadFavIcon());

    byte[] LoadFavIcon() {
      using (var resourceStream = new FileStream(Path.Combine(_rootPathResolver.ResolvePhysicalRootPath(), "Static", "vote64.png"), FileMode.Open, FileAccess.Read, FileShare.Read)) {
        using (var memoryStream = new MemoryStream()) {
          resourceStream.CopyTo(memoryStream);
          return memoryStream.ToArray();
        }
      }
    }
  }
}