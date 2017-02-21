using System;
using System.Collections.Generic;

namespace DavidLievrouw.Voter.Common {
  public static class EnumerableExtensions {
    public static void ForEach<T>(this IEnumerable<T> source, Action<T> action) {
      foreach (var element in source) {
        action(element);
      }
    }
  }
}