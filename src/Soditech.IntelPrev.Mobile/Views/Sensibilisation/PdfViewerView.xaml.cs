using System.Collections.Generic;
using Soditech.IntelPrev.Mobile.ViewModels.Sensibilisation;
using Microsoft.Maui.Controls;
using System.Reflection;

namespace Soditech.IntelPrev.Mobile.Views.Sensibilisation;

public partial class PdfViewerView : ContentPage, IQueryAttributable
{
    public PdfViewerView()
    {
        InitializeComponent();
        BindingContext = new RiskPreventionViewModel();
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("DocumentPath", out var documentPath) && documentPath is string path && BindingContext is RiskPreventionViewModel viewModel)
        {
            var assembly = typeof(App).GetTypeInfo().Assembly;
            viewModel.PdfDocumentStream = assembly.GetManifestResourceStream($"Soditech.IntelPrev.Mobile.Assets.{path}");
            pdfviewer.DocumentSource = viewModel.PdfDocumentStream;
        }
    }
}
