using Autofac;
using DavidLievrouw.Voter.Configuration;
using Newtonsoft.Json;

namespace DavidLievrouw.Voter.Composition {
  public class CommonModule : NancyModule {
    protected override void Load(ContainerBuilder builder) {
      base.Load(builder);

      builder.RegisterType<CustomJsonSerializer>()
             .As<JsonSerializer>()
             .AsImplementedInterfaces()
             .SingleInstance();
    }
  }
}