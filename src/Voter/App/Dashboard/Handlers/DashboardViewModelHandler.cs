using System;
using System.Threading.Tasks;
using DavidLievrouw.Utils;
using DavidLievrouw.Voter.App.ApplicationInfo;
using DavidLievrouw.Voter.App.Dashboard.Models;
using DavidLievrouw.Voter.Configuration;
using Nancy;

namespace DavidLievrouw.Voter.App.Dashboard.Handlers {
  public class DashboardViewModelHandler : IHandler<NancyContext, DashboardViewModel> {
    readonly IApplicationInfoProvider _applicationInfoProvider;
    readonly ICustomJsonSerializer _jsonSerializer;

    public DashboardViewModelHandler(
      IApplicationInfoProvider applicationInfoProvider,
      ICustomJsonSerializer jsonSerializer) {
      if (applicationInfoProvider == null) throw new ArgumentNullException(nameof(applicationInfoProvider));
      if (jsonSerializer == null) throw new ArgumentNullException(nameof(jsonSerializer));
      _applicationInfoProvider = applicationInfoProvider;
      _jsonSerializer = jsonSerializer;
    }

    public Task<DashboardViewModel> Handle(NancyContext context) {
      if (context == null) throw new ArgumentNullException(nameof(context));
      return Task.FromResult(new DashboardViewModel {
        ApplicationInfo = _jsonSerializer.Serialize(_applicationInfoProvider.GetApplicationInfo(context))
      });
    }
  }
}