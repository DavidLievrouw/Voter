using System;
using DavidLievrouw.Utils;
using DavidLievrouw.Voter.App.Models;
using Nancy;

namespace DavidLievrouw.Voter.App {
  public class AppModule : NancyModule {
    public AppModule(IHandler<NancyContext, LoginViewModel> loginHandler) {
      if (loginHandler == null) throw new ArgumentNullException(nameof(loginHandler));
      Get["/", true] = async (parameters, cancellationToken) => View["App/Login/login", await loginHandler.Handle(Context)];
    }
  }
}