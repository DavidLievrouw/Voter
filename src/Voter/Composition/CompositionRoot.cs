﻿using System.Web.Configuration;
using Autofac;
using DavidLievrouw.Voter.Configuration;
using Newtonsoft.Json;

namespace DavidLievrouw.Voter.Composition {
  public static class CompositionRoot {
    public static IContainer Compose() {
      return Compose(WebConfigurationManager.OpenWebConfiguration("~/"));
    }

    public static IContainer Compose(System.Configuration.Configuration configuration) {
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