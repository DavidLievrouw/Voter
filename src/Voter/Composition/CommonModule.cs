using Autofac;
using DavidLievrouw.Utils;
using DavidLievrouw.Voter.Configuration;
using Newtonsoft.Json;

namespace DavidLievrouw.Voter.Composition {
  public class CommonModule : NancyModule {
    protected override void Load(ContainerBuilder builder) {
      base.Load(builder);

      var webApiAssembly = typeof(CompositionRoot).Assembly;
      builder.RegisterAssemblyTypes(webApiAssembly)
             .AsClosedTypesOf(typeof(IAdapter<,>))
             .SingleInstance();

      builder.RegisterType<CustomJsonSerializer>()
             .As<JsonSerializer>()
             .AsImplementedInterfaces()
             .SingleInstance();
    }
  }
}