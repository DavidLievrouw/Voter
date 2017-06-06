using System.Threading.Tasks;

namespace DavidLievrouw.Voter.Data {
  public interface IGoogleUserDataService {
    // Violation of CQS, but we need the correlation Id from Google
    Task<string> ActivateGooglePlusUser(string accessToken);
  }
}