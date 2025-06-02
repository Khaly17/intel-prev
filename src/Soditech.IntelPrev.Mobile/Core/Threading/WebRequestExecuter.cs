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
    public static async Task Execute<TResult>(
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
                        await Execute(func, successCallback, failCallback);
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
                await HandleException(exception, func, successCallback, failCallback);
            }
            finally
            {
                finallyCallback?.Invoke();
            }
    }

        public static async Task Execute(
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
                        await Execute(func, successCallback, failCallback);
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
                await HandleException(ex, func, successCallback, failCallback);
            }
            finally
            {
                finallyCallback?.Invoke();
            }
        }

        private static async Task HandleException<TResult>(Exception exception,
            Func<Task<TResult>> func,
            Func<TResult, Task> successCallback,
            Func<Exception, Task> failCallback)
        {
            switch (exception)
            {
                case FlurlHttpTimeoutException httpTimeoutException:
                    await HandleFlurlHttpTimeoutException(httpTimeoutException, func, successCallback, failCallback);
                    break;
                case FlurlHttpException httpException:
                    await HandleFlurlHttpException(httpException, func, successCallback, failCallback);
                    break;
                default:
                    await HandleDefaultException(exception, func, successCallback, failCallback);
                    break;
            }
        }

        private static async Task HandleException(Exception exception,
            Func<Task> func,
            Func<Task> successCallback,
            Func<Exception, Task> failCallback)
        {

            switch (exception)
            {
                case FlurlHttpTimeoutException httpTimeoutException:
                    await HandleFlurlHttpTimeoutException(httpTimeoutException, func, successCallback, failCallback);
                    break;
                case FlurlHttpException httpException:
                    await HandleFlurlHttpException(httpException, func, successCallback, failCallback);
                    break;
                default:
                    await HandleDefaultException(exception, func, successCallback, failCallback);
                    break;
            }
        }
        
        private static async Task HandleFlurlHttpTimeoutException<TResult>(
            FlurlHttpTimeoutException httpTimeoutException,
            Func<Task<TResult>> func,
            Func<TResult, Task> successCallback,
            Func<Exception, Task> failCallback)
        {
            var accepted = await UserDialogs.Instance.ConfirmAsync(L.Localize("DoYouWantToTryAgain"), L.Localize("RequestIsTimedOut"));

            if (accepted)
            {
                await Execute(func, successCallback, failCallback);
            }
            else
            {
                await failCallback(httpTimeoutException);
            }
        }

        private static async Task HandleFlurlHttpTimeoutException(FlurlHttpTimeoutException httpTimeoutException,
            Func<Task> func,
            Func<Task> successCallback,
            Func<Exception, Task> failCallback)
        {
            var accepted = await UserDialogs.Instance.ConfirmAsync(L.Localize("DoYouWantToTryAgain"), L.Localize("RequestIsTimedOut"));

            if (accepted)
            {
                await Execute(func, successCallback, failCallback);
            }
            else
            {
                await failCallback(httpTimeoutException);
            }
        }

        private static async Task HandleFlurlHttpException(FlurlHttpException httpException,
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
                await Execute(func, successCallback, failCallback);
            }
            else
            {
                await failCallback(httpException);
            }
        }

        private static async Task HandleFlurlHttpException<TResult>(FlurlHttpException httpException,
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
                await Execute(func, successCallback, failCallback);
            }
            else
            {
                await failCallback(httpException);
            }
        }
        
        private static async Task HandleDefaultException(Exception exception,
            Func<Task> func,
            Func<Task> successCallback,
            Func<Exception, Task> failCallback)
        {

        var accepted = await UserDialogs.Instance.ConfirmAsync(L.Localize("DoYouWantToTryAgain"), L.Localize("UnhandledWebRequestException"));

        if (accepted)
        {
            await Execute(func, successCallback, failCallback);
        }
        else
        {
            await failCallback(exception);
        }
    }

        private static async Task HandleDefaultException<TResult>(Exception exception,
            Func<Task<TResult>> func,
            Func<TResult, Task> successCallback,
            Func<Exception, Task> failCallback)
        {
            var accepted = await UserDialogs.Instance.ConfirmAsync(L.Localize("DoYouWantToTryAgain"), L.Localize("UnhandledWebRequestException"));

            if (accepted)
            {
                await Execute(func, successCallback, failCallback);
            }
            else
            {
                await failCallback(exception);
            }
        }
}

public static class WebRequestExecutorWithParam
{
    public static async Task Execute<TParam>(
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
                    await Execute(func, param, successCallback, failCallback);
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
            await HandleException(ex, func, param, successCallback, failCallback);
        }
        finally
        {
            finallyCallback?.Invoke();
        }
    }
    
    public static async Task Execute<TParam, TResult>(
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
                    await Execute(func, param, successCallback, failCallback);
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
            await HandleException(exception, func, param, successCallback, failCallback);
        }
        finally
        {
            finallyCallback?.Invoke();
        }
    }
    
    private static async Task HandleException<TParam, TResult>(
        Exception exception,
        Func<TParam, Task<TResult>> func,
        TParam param,
        Func<TResult, Task> successCallback,
        Func<Exception, Task> failCallback)
    {
        switch (exception)
        {
            case FlurlHttpTimeoutException httpTimeoutException:
                await HandleFlurlHttpTimeoutException(httpTimeoutException, func, param, successCallback, failCallback);
                break;
            case FlurlHttpException httpException:
                await HandleFlurlHttpException(httpException, func, param, successCallback, failCallback);
                break;
            default:
                await HandleDefaultException(exception, func, param, successCallback, failCallback);
                break;
        }
    }
    
    

    private static async Task HandleException<TParam>(
        Exception exception,
        Func<TParam, Task> func,
        TParam param,
        Func<Task> successCallback,
        Func<Exception, Task> failCallback)
    {
        switch (exception)
        {
            case FlurlHttpTimeoutException httpTimeoutException:
                await HandleFlurlHttpTimeoutException(httpTimeoutException, func, param, successCallback, failCallback);
                break;
            case FlurlHttpException httpException:
                await HandleFlurlHttpException(httpException, func, param, successCallback, failCallback);
                break;
            default:
                await HandleDefaultException(exception, func, param, successCallback, failCallback);
                break;
        }
    }

    private static async Task HandleFlurlHttpTimeoutException<TParam, TResult>(
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
            await Execute(func, param, successCallback, failCallback);
        }
        else
        {
            await failCallback(httpTimeoutException);
        }
        
    }
    
    private static async Task HandleFlurlHttpTimeoutException<TParam>(
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
            await Execute(func, param, successCallback, failCallback);
        }
        else
        {
            await failCallback(httpTimeoutException);
        }
        
    }
    
    private static async Task HandleFlurlHttpException<TParam>(
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
            await Execute(func, param, successCallback, failCallback);
        }
        else
        {
            await failCallback(httpException);
        }
    }
    
    private static async Task HandleFlurlHttpException<TParam, TResult>(
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

    private static async Task HandleDefaultException<TParam>(
        Exception exception,
        Func<TParam, Task> func,
        TParam param,
        Func<Task> successCallback,
        Func<Exception, Task> failCallback)
    {
        await failCallback(exception);
    }

    private static async Task HandleDefaultException<TParam, TResult>(
        Exception exception,
        Func<TParam, Task<TResult>> func,
        TParam param,
        Func<TResult, Task> successCallback,
        Func<Exception, Task> failCallback)
    {
        await failCallback(exception);
    }
}