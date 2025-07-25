﻿using System.Threading.Tasks;

namespace Soditech.IntelPrev.Emails.Helpers;

/// <summary>
/// Interface for rendering email templates.
/// </summary>
public interface IEmailTemplateRenderer
{
    /// <summary>
    /// Renders the specified template with the given model.
    /// </summary>
    /// <param name="relativePath">The relative path of the view template.</param>
    /// <param name="model">The model to pass to the view template.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the rendered template as a string.</returns>
    Task<string> RenderTemplateAsync(string relativePath, object model);
}