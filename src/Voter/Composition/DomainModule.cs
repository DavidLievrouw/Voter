using Autofac;
using DavidLievrouw.Utils;
using DavidLievrouw.Voter.Domain;

namespace DavidLievrouw.Voter.Composition {
  public class DomainModule : NancyModule {
    protected override void Load(ContainerBuilder builder) {
      base.Load(builder);

      var domainAssembly = typeof(UserService).Assembly;
      
      builder.RegisterAssemblyTypes(domainAssembly)
             .AsClosedTypesOf(typeof(IAdapter<,>))
             .SingleInstance();

      builder.RegisterType<UserService>()
             .AsImplementedInterfaces()
             .SingleInstance();
    }
  }
}