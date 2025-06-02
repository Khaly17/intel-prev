using System;
using System.Text.Json;
using System.Threading.Tasks;
using Flurl.Http;
using Microsoft.Extensions.Logging;
using Sensor6ty.Results;
using Soditech.IntelPrev.Mobile.Core.Dependency;

namespace Soditech.IntelPrev.Mobile.Helpers;

public static class StringExtensions
{
    public static string EnsureEndsWith(this string str, char c)
    {
        return str.EndsWith(c) ? str : str + c;
    }

    public static string EnsureEndsWith(this string str, string s)
    {
        return str.EndsWith(s) ? str : str + s;
    }
    
    public static bool IsNullOrEmpty(this string str)
    {
        return string.IsNullOrEmpty(str);
    }
}

public static class FlurlResponseExtensions
{
    public static async Task<TResult<T>> GetResponseAsync<T>(this Task<IFlurlResponse> responseTask)
    {
        var logger = DependencyResolver.GetRequiredService<ILoggerFactory>()
            .CreateLogger(nameof(GetResponseAsync));

        try
        {

            var response = await responseTask.ConfigureAwait(false);
            
            if (response.StatusCode == 200)
            {
                var resultSting = await response.GetStringAsync();
                
                // if resulString contains "IsSuccess", then parse it with TResult<T>
                if (resultSting.Contains("IsSuccess"))
                {
                    var tResult = JsonSerializer.Deserialize<TResult<T>>(resultSting);
                    return tResult ?? Result.Failure<T>(new Error("DeserializationError",$"Cannot deserialize {typeof(T).Name}"));
                }

                var result = JsonSerializer.Deserialize<T>(resultSting);
                if (result != null)
                {
                    return Result.Success(result);
                    
                }
                return Result.Failure<T>(new Error("DeserializationError",$"Cannot deserialize {typeof(T).Name}"));

            }
            
            if (response.StatusCode == 401)
            {
                return Result.Failure<T>(new Error("401", response.ResponseMessage.ToString()));   
            }
           
            
            return Result.Failure<T>(new Error(response.StatusCode.ToString(), "Error occured !!"));
        }
        catch (FlurlHttpException e)
        {
            logger.LogError(e, "Error while getting response {responseType}", typeof(T).Name);
            if (e.StatusCode == 401)
            {
                return Result.Failure<T>(new Error("401", e.Message));   
            }
            
            return Result.Failure<T>(new Error(e.StatusCode.ToString() ?? "500",e.Message));
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error while getting response {responseType}", typeof(T).Name);

            return Result.Failure<T>(new Error("UnKnownError",$"Cannot deserialize {typeof(T).Name}"));
        }
    }
    
    public static async Task<Result> GetResponseAsync(this Task<IFlurlResponse> responseTask)
    {
        var logger = DependencyResolver.GetRequiredService<ILoggerFactory>()
            .CreateLogger(nameof(GetResponseAsync));

        try
        {
            var response = await responseTask.ConfigureAwait(false);
            
            if (response.StatusCode == 200)
            {
                var resultSting = await response.GetStringAsync();
                
                // if resulString contains "IsSuccess", then parse it with TResult<T>
                if (resultSting.Contains("IsSuccess"))
                {
                    var tResult = JsonSerializer.Deserialize<Result>(resultSting);
                    return tResult ?? Result.Failure(new Error("DeserializationError",$"Cannot deserialize {nameof(Result)}"));
                }

                return Result.Success();
            }
            
            if (response.StatusCode == 401)
            {
                return Result.Failure(new Error("401", response.ResponseMessage.ToString()));   
            }
            return Result.Failure(new Error(response.StatusCode.ToString(), "Error occured !!"));
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error while getting response {responseType}", nameof(Result));
            return Result.Failure(new Error("UnKnownError",$"Cannot deserialize {nameof(Result)}"));
        }
    }

}