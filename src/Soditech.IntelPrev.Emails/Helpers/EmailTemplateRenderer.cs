using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;

namespace Soditech.IntelPrev.Emails.Helpers;

/// <summary>
/// Implementation of <see cref="IEmailTemplateRenderer"/> that uses Razor views to render email templates.
/// </summary>
public class EmailTemplateRenderer : IEmailTemplateRenderer
{
    private readonly IRazorViewEngine _viewEngine;
    private readonly IActionContextAccessor _actionContextAccessor;
    private readonly ITempDataProvider _tempDataProvider;
    private readonly ILogger<EmailTemplateRenderer> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="EmailTemplateRenderer"/> class.
    /// </summary>
    /// <param name="viewEngine">The Razor view engine.</param>
    /// <param name="actionContextAccessor">The action context accessor.</param>
    /// <param name="tempDataProvider">The temp data provider.</param>
    /// <param name="logger">The logger.</param>
    public EmailTemplateRenderer(IRazorViewEngine viewEngine, IActionContextAccessor actionContextAccessor, ITempDataProvider tempDataProvider, ILogger<EmailTemplateRenderer> logger)
    {
        _viewEngine = viewEngine;
        _actionContextAccessor = actionContextAccessor;
        _tempDataProvider = tempDataProvider;
        _logger = logger;
    }

    /// <summary>
    /// Renders the specified template with the given model.
    /// </summary>
    /// <typeparam name="T">The type of the model.</typeparam>
    /// <param name="viewName">The name of the view template.</param>
    /// <param name="model">The model to pass to the view template.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the rendered template as a string.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the action context is null.</exception>
    /// <exception cref="FileNotFoundException">Thrown when the view template is not found.</exception>
    public async Task<string> RenderTemplateAsync<T>(string viewName, T model)
    {
        try
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model), "The model cannot be null.");
            }

            var actionContext = _actionContextAccessor.ActionContext;
            if (actionContext == null)
            {
                throw new InvalidOperationException("ActionContext cannot be null.");
            }

            if (string.IsNullOrEmpty(viewName))
            {
                throw new ArgumentException("View name cannot be null or empty.", nameof(viewName));
            }

            var viewResult = _viewEngine.FindView(actionContext, viewName, false);

            if (viewResult.View == null)
            {
                throw new FileNotFoundException("View not found", viewName);
            }

            var viewData = new ViewDataDictionary<T>(new EmptyModelMetadataProvider(), new ModelStateDictionary())
            {
                Model = model
            };

            await using var sw = new StringWriter();
            var viewContext = new ViewContext(
                actionContext,
                viewResult.View,
                viewData,
                new TempDataDictionary(actionContext.HttpContext, _tempDataProvider),
                sw,
                new HtmlHelperOptions()
            );

            await viewResult.View.RenderAsync(viewContext);
            return sw.ToString();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Cannot load template `{viewName}` for `{model}`", viewName, typeof(T).Name);
            return string.Empty;
        }
    }
}
