using System;
using System.Web.Configuration;
using Autofac;
using DavidLievrouw.Voter.Configuration;
using Newtonsoft.Json;

namespace DavidLievrouw.Voter.Composition {
  public static class CompositionRoot {
    public static IContainer Compose(IPhysicalRootPathResolver physicalRootPathResolver) {
      return Compose(WebConfigurationManager.OpenWebConfiguration("~/"), physicalRootPathResolver);
    }

    public static IContainer Compose(System.Configuration.Configuration configuration, IPhysicalRootPathResolver physicalRootPathResolver) {
      if (configuration == null) throw new ArgumentNullException(nameof(configuration));
      if (physicalRootPathResolver == null) throw new ArgumentNullException(nameof(physicalRootPathResolver));

      var builder = new ContainerBuilder();
      var appSettingsReader = new AppSettingsReader(configuration);

      builder.RegisterType<CustomJsonSerializer>()
             .As<JsonSerializer>()
             .AsImplementedInterfaces()
             .SingleInstance();

      builder.RegisterModule<SecurityModule>();
      builder.RegisterModule<NancyModule>();
      builder.RegisterModule(new DataModule(appSettingsReader));

      return builder.Build();
    }
  }
}