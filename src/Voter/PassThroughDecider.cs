using Nancy;

namespace DavidLievrouw.Voter {
  public static class PassThroughDecider {
    public static bool ConfigureAndPerformPassThroughIfNeeded(NancyContext ctx) {
      return ctx?.Response?.StatusCode == HttpStatusCode.NotFound;
    }
  }
}