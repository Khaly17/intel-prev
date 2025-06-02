using Sensor6ty.Results;

namespace Soditech.IntelPrev.Web.Services.UserInfo;

public interface IUserInfoService
{
    Task<TResult<Models.UserInfo>> GetUserInfoAsync();
}