using System;
using System.Net.Http;
using System.Security;
using System.Threading.Tasks;
using DavidLievrouw.Voter.Common;
using DavidLievrouw.Voter.Data.Records;
using DavidLievrouw.Voter.Data.WebRequestSender;

namespace DavidLievrouw.Voter.Data {
  public class GoogleUserDataService : IGoogleUserDataService {
    readonly IKnownUserDataService _knownUserDataService;
    readonly IKnownUserFromGoogleUserBuilder _knownUserFromGoogleUserBuilder;
    readonly IJsonSerializer _jsonSerializer;
    readonly IWebRequestSender _webRequestSender;

    public GoogleUserDataService(
      IWebRequestSender webRequestSender, 
      IJsonSerializer jsonSerializer,
      IKnownUserFromGoogleUserBuilder knownUserFromGoogleUserBuilder,
      IKnownUserDataService knownUserDataService) {
      _knownUserDataService = knownUserDataService ?? throw new ArgumentNullException(nameof(knownUserDataService));
      _knownUserFromGoogleUserBuilder = knownUserFromGoogleUserBuilder ?? throw new ArgumentNullException(nameof(knownUserFromGoogleUserBuilder));
      _webRequestSender = webRequestSender ?? throw new ArgumentNullException(nameof(webRequestSender));
      _jsonSerializer = jsonSerializer ?? throw new ArgumentNullException(nameof(jsonSerializer));
    }

    // Violation of CQS, but we need the correlation Id from Google
    public async Task<string> ActivateGooglePlusUser(string accessToken) {
      if (accessToken == null) throw new ArgumentNullException(nameof(accessToken));

      try {
        var uri = new Uri("https://www.googleapis.com/oauth2/v1/userinfo?alt=json&access_token=" + accessToken, UriKind.Absolute);
        var request = _webRequestSender.NewRequest(HttpMethod.Get, uri).Build();
        var response = await _webRequestSender.SendRequestAsync(request).ConfigureAwait(false);

        var googleUser = response.Content == null
          ? _jsonSerializer.Deserialize<GoogleUserDataRecord>(null)
          : _jsonSerializer.Deserialize<GoogleUserDataRecord>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
        if (string.IsNullOrEmpty(googleUser?.Id)) throw new InvalidOperationException("A valid Google user could not be determined.");

        // CQS violation here: keep track of Google user in the local storage
        var knownUserDataRecord = _knownUserFromGoogleUserBuilder.BuildKnownUser(googleUser);
        await _knownUserDataService.AddOrUpdateKnownUser(knownUserDataRecord);

        return googleUser.Id;
      } catch (Exception ex) {
        throw new SecurityException("Invalid access token.", ex);
      }
    }
  }
}