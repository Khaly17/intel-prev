using System;
using System.Collections.Generic;
using System.Text;

namespace Soditech.IntelPrev.Web.Services.Extensions;

public static class UrlExtensions
{
    public static string AddQueryParameters(this string url, object obj)
    {
        var queryBuilder = new StringBuilder(url);
        var properties = obj.GetType().GetProperties();

        if (properties.Length <= 0) return queryBuilder.ToString();

        queryBuilder.Append(!url.Contains("?") ? "?" : "&");

        var queryParameters = new List<string>();

        foreach ( var property in properties)
        {
            var value = property.GetValue(obj);

            switch (value)
            {
                case null:
                    continue;
                case DateTime dateTime:
                    queryParameters.Add($"{property.Name}={dateTime:yyyy-MM-dd}");
                    break;
                case DateTimeOffset dateTimeOffset:
                    queryParameters.Add($"{property.Name}={dateTimeOffset:yyyy-MM-dd}");
                    break;
                default:
                    queryParameters.Add($"{property.Name}={value}");
                    break;
            }
        }

        queryBuilder.Append(string.Join("&", queryParameters));

        return queryBuilder.ToString();
    }

    // AddQueryParameters overload for many params
    public static string AddQueryParametersKeyValue(this string url, params (string key, object value)[] parameters)
    {
        var queryBuilder = new StringBuilder(url);

        if (parameters.Length <= 0) return queryBuilder.ToString();

        queryBuilder.Append(!url.Contains('?') ? "?" : "&");

        var queryParameters = new List<string>();

        foreach (var (key, value) in parameters)
        {
            switch (value)
            {
                case null:
                    continue;
                case DateTime dateTime:
                    queryParameters.Add($"{key}={dateTime:yyyy-MM-dd}");
                    break;
                case DateTimeOffset dateTimeOffset:
                    queryParameters.Add($"{key}={dateTimeOffset:yyyy-MM-dd}");
                    break;
                default:
                    queryParameters.Add($"{key}={value}");
                    break;
            }
        }

        queryBuilder.Append(string.Join("&", queryParameters));

        return queryBuilder.ToString();
    }
}