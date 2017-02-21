﻿using System.IO;
using System.Reflection;
using System.Web;
using System.Web.SessionState;
using DavidLievrouw.Voter.Common;
using DavidLievrouw.Voter.Domain.DTO;

namespace DavidLievrouw.Voter.Security {
  public class HttpContextBuilder {
    HttpContextBase _instance;

    public HttpContextBuilder New() {
      _instance = new HttpContextWrapper(
        new HttpContext(
          new HttpRequest("", "http://tempuri.org", ""),
          new HttpResponse(new StringWriter())));
      return this;
    }

    public HttpContextBuilder WithSession() {
      var sessionContainer = new HttpSessionStateContainer("id",
                                                           new SessionStateItemCollection(),
                                                           new HttpStaticObjectsCollection(),
                                                           10,
                                                           true,
                                                           HttpCookieMode.AutoDetect,
                                                           SessionStateMode.InProc,
                                                           false);

      _instance.Items["AspSession"] = typeof(HttpSessionState).GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance,
                                                                              null,
                                                                              CallingConventions.Standard,
                                                                              new[] {typeof(HttpSessionStateContainer)},
                                                                              null)
                                                              .Invoke(new object[] {sessionContainer});
      return this;
    }

    public HttpContextBuilder WithUser(User user) {
      ((HttpSessionState) _instance.Items["AspSession"])[Constants.SessionKeyForUser] = user;
      return this;
    }

    public HttpContextBase Build() {
      return _instance;
    }
  }
}