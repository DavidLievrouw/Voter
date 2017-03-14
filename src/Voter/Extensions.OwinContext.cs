using Microsoft.Owin;

namespace DavidLievrouw.Voter {
  public static partial class Extensions {
    public static T Get<T>(this IOwinContext context) {
      return context.Get<T>(typeof(T).FullName);
    }
  }
}