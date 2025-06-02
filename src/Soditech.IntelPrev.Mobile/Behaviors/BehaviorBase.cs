using System;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Internals;

namespace Soditech.IntelPrev.Mobile.Behaviors;

[Preserve(AllMembers = true)]
public class BehaviorBase<T> : Behavior<T> where T : BindableObject
{
    protected T AssociatedObject { get; private set; }

    protected override void OnAttachedTo(T bindable)
    {
        base.OnAttachedTo(bindable);
        AssociatedObject = bindable;

        if (bindable.BindingContext != null)
        {
            BindingContext = bindable.BindingContext;
        }

        bindable.BindingContextChanged += OnBindingContextChanged;
    }

    protected override void OnDetachingFrom(T bindable)
    {
        base.OnDetachingFrom(bindable);
        bindable.BindingContextChanged -= OnBindingContextChanged;
        AssociatedObject = null;
    }

    private void OnBindingContextChanged(object sender, EventArgs e)
    {
        OnBindingContextChanged();
    }

    protected override void OnBindingContextChanged()
    {
        base.OnBindingContextChanged();
        BindingContext = AssociatedObject.BindingContext;
    }
}