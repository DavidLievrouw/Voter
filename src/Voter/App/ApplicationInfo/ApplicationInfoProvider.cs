using System;
using DavidLievrouw.Voter.Api.Models;
using DavidLievrouw.Voter.Security.Nancy;
using Nancy;

namespace DavidLievrouw.Voter.App.ApplicationInfo {
  public class ApplicationInfoProvider : IApplicationInfoProvider {
    readonly IUrlInfoFromRequestProvider _urlInfoFromRequestProvider;

    public ApplicationInfoProvider(
      IUrlInfoFromRequestProvider urlInfoFromRequestProvider) {
      if (urlInfoFromRequestProvider == null) throw new ArgumentNullException(nameof(urlInfoFromRequestProvider));
      _urlInfoFromRequestProvider = urlInfoFromRequestProvider;
    }

    public ApplicationInfo GetApplicationInfo(NancyContext context) {
      if (context == null) throw new ArgumentNullException(nameof(context));

      var userIdentity = context.CurrentUser as VoterIdentity;
      var currentUser = userIdentity?.User == null
        ? null
        : new User {
          FirstName = userIdentity.User.FirstName,
          LastNamePrefix = userIdentity.User.LastNamePrefix,
          UniqueId = userIdentity.User.UniqueId,
          LastName = userIdentity.User.LastName
        };
      var urlInfo = _urlInfoFromRequestProvider.Provide(context.Request);
      var brokaInfo = new ApplicationInfo {
        UrlInfo = urlInfo,
        CurrentUser = currentUser
      };
      return brokaInfo;
    }
  }
}