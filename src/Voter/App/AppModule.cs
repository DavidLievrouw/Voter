using Nancy;

namespace DavidLievrouw.Voter.App {
  public class AppModule : NancyModule {
    public AppModule() {
      Get["/"] = parameters => View["App/Login/login"];
    }
  }
}