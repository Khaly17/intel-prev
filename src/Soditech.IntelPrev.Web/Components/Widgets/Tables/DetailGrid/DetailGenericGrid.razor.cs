using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Navigations;
using ChangeEventArgs = Microsoft.AspNetCore.Components.ChangeEventArgs;

namespace Soditech.IntelPrev.Web.Components.Widgets.Tables.DetailGrid;

public partial class DetailGenericGrid<TItem> : ComponentBase
{
    [Parameter] public Action<ActionEventArgs<TItem>> Onactioncomplete { get; set; }
    [Parameter] public string GridTitle { get; set; } = default!;
    [Parameter] public int PageSize { get; set; } = 5;
    [Parameter] public IEnumerable<TItem> Items { get; set; } = default!;
    [Parameter] public EventCallback<string> OnToolbarClick { get; set; }
    [Parameter] public EventCallback<TItem> OnAdd { get; set; }
    [Parameter] public Action<TItem>? DetailAction { get; set; } = default!;
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter] public List<string> ToolbarItems { get; set; } = new List<string>();
    [Parameter] public List<GridColumnDefinition<TItem>> Columns { get; set; } = new List<GridColumnDefinition<TItem>>();
    [Parameter] public bool DisplayActionsBtn { get; set; } = true;
    [Parameter] public bool DisplayPagination { get; set; } = true;
    [Parameter] public EventCallback addButton { get; set; }
    [Parameter] public EventCallback<TItem> OnCustomAction { get; set; }
    [Parameter] public bool DisplayTitle { get; set; } = true;
    [Parameter] public int PageCount { get; set; } = default;
    [Parameter] public int CurrentPage { get; set; } = 1; // Current page from parent
    [Parameter] public int TotalCount { get; set; } // Total number of items from backend
    [Parameter] public List<int> PageSizeOptions { get; set; } = new List<int> { 5, 10, 20, 50 }; // Options for page sizes
    [Parameter] public EventCallback<PageChangedEventArgs> OnPageChanged { get; set; }
    [Parameter] public EventCallback<QueryCellInfoEventArgs<TItem>> Callback { get; set; }
    
    [Parameter] public EventCallback<PageSizeChangedArgs> PchangedEvent  { get; set; } 
    private SfGrid<TItem> GenGrid { get; set; }
    [Parameter] public bool Enablehover { get; set; }
    [Parameter] public bool? CustomSearch { get; set; } = false;

    [Parameter] public string TextValue { get; set; } = string.Empty;
    [Parameter] public string DetailBtnLabel { get; set; } = "Detail";
    [Parameter] public string CustomBtnLabel { get; set; } = "Custom";
    [Parameter] public EventCallback<string> TextValueChanged { get; set; }
    [Parameter] public bool DisplaySpinnerOnLoading { get; set; }



    private async Task OnPageChangedHandler(GridPageChangedEventArgs args)
    {
        await OnPageChanged.InvokeAsync(args);
    }



    public void ToolbarClickHandler(Syncfusion.Blazor.Navigations.ClickEventArgs args)
    {
        OnToolbarClick.InvokeAsync(args.Item.Text);
        StateHasChanged();
    }

    private Task OnPageSizeChanged(ChangeEventArgs e)
    {
        if (int.TryParse(e.Value.ToString(), out var newSize))
        {
            PageSize = newSize; // Update page size
            CurrentPage = 1; // Reset to first page on page size change
        }
        StateHasChanged();

        return Task.CompletedTask;
    }
    [Parameter] public Func<Task> OnSearchMethod { get; set; } = default!;

    
    private async Task SearchMethod()
    {
        CustomSearch = true;
        DisplaySpinnerOnLoading = true;
        await TextValueChanged.InvokeAsync(TextValue);
        await OnSearchMethod();
        await this.GenGrid.SearchAsync(TextValue);
        DisplaySpinnerOnLoading = false;
        StateHasChanged();

    }

    
}


public class GridColumnDefinition<TItem>
{
    public string Field { get; set; } = string.Empty;
    public string HeaderText { get; set; } = string.Empty;
    public string Width { get; set; } = "80";
    public string Format { get; set; } = string.Empty;
    public RenderFragment<TItem>? HtmlContent { get; set; }
}