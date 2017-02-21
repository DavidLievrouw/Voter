using System;
using Nancy;

namespace DavidLievrouw.Voter.Security.Nancy {
  public class NancySecurityContextFactory : INancySecurityContextFactory {
    readonly INancySessionFromNancyContextResolver _nancySessionFromNancyContextResolver;
    readonly IVoterIdentityFactory _voterIdentityFactory;

    public NancySecurityContextFactory(INancySessionFromNancyContextResolver nancySessionFromNancyContextResolver, IVoterIdentityFactory voterIdentityFactory) {
      if (nancySessionFromNancyContextResolver == null) throw new ArgumentNullException(nameof(nancySessionFromNancyContextResolver));
      if (voterIdentityFactory == null) throw new ArgumentNullException(nameof(voterIdentityFactory));
      _nancySessionFromNancyContextResolver = nancySessionFromNancyContextResolver;
      _voterIdentityFactory = voterIdentityFactory;
    }

    public ISecurityContext Create(NancyContext nancyContext) {
      if (nancyContext == null) throw new ArgumentNullException(nameof(nancyContext));
      return new NancySecurityContext(nancyContext, _nancySessionFromNancyContextResolver, _voterIdentityFactory);
    }
  }
}