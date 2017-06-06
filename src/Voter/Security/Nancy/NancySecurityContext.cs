using System;
using System.Security;
using DavidLievrouw.Voter.Common;
using DavidLievrouw.Voter.Domain.DTO;
using Nancy;

namespace DavidLievrouw.Voter.Security.Nancy {
  public class NancySecurityContext : ISecurityContext {
    readonly NancyContext _nancyContext;
    readonly INancySessionFromNancyContextResolver _nancySessionFromNancyContextResolver;
    readonly IVoterIdentityFactory _voterIdentityFactory;

    public NancySecurityContext(NancyContext nancyContext, INancySessionFromNancyContextResolver nancySessionFromNancyContextResolver, IVoterIdentityFactory voterIdentityFactory) {
      _nancyContext = nancyContext ?? throw new ArgumentNullException(nameof(nancyContext));
      _nancySessionFromNancyContextResolver = nancySessionFromNancyContextResolver ?? throw new ArgumentNullException(nameof(nancySessionFromNancyContextResolver));
      _voterIdentityFactory = voterIdentityFactory ?? throw new ArgumentNullException(nameof(voterIdentityFactory));
    }

    public void SetAuthenticatedUser(User user) {
      var session = _nancySessionFromNancyContextResolver.ResolveSession(_nancyContext);
      if (session == null) throw new SecurityException("There is no current session.");
      session[Constants.SessionKeyForUser] = user;
      _nancyContext.CurrentUser = _voterIdentityFactory.Create(user);
      if (user == null) session.Abandon();
    }

    public User GetAuthenticatedUser() {
      var identity = _nancyContext.CurrentUser as VoterIdentity;
      return identity?.User;
    }
  }
}