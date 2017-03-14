using System;
using System.Collections;

namespace DavidLievrouw.Voter {
  public static partial class Extensions {
    public static T Get<T>(this IDictionary dictionary, string key) {
      if (dictionary == null) throw new ArgumentNullException(nameof(dictionary));
      if (dictionary.Contains(key)) return (T)dictionary[key];
      return default(T);
    }
  }
}