using System;

namespace Soditech.IntelPrev.Mobile;

public class AppConsts
{
    public static TimeSpan AccessTokenExpiration = TimeSpan.FromDays(1);
    public static TimeSpan RefreshTokenExpiration = TimeSpan.FromDays(365);
    public static string UserProfileKey = "UserProfile";
    public static string CurrentPinKey = "CurrentPin";
}