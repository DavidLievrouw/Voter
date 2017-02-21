using System;
using Nancy;

namespace DavidLievrouw.Voter.Security.Nancy {
  public class NancyIdentityFromContextAssigner : INancyIdentityFromContextAssigner {
    readonly INancySessionFromNancyContextResolver _nancySessionFromNancyContextResolver;
    readonly IUserFromSessionResolver _userFromSessionResolver;
    readonly IVoterIdentityFactory _voterIdentityFactory;

    public NancyIdentityFromContextAssigner(
      INancySessionFromNancyContextResolver nancySessionFromNancyContextResolver,
      IUserFromSessionResolver userFromSessionResolver,
      IVoterIdentityFactory voterIdentityFactory) {
      if (nancySessionFromNancyContextResolver == null) throw new ArgumentNullException(nameof(nancySessionFromNancyContextResolver));
      if (userFromSessionResolver == null) throw new ArgumentNullException(nameof(userFromSessionResolver));
      if (voterIdentityFactory == null) throw new ArgumentNullException(nameof(voterIdentityFactory));
      _nancySessionFromNancyContextResolver = nancySessionFromNancyContextResolver;
      _userFromSessionResolver = userFromSessionResolver;
      _voterIdentityFactory = voterIdentityFactory;
    }

    public void AssignNancyIdentityFromContext(NancyContext nancyContext) {
      if (nancyContext == null) throw new ArgumentNullException(nameof(nancyContext));

      var session = _nancySessionFromNancyContextResolver.ResolveSession(nancyContext);
      var userFromSession = _userFromSessionResolver.ResolveUser(session);
      nancyContext.CurrentUser = _voterIdentityFactory.Create(userFromSession);
    }
  }
}