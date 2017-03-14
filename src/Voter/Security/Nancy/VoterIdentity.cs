using System;
using System.Collections.Generic;
using System.Linq;
using DavidLievrouw.Voter.Domain.DTO;
using Nancy.Security;

namespace DavidLievrouw.Voter.Security.Nancy {
  public class VoterIdentity : IUserIdentity {
    public VoterIdentity(User user) {
      if (user == null) throw new ArgumentNullException(nameof(user));
      User = user;
    }

    public User User { get; private set; }
    public string UserName => User.UniqueId.ToString();
    public IEnumerable<string> Claims => Enumerable.Empty<string>();
  }
}