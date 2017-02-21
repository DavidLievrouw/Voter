using System;
using DavidLievrouw.Voter.Configuration;
using DavidLievrouw.Voter.Domain.DTO;
using DavidLievrouw.Voter.Security.Nancy;
using Nancy.Testing;
using Nancy.ViewEngines;

namespace DavidLievrouw.Voter {
  public class AppBootstrapper : LightweightBootstrapper {
    public AppBootstrapper(Action<ConfigurableBootstrapperConfigurator> configuration, bool enableViewSupportWhichMakesTheUnitTestsReallySlow = false) : base(with => {
      if (enableViewSupportWhichMakesTheUnitTestsReallySlow) {
        // Do not use view support if it is not needed. Makes the unit tests much slower.
        with.ViewLocationProvider<FileSystemViewLocationProvider>();
        with.ViewFactory<TestingViewFactory>();
      }
      configuration(with);
    }) {
      JsonSerializer = new CustomJsonSerializer();
      InternalConfiguration.Serializers.Clear();
      InternalConfiguration.Serializers.Add(JsonSerializer.GetType());

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

    public ICustomJsonSerializer JsonSerializer { get; }
    public User AuthenticatedUser { get; set; }
  }
}