using System;
using System.Collections.Generic;
using System.Linq;
using DavidLievrouw.Voter.Domain.DTO;
using Nancy.Security;

namespace DavidLievrouw.Voter.Security.Nancy {
  public class VoterIdentity : IUserIdentity {
    public VoterIdentity(User user) {
      if (user == null) throw new ArgumentNullException("user");
      User = user;
    }

    public User User { get; private set; }

    public string UserName {
      get { return User.Login.Value; }
    }

    public IEnumerable<string> Claims {
      get { return Enumerable.Empty<string>(); }
    }
  }
}