using System;
using DavidLievrouw.Utils;
using DavidLievrouw.Voter.App.Dashboard.Models;
using DavidLievrouw.Voter.App.Login.Models;
using Nancy;
using Nancy.Security;

namespace DavidLievrouw.Voter.App {
  public class AppModule : NancyModule {
    public AppModule(
      IHandler<NancyContext, LoginViewModel> loginHandler,
      IHandler<NancyContext, DashboardViewModel> dashboardHandler) {
      if (loginHandler == null) throw new ArgumentNullException(nameof(loginHandler));
      if (dashboardHandler == null) throw new ArgumentNullException(nameof(dashboardHandler));

      Get["/", true] = async (parameters, cancellationToken) => View["App/Login/login", await loginHandler.Handle(Context)];

      Get["/dashboard", true] = async (parameters, cancellationToken) => {
        this.RequiresAuthentication();
        return View["App/Dashboard/dashboard", await dashboardHandler.Handle(Context)];
      };
    }
  }
}