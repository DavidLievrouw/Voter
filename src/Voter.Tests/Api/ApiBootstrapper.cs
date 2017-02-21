using System;
using DavidLievrouw.Voter.Configuration;
using DavidLievrouw.Voter.Domain.DTO;
using DavidLievrouw.Voter.Security.Nancy;
using Nancy.Testing;

namespace DavidLievrouw.Voter.Api {
  public class ApiBootstrapper : LightweightBootstrapper {
    public ApiBootstrapper(Action<ConfigurableBootstrapper.ConfigurableBootstrapperConfigurator> configuration) : base(configuration) {
      InternalConfiguration.Serializers.Clear();
      InternalConfiguration.Serializers.Add(typeof(CustomJsonSerializer));
      BeforeRequest.AddItemToEndOfPipeline(context => {
        if (AuthenticatedUser == null) context.CurrentUser = null;
        else {
          var userIdentity = new VoterIdentity(AuthenticatedUser);
          context.CurrentUser = userIdentity;
        }
        return null;
      });

      OnError = OnError
        + ErrorPipelines.HandleModelBindingException()
        + ErrorPipelines.HandleRequestValidationException()
        + ErrorPipelines.HandleSecurityException();
    }

    public User AuthenticatedUser { get; set; }
  }
}