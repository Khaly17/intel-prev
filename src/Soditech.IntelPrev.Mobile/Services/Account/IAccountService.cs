using System.Threading.Tasks;
using Sensor6ty.Results;
using Soditech.IntelPrev.Mobile.Services.Account.Models;

namespace Soditech.IntelPrev.Mobile.Services.Account;

public interface IAccountService
{
    AuthenticateModel AuthenticateModel{ get; set; }
    AuthenticateResultModel? AuthenticateResult { get; set; }
    
    bool IsUserLoggedIn { get; }
    
    Task LoginUserAsync();

    Task LogoutAsync();
    Task<Result> ChangePasswordAsync(string userId, string oldPassword, string newPassword);
    Task<bool> VerifyCurrentPinAsync(string pin);

    Task<Result> ForgotPasswordAsync(string username);
}