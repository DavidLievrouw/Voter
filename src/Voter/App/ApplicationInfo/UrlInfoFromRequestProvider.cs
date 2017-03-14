using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Microsoft.Owin;
using Nancy;

namespace DavidLievrouw.Voter.App.ApplicationInfo {
  public class UrlInfoFromRequestProvider : IUrlInfoFromRequestProvider {
    public UrlInfo Provide(Request request) {
      if (request == null) throw new ArgumentNullException(nameof(request));
      return new UrlInfo {
        BaseUrl = request.Url.BasePath,
        Path = request.Url.Path,
        SiteUrl = request.Url.SiteBase
      };
    }

    public UrlInfo Provide(IOwinRequest request) {
      if (request == null) throw new ArgumentNullException(nameof(request));
      return new UrlInfo {
        BaseUrl = request.PathBase.HasValue
          ? request.PathBase.Value
          : string.Empty,
        Path = request.Path.HasValue
          ? request.Path.Value
          : string.Empty,
        SiteUrl = new StringBuilder()
          .Append(request.Uri.Scheme)
          .Append("://")
          .Append(GetHostName(request.Uri.Host))
          .Append(":")
          .Append(request.Uri.Port)
          .ToString()
      };
    }

    static string GetHostName(string hostName) {
      IPAddress address;

      if (IPAddress.TryParse(hostName, out address)) {
        var addressString = address.ToString();

        return address.AddressFamily == AddressFamily.InterNetworkV6
          ? $"[{addressString}]"
          : addressString;
      }

      return hostName;
    }
  }
}