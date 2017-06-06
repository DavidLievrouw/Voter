using System.Linq;
using Autofac;
using Autofac.Core;
using DavidLievrouw.Utils;
using DavidLievrouw.Voter.Api.Handlers;
using DavidLievrouw.Voter.App;
using DavidLievrouw.Voter.App.ApplicationInfo;
using DavidLievrouw.Voter.Security.Nancy;
using FluentValidation;

namespace DavidLievrouw.Voter.Composition {
  public class NancyModule : Module {
    protected override void Load(ContainerBuilder builder) {
      base.Load(builder);

      var nancyAssembly = typeof(AppModule).Assembly;

      // Register adapters
      builder.RegisterAssemblyTypes(nancyAssembly)
             .AsClosedTypesOf(typeof(IAdapter<,>))
             .SingleInstance();
      
      // Register application general info providers
      builder.RegisterType<ApplicationInfoProvider>()
             .AsImplementedInterfaces()
             .SingleInstance();
      builder.RegisterType<UrlInfoFromRequestProvider>()
             .AsImplementedInterfaces()
             .SingleInstance();

      // Register validators
      builder.RegisterAssemblyTypes(nancyAssembly)
             .AsClosedTypesOf(typeof(IValidator<>))
             .SingleInstance();

      // register all query handlers
      builder.RegisterAssemblyTypes(nancyAssembly)
             .Where(t => t.IsClosedTypeOf(typeof(IHandler<>)))
             .AsImplementedInterfaces();
      builder.RegisterAssemblyTypes(nancyAssembly)
             .Where(t => t.IsClosedTypeOf(typeof(IHandler<,>)))
             .As(t => new KeyedService("queryHandler", t.GetInterfaces().Single(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IHandler<,>))));
      builder.RegisterGenericDecorator(typeof(RequestValidationAwareHandler<,>), typeof(IHandler<,>), "queryHandler");

      // Register other stuff
      builder.RegisterType<NancyIdentityFromContextAssigner>()
             .AsImplementedInterfaces()
             .SingleInstance();
      builder.RegisterType<NancySecurityContextFactory>()
             .AsImplementedInterfaces()
             .SingleInstance();
    }
  }
}