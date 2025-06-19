using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Controls.UserDialogs.Maui;
using Flurl.Http;
using Microsoft.Maui.Networking;
using Soditech.IntelPrev.Mobile.Localization;

namespace Soditech.IntelPrev.Mobile.Core.Threading;

/// <summary>
/// Provides methods to execute web requests with success, failure, and finally callbacks.
/// </summary>
public static class WebRequestExecutor
{
    /// <summary>
    /// Executes a web request with specified success, failure, and finally callbacks.
    /// </summary>
    /// <typeparam name="TResult">The type of the result returned by the web request.</typeparam>
    /// <param name="func">The function representing the web request to be executed.</param>
    /// <param name="successCallback">The callback to be executed if the web request is successful.</param>
    /// <param name="failCallback">The callback to be executed if the web request fails.</param>
    /// <param name="finallyCallback">The callback to be executed after the web request completes, regardless of success or failure.</param>
    public static async Task ExecuteAsync<TResult>(
        Func<Task<TResult>> func,
        Func<TResult, Task> successCallback,
        Func<Exception, Task> failCallback = null,
        Action finallyCallback = null)
    {
            if (successCallback == null)
            {
                successCallback = _ => Task.CompletedTask;
            }

            if (failCallback == null)
            {
                failCallback = _ => Task.CompletedTask;
            }

            try
            {
                
                if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
                {

                    var accepted = await UserDialogs.Instance.ConfirmAsync(L.Localize("DoYouWantToEnableInternetAndTryAgain"), L.Localize("NoInternet"));

                    if (accepted)
                    {
                        await ExecuteAsync(func, successCallback, failCallback);
                    }
                    else
                    {
                        await failCallback(new Exception(L.Localize("NoInternet")));
                    }
                }
                else
                {
                    await successCallback(await func());
                }
            }
            catch (Exception exception)
            {
                await HandleExceptionAsync(exception, func, successCallback, failCallback);
            }
            finally
            {
                finallyCallback?.Invoke();
            }
    }

        public static async Task ExecuteAsync(
            Func<Task> func,
            Func<Task> successCallback = null,
            Func<Exception, Task> failCallback = null,
            Action finallyCallback = null)
        {
            if (successCallback == null)
            {
                successCallback = () => Task.CompletedTask;
            }

            if (failCallback == null)
            {
                failCallback = _ => Task.CompletedTask;
            }

            try
            {
                if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
                {
                    var accepted = await UserDialogs.Instance.ConfirmAsync(L.Localize("DoYouWantToTryAgain"), L.Localize("NoInternet"));

                    if (accepted)
                    {
                        await ExecuteAsync(func, successCallback, failCallback);
                    }
                    else
                    {
                        await failCallback(new Exception(L.Localize("NoInternet")));
                    }
                }
                else
                {
                    await func();
                    await successCallback();
                }
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(ex, func, successCallback, failCallback);
            }
            finally
            {
                finallyCallback?.Invoke();
            }
        }

        private static async Task HandleExceptionAsync<TResult>(Exception exception,
            Func<Task<TResult>> func,
            Func<TResult, Task> successCallback,
            Func<Exception, Task> failCallback)
        {
            switch (exception)
            {
                case FlurlHttpTimeoutException httpTimeoutException:
                    await HandleFlurlHttpTimeoutExceptionAsync(httpTimeoutException, func, successCallback, failCallback);
                    break;
                case FlurlHttpException httpException:
                    await HandleFlurlHttpExceptionAsync(httpException, func, successCallback, failCallback);
                    break;
                default:
                    await HandleDefaultExceptionAsync(exception, func, successCallback, failCallback);
                    break;
            }
        }

        private static async Task HandleExceptionAsync(Exception exception,
            Func<Task> func,
            Func<Task> successCallback,
            Func<Exception, Task> failCallback)
        {

            switch (exception)
            {
                case FlurlHttpTimeoutException httpTimeoutException:
                    await HandleFlurlHttpTimeoutExceptionAsync(httpTimeoutException, func, successCallback, failCallback);
                    break;
                case FlurlHttpException httpException:
                    await HandleFlurlHttpExceptionAsync(httpException, func, successCallback, failCallback);
                    break;
                default:
                    await HandleDefaultExceptionAsync(exception, func, successCallback, failCallback);
                    break;
            }
        }
        
        private static async Task HandleFlurlHttpTimeoutExceptionAsync<TResult>(
            FlurlHttpTimeoutException httpTimeoutException,
            Func<Task<TResult>> func,
            Func<TResult, Task> successCallback,
            Func<Exception, Task> failCallback)
        {
            var accepted = await UserDialogs.Instance.ConfirmAsync(L.Localize("DoYouWantToTryAgain"), L.Localize("RequestIsTimedOut"));

            if (accepted)
            {
                await ExecuteAsync(func, successCallback, failCallback);
            }
            else
            {
                await failCallback(httpTimeoutException);
            }
        }

        private static async Task HandleFlurlHttpTimeoutExceptionAsync(FlurlHttpTimeoutException httpTimeoutException,
            Func<Task> func,
            Func<Task> successCallback,
            Func<Exception, Task> failCallback)
        {
            var accepted = await UserDialogs.Instance.ConfirmAsync(L.Localize("DoYouWantToTryAgain"), L.Localize("RequestIsTimedOut"));

            if (accepted)
            {
                await ExecuteAsync(func, successCallback, failCallback);
            }
            else
            {
                await failCallback(httpTimeoutException);
            }
        }

        private static async Task HandleFlurlHttpExceptionAsync(FlurlHttpException httpException,
            Func<Task> func,
            Func<Task> successCallback,
            Func<Exception, Task> failCallback)
        {
            var httpExceptionMessage = "";
            if (Debugger.IsAttached)
            {
                httpExceptionMessage += Environment.NewLine + httpException.Message;
            }

            var accepted = await UserDialogs.Instance.ConfirmAsync(httpExceptionMessage + " " + L.Localize("DoYouWantToTryAgain"), L.Localize("HTTPException"));

            if (accepted)
            {
                await ExecuteAsync(func, successCallback, failCallback);
            }
            else
            {
                await failCallback(httpException);
            }
        }

        private static async Task HandleFlurlHttpExceptionAsync<TResult>(FlurlHttpException httpException,
            Func<Task<TResult>> func,
            Func<TResult, Task> successCallback,
            Func<Exception, Task> failCallback)
        {
            var httpExceptionMessage = "";
            if (Debugger.IsAttached)
            {
                httpExceptionMessage += Environment.NewLine + httpException.Message;
            }

            var accepted = await UserDialogs.Instance.ConfirmAsync(httpExceptionMessage + " " + L.Localize("DoYouWantToTryAgain"), L.Localize("HTTPException"));

            if (accepted)
            {
                await ExecuteAsync(func, successCallback, failCallback);
            }
            else
            {
                await failCallback(httpException);
            }
        }
        
        private static async Task HandleDefaultExceptionAsync(Exception exception,
            Func<Task> func,
            Func<Task> successCallback,
            Func<Exception, Task> failCallback)
        {

        var accepted = await UserDialogs.Instance.ConfirmAsync(L.Localize("DoYouWantToTryAgain"), L.Localize("UnhandledWebRequestException"));

        if (accepted)
        {
            await ExecuteAsync(func, successCallback, failCallback);
        }
        else
        {
            await failCallback(exception);
        }
    }

        private static async Task HandleDefaultExceptionAsync<TResult>(Exception exception,
            Func<Task<TResult>> func,
            Func<TResult, Task> successCallback,
            Func<Exception, Task> failCallback)
        {
            var accepted = await UserDialogs.Instance.ConfirmAsync(L.Localize("DoYouWantToTryAgain"), L.Localize("UnhandledWebRequestException"));

            if (accepted)
            {
                await ExecuteAsync(func, successCallback, failCallback);
            }
            else
            {
                await failCallback(exception);
            }
        }
}

public static class WebRequestExecutorWithParam
{
    public static async Task ExecuteAsync<TParam>(
        Func<TParam, Task> func,
        TParam param,
        Func<Task> successCallback = null,
        Func<Exception, Task> failCallback = null,
        Action finallyCallback = null)
    {
        if (successCallback == null)
        {
            successCallback = () => Task.CompletedTask;
        }

        if (failCallback == null)
        {
            failCallback = _ => Task.CompletedTask;
        }

        try
        {
            if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
            {
                var accepted =
                    await UserDialogs.Instance.ConfirmAsync(L.Localize("DoYouWantToTryAgain"),
                        L.Localize("NoInternet"));

                if (accepted)
                {
                    await ExecuteAsync(func, param, successCallback, failCallback);
                }
                else
                {
                    await failCallback(new Exception(L.Localize("NoInternet")));
                }
            }
            else
            {
                await func(param);
                await successCallback();
            }
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(ex, func, param, successCallback, failCallback);
        }
        finally
        {
            finallyCallback?.Invoke();
        }
    }
    
    public static async Task ExecuteAsync<TParam, TResult>(
        Func<TParam, Task<TResult>> func,
        TParam param,
        Func<TResult, Task> successCallback,
        Func<Exception, Task> failCallback = null,
        Action finallyCallback = null)
    {
        if (successCallback == null)
        {
            successCallback = _ => Task.CompletedTask;
        }

        if (failCallback == null)
        {
            failCallback = _ => Task.CompletedTask;
        }

        try
        {
            if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
            {
                var accepted = await UserDialogs.Instance.ConfirmAsync(L.Localize("DoYouWantToTryAgain"), L.Localize("NoInternet"));

                if (accepted)
                {
                    await ExecuteAsync(func, param, successCallback, failCallback);
                }
                else
                {
                    await failCallback(new Exception(L.Localize("NoInternet")));
                }
            }
            else
            {
                await successCallback(await func(param));
            }
        }
        catch (Exception exception)
        {
            await HandleExceptionAsync(exception, func, param, successCallback, failCallback);
        }
        finally
        {
            finallyCallback?.Invoke();
        }
    }
    
    private static async Task HandleExceptionAsync<TParam, TResult>(
        Exception exception,
        Func<TParam, Task<TResult>> func,
        TParam param,
        Func<TResult, Task> successCallback,
        Func<Exception, Task> failCallback)
    {
        switch (exception)
        {
            case FlurlHttpTimeoutException httpTimeoutException:
                await HandleFlurlHttpTimeoutExceptionAsync(httpTimeoutException, func, param, successCallback, failCallback);
                break;
            case FlurlHttpException httpException:
                await HandleFlurlHttpExceptionAsync(httpException, func, param, successCallback, failCallback);
                break;
            default:
                await HandleDefaultExceptionAsync(exception, func, param, successCallback, failCallback);
                break;
        }
    }
    
    

    private static async Task HandleExceptionAsync<TParam>(
        Exception exception,
        Func<TParam, Task> func,
        TParam param,
        Func<Task> successCallback,
        Func<Exception, Task> failCallback)
    {
        switch (exception)
        {
            case FlurlHttpTimeoutException httpTimeoutException:
                await HandleFlurlHttpTimeoutExceptionAsync(httpTimeoutException, func, param, successCallback, failCallback);
                break;
            case FlurlHttpException httpException:
                await HandleFlurlHttpExceptionAsync(httpException, func, param, successCallback, failCallback);
                break;
            default:
                await HandleDefaultExceptionAsync(exception, func, param, successCallback, failCallback);
                break;
        }
    }

    private static async Task HandleFlurlHttpTimeoutExceptionAsync<TParam, TResult>(
        FlurlHttpTimeoutException httpTimeoutException,
        Func<TParam, Task<TResult>> func,
        TParam param,
        Func<TResult, Task> successCallback,
        Func<Exception, Task> failCallback)
    {
        var accepted =
            await UserDialogs.Instance.ConfirmAsync(L.Localize("DoYouWantToTryAgain"), L.Localize("RequestIsTimedOut"));

        if (accepted)
        {
            await ExecuteAsync(func, param, successCallback, failCallback);
        }
        else
        {
            await failCallback(httpTimeoutException);
        }
        
    }
    
    private static async Task HandleFlurlHttpTimeoutExceptionAsync<TParam>(
        FlurlHttpTimeoutException httpTimeoutException,
        Func<TParam, Task> func,
        TParam param,
        Func<Task> successCallback,
        Func<Exception, Task> failCallback)
    {
        var accepted =
            await UserDialogs.Instance.ConfirmAsync(L.Localize("DoYouWantToTryAgain"), L.Localize("RequestIsTimedOut"));

        if (accepted)
        {
            await ExecuteAsync(func, param, successCallback, failCallback);
        }
        else
        {
            await failCallback(httpTimeoutException);
        }
        
    }
    
    private static async Task HandleFlurlHttpExceptionAsync<TParam>(
        FlurlHttpException httpException,
        Func<TParam, Task> func,
        TParam param,
        Func<Task> successCallback,
        Func<Exception, Task> failCallback)
    {
        var httpExceptionMessage = "";
        if (Debugger.IsAttached)
        {
            httpExceptionMessage += Environment.NewLine + httpException.Message;
        }

        var accepted = await UserDialogs.Instance.ConfirmAsync(httpExceptionMessage + " " + L.Localize("DoYouWantToTryAgain"), L.Localize("HTTPException"));

        if (accepted)
        {
            await ExecuteAsync(func, param, successCallback, failCallback);
        }
        else
        {
            await failCallback(httpException);
        }
    }
    
    private static async Task HandleFlurlHttpExceptionAsync<TParam, TResult>(
        FlurlHttpException httpException,
        Func<TParam, Task<TResult>> func,
        TParam param,
        Func<TResult, Task> successCallback,
        Func<Exception, Task> failCallback)
    {
        var httpExceptionMessage = "";
        if (Debugger.IsAttached)
        {
            httpExceptionMessage += Environment.NewLine + httpException.Message;
        }

        await failCallback(httpException);

    }

    private static async Task HandleDefaultExceptionAsync<TParam>(
        Exception exception,
        Func<TParam, Task> func,
        TParam param,
        Func<Task> successCallback,
        Func<Exception, Task> failCallback)
    {
        await failCallback(exception);
    }

    private static async Task HandleDefaultExceptionAsync<TParam, TResult>(
        Exception exception,
        Func<TParam, Task<TResult>> func,
        TParam param,
        Func<TResult, Task> successCallback,
        Func<Exception, Task> failCallback)
    {
        await failCallback(exception);
    }
}