using System;
using System.Linq;
using System.Security;
using System.Threading.Tasks;
using DavidLievrouw.Utils;
using DavidLievrouw.Voter.Data;
using DavidLievrouw.Voter.Data.Records;
using DavidLievrouw.Voter.Domain.DTO;

namespace DavidLievrouw.Voter.Domain {
  public class UserService : IUserService {
    readonly IAdapter<KnownUserRecord, User> _userAdapter;
    readonly IKnownUserDataService _knownUserDataService;
    readonly IGoogleUserDataService _googleUserDataService;

    public UserService(
      IKnownUserDataService knownUserDataService, 
      IGoogleUserDataService googleUserDataService,
      IAdapter<KnownUserRecord, User> userAdapter) {
      _userAdapter = userAdapter ?? throw new ArgumentNullException(nameof(userAdapter));
      _knownUserDataService = knownUserDataService ?? throw new ArgumentNullException(nameof(knownUserDataService));
      _googleUserDataService = googleUserDataService ?? throw new ArgumentNullException(nameof(googleUserDataService));
    }

    // Violation of CQS, but we are not master of the Google identifiers...
    public async Task<User> ActivateGooglePlusUser(string accessToken) {
      if (accessToken == null) throw new ArgumentNullException(nameof(accessToken));
      var correlationId = await _googleUserDataService.ActivateGooglePlusUser(accessToken);
      var knownUsers = (await _knownUserDataService.FindKnownUserByCorrelationId((char)UserType.GooglePlus, correlationId)).ToList();
      if (knownUsers.Count > 1) throw new SecurityException("Google authentication succeeded, but multiple locally known users were found.");
      if (!knownUsers.Any()) throw new SecurityException("Google authentication succeeded, but no known user could be found.");
      return _userAdapter.Adapt(knownUsers.Single());
    }

    public async Task<User> GetKnownUserById(Guid uniqueId) {
      var knownUser = await _knownUserDataService.GetKnownUserById(uniqueId);
      if (knownUser == null) throw new InvalidOperationException($"Could not find the local user with the specified unique id ({uniqueId}).");
      return _userAdapter.Adapt(knownUser);
    }
  }
}