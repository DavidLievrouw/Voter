using System;
using DavidLievrouw.Voter.Configuration;
using Nancy.Testing;

namespace DavidLievrouw.Voter {
  public class AppBootstrapper : ConfigurableBootstrapper {
    public AppBootstrapper(Action<ConfigurableBootstrapperConfigurator> configuration) : base(configuration) {
      InternalConfiguration.Serializers.Clear();
      InternalConfiguration.Serializers.Add(typeof(CustomJsonSerializer));
    }
  }
}