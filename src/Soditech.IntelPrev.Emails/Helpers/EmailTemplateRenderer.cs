using System;
using System.IO;
using System.Threading.Tasks;
using Scriban;

namespace Soditech.IntelPrev.Emails.Helpers;

/// <summary>
/// Implementation of <see cref="IEmailTemplateRenderer"/> that uses `Scriban` to render email templates from .cshtml file.
/// </summary>
public class EmailTemplateRenderer : IEmailTemplateRenderer
{
    public async Task<string> RenderTemplateAsync(string relativePath, object model)
    {
        var path = Path.Combine(AppContext.BaseDirectory, relativePath);
        if (!File.Exists(path))
            throw new FileNotFoundException($"Template '{path}' not found.");

        var content = await File.ReadAllTextAsync(path);
        var template = Template.Parse(content);

        if (template.HasErrors)
            throw new InvalidOperationException($"Template parse errors: {string.Join(", ", template.Messages)}");

        return await template.RenderAsync(model, member => member.Name);
    }
}
