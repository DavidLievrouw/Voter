using System;
using Autofac;
using DavidLievrouw.Voter.Data;
using DavidLievrouw.Voter.Data.Dapper;
using DavidLievrouw.Voter.Data.WebRequestSender;

namespace DavidLievrouw.Voter.Composition {
  public class DataModule : Module {
    readonly IAppSettingsReader _appSettingsReader;

    public DataModule(IAppSettingsReader appSettingsReader) {
      _appSettingsReader = appSettingsReader ?? throw new ArgumentNullException(nameof(appSettingsReader));
    }

    protected override void Load(ContainerBuilder builder) {
      base.Load(builder);

      var voterDbConnectionString = _appSettingsReader.ReadConnectionString("Voter");
      builder.Register<IDbConnectionFactory>(context => new DbConnectionFactoryByConnectionString(voterDbConnectionString))
             .SingleInstance();
      builder.RegisterType<QueryExecutor>()
             .AsImplementedInterfaces()
             .InstancePerDependency();

      builder.RegisterType<KnownUserDataService>()
             .AsImplementedInterfaces()
             .SingleInstance();
      builder.Register(ctx => new GoogleUserDataService(ctx.Resolve<IWebRequestSender>(), new GoogleJsonSerializer()))
             .AsImplementedInterfaces()
             .SingleInstance();

      builder.RegisterType<WebRequestSender>()
             .AsImplementedInterfaces()
             .SingleInstance();
      builder.RegisterType<RequestSender>()
             .AsImplementedInterfaces()
             .SingleInstance();
      builder.RegisterType<HttpClientFactory>()
             .AsImplementedInterfaces()
             .SingleInstance();
    }
  }
}