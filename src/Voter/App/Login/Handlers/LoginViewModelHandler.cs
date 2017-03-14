using System;
using System.Threading.Tasks;
using DavidLievrouw.Utils;
using DavidLievrouw.Voter.App.ApplicationInfo;
using DavidLievrouw.Voter.App.Login.Models;
using DavidLievrouw.Voter.Configuration;
using Nancy;

namespace DavidLievrouw.Voter.App.Login.Handlers {
  public class LoginViewModelHandler : IHandler<NancyContext, LoginViewModel> {
    readonly IApplicationInfoProvider _applicationInfoProvider;
    readonly ICustomJsonSerializer _jsonSerializer;

    public LoginViewModelHandler(
      IApplicationInfoProvider applicationInfoProvider,
      ICustomJsonSerializer jsonSerializer) {
      if (applicationInfoProvider == null) throw new ArgumentNullException(nameof(applicationInfoProvider));
      if (jsonSerializer == null) throw new ArgumentNullException(nameof(jsonSerializer));
      _applicationInfoProvider = applicationInfoProvider;
      _jsonSerializer = jsonSerializer;
    }

    public Task<LoginViewModel> Handle(NancyContext context) {
      if (context == null) throw new ArgumentNullException(nameof(context));
      return Task.FromResult(new LoginViewModel {
        ApplicationInfo = _jsonSerializer.Serialize(_applicationInfoProvider.GetApplicationInfo(context))
      });
    }
  }
}