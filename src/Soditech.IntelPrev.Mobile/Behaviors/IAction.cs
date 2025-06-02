using Microsoft.Maui.Controls.Internals;

namespace Soditech.IntelPrev.Mobile.Behaviors;

[Preserve(AllMembers = true)]
public interface IAction
{
    bool Execute(object sender, object parameter);
}