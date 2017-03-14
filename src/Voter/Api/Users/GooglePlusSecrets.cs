using Google.Apis.Auth.OAuth2;
using Google.Apis.Plus.v1;

namespace DavidLievrouw.Voter.Api.Users {
  public static class GooglePlusSecrets {
    // These come from the APIs console: https://code.google.com/apis/console
    public static ClientSecrets Secrets = new ClientSecrets {
      ClientId = "34588642309-ogg3uciumd17cfu6l1m6an3hhto90543.apps.googleusercontent.com",
      ClientSecret = "jSRGYMwQqKVVfoOY_uAccNi8"
    };

    public static string[] Scopes = {PlusService.Scope.PlusLogin};
  }
}