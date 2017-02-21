using System;
using Autofac;
using DavidLievrouw.Voter.Data.Dapper;

namespace DavidLievrouw.Voter.Composition {
  public class DataModule : Module {
    readonly IAppSettingsReader _appSettingsReader;

    public DataModule(IAppSettingsReader appSettingsReader) {
      if (appSettingsReader == null) throw new ArgumentNullException(nameof(appSettingsReader));
      _appSettingsReader = appSettingsReader;
    }

    protected override void Load(ContainerBuilder builder) {
      base.Load(builder);

      var voterDbConnectionString = _appSettingsReader.ReadConnectionString("Voter");

      builder.Register<IDbConnectionFactory>(context => new DbConnectionFactoryByConnectionString(voterDbConnectionString))
             .SingleInstance();
      builder.RegisterType<QueryExecutor>()
             .AsImplementedInterfaces()
             .InstancePerDependency();
    }
  }
}