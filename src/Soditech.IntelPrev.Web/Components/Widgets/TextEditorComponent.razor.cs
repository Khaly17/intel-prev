using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.RichTextEditor;
using ChangeEventArgs = Microsoft.AspNetCore.Components.ChangeEventArgs;

namespace Soditech.IntelPrev.Web.Components.Widgets;

public partial class TextEditorComponent
{
    private string _value = string.Empty;

    [Parameter]
    public string Value
    {
        get => _value;
        set
        {
            if (_value != value)
            {
                _value = value;
                ValueChanged.InvokeAsync(value);
            }
        }
    }

    [Parameter]
    public EventCallback<string> ValueChanged { get; set; }
    private void OnValueChanged(string newValue)
    {
        Value = newValue;
    }
    private void OnInput(ChangeEventArgs e)
    {
        Value = e.Value.ToString();
    }
    
    private readonly List<ToolbarItemModel> _tools =
    [
        new() { Command = ToolbarCommand.Bold },
        new() { Command = ToolbarCommand.Italic },
        new() { Command = ToolbarCommand.Underline },
        new() { Command = ToolbarCommand.StrikeThrough },
        new() { Command = ToolbarCommand.FontName },
        new() { Command = ToolbarCommand.FontSize },
        new() { Command = ToolbarCommand.Separator },
        new() { Command = ToolbarCommand.FontColor },
        new() { Command = ToolbarCommand.BackgroundColor },
        new() { Command = ToolbarCommand.Separator },
        new() { Command = ToolbarCommand.Formats },
        new() { Command = ToolbarCommand.Alignments },
        new() { Command = ToolbarCommand.Separator },
        new() { Command = ToolbarCommand.LowerCase },
        new() { Command = ToolbarCommand.UpperCase },
        new() { Command = ToolbarCommand.SuperScript },
        new() { Command = ToolbarCommand.SubScript },
        new() { Command = ToolbarCommand.Separator },
        new() { Command = ToolbarCommand.OrderedList },
        new() { Command = ToolbarCommand.UnorderedList },
        new() { Command = ToolbarCommand.Outdent },
        new() { Command = ToolbarCommand.Indent },
        new() { Command = ToolbarCommand.Separator },
        new() { Command = ToolbarCommand.CreateLink },
        new() { Command = ToolbarCommand.Image },
        new() { Command = ToolbarCommand.CreateTable },
        new() { Command = ToolbarCommand.Separator },
        new() { Command = ToolbarCommand.ClearFormat },
        new() { Command = ToolbarCommand.Print },
        new() { Command = ToolbarCommand.SourceCode },
        new() { Command = ToolbarCommand.FullScreen },
        new() { Command = ToolbarCommand.Separator },
        new() { Command = ToolbarCommand.Undo },
        new() { Command = ToolbarCommand.Redo }
    ];
}
