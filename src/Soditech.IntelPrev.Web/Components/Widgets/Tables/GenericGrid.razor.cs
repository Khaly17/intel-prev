using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Inputs;

namespace Soditech.IntelPrev.Web.Components.Widgets.Tables;

public partial class GenericGrid<TItem> : SfGrid<TItem>
{
    [Parameter] public Action<ActionEventArgs<TItem>> Onactioncomplete { get; set; }
    [Parameter] public string GridTitle { get; set; } = default!;
    [Parameter] public int PageSize { get; set; } = 5;
    [Parameter] public IEnumerable<TItem> Items { get; set; } = default!;
    [Parameter] public EventCallback<string> OnToolbarClick { get; set; }
    [Parameter] public EventCallback<TItem> OnAdd { get; set; }
    [Parameter] public EventCallback<TItem> OnEdit { get; set; }
    [Parameter] public EventCallback<TItem> OnDelete { get; set; }
    [Parameter] public Action<TItem>? DetailAction { get; set; } = default!;
    [Parameter] public RenderFragment? CustomChildContent { get; set; }
    [Parameter] public List<string> ToolbarItems { get; set; } = ["Search", "ExcelExport", "PdfExport", "Print"];
    //[Parameter] public List<GridColumnDefinition> Columns { get; set; } = new List<GridColumnDefinition>();
    [Parameter] public bool DisplayActionsBtn { get; set; } = true;
    [Parameter] public EventCallback addButton { get; set; }
    [Parameter] public EventCallback<TItem> OnCustomAction { get; set; }
    [Parameter] public bool DisplayTitle { get; set; } = true;
    [Parameter] public bool DisplayPagination { get; set; } = true;
    [Parameter] public bool DisplayPageSize { get; set; } = true;
    private string[] _pagerDropdown = ["All", "5", "10", "15", "20"];
    [Parameter] public int PageCount { get; set; } = default;
    [Parameter] public int CurrentPage { get; set; } = 1; // Current page from parent
    [Parameter] public int TotalCount { get; set; } // Total number of items from backend
   
    public SfGrid<TItem> GenGrid { get; set; }

    private SfTextBox _textBox;
    [Parameter] public string TextValue { get; set; }
    [Parameter] public string DetailBtnLabel { get; set; } = "Detail"; 
    [Parameter] public string CustomBtnLabel { get; set; } = "Custom"; 
    [Parameter] public string CustomBtnIconCss { get; set; } = "fa fa-refresh"; 
    [Parameter] public EventCallback<string> TextValueChanged { get; set; }
    [Parameter] public bool DisplaySpinnerOnLoading { get; set; }
    private string ActionBtnWidth
    {
        get
        {
            // Calculate the width of the action buttons based on the number of buttons
            var btnCount = 0;
            if (DetailAction != null) btnCount+=100;
            if (OnEdit.HasDelegate) btnCount+=100;
            if (OnDelete.HasDelegate) btnCount+=120;
            if (OnCustomAction.HasDelegate) btnCount+= 10 + CustomBtnLabel.Length * 14; 
            return btnCount.ToString();
        }
    }
        

      

    public void ToolbarClickHandler(Syncfusion.Blazor.Navigations.ClickEventArgs args)
    {
        OnToolbarClick.InvokeAsync(args.Item.Text);
    }

    private Task OnPageSizeChanged(ChangeEventArgs e)
    {
        if (int.TryParse(e.Value.ToString(), out var newSize))
        {
            PageSize = newSize; // Update page size
            CurrentPage = 1; // Reset to first page on page size change
        }

        return Task.CompletedTask;
    }
        
}