using System;
using Nancy;

namespace DavidLievrouw.Voter.Security.Nancy {
  public class NancySecurityContextFactory : INancySecurityContextFactory {
    readonly INancySessionFromNancyContextResolver _nancySessionFromNancyContextResolver;
    readonly IVoterIdentityFactory _voterIdentityFactory;

    public NancySecurityContextFactory(INancySessionFromNancyContextResolver nancySessionFromNancyContextResolver, IVoterIdentityFactory voterIdentityFactory) {
      if (nancySessionFromNancyContextResolver == null) throw new ArgumentNullException("nancySessionFromNancyContextResolver");
      if (voterIdentityFactory == null) throw new ArgumentNullException("voterIdentityFactory");
      _nancySessionFromNancyContextResolver = nancySessionFromNancyContextResolver;
      _voterIdentityFactory = voterIdentityFactory;
    }

    public ISecurityContext Create(NancyContext nancyContext) {
      if (nancyContext == null) throw new ArgumentNullException("nancyContext");
      return new NancySecurityContext(nancyContext, _nancySessionFromNancyContextResolver, _voterIdentityFactory);
    }
  }
}