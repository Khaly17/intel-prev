using System.Text.RegularExpressions;

namespace Soditech.IntelPrev.Mobile.Helpers;

public static class ValidationHelper
{
    private const string emailRegex = @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";

    public static bool IsEmail(string value)
    {
        if (value.IsNullOrEmpty())
        {
            return false;
        }

        var regex = new Regex(emailRegex);
        return regex.IsMatch(value);
    }
}