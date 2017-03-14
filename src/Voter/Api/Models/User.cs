using System;

namespace DavidLievrouw.Voter.Api.Models {
  public class User {
    public Guid UniqueId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string LastNamePrefix { get; set; }
  }
}