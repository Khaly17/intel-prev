﻿using System;
using System.Collections.Specialized;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Internals;

namespace Soditech.IntelPrev.Mobile.Behaviors;

[Preserve(AllMembers = true)]
public class ActionCollection : BindableObjectCollection
{
    public ActionCollection()
    {
        CollectionChanged += ActionCollection_CollectionChanged;
    }

    private void ActionCollection_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        var collectionChange = e.Action;

        if (collectionChange == NotifyCollectionChangedAction.Reset)
        {
            foreach (var bindable in this)
            {
                VerifyType(bindable);
            }
        }
        else if (collectionChange == NotifyCollectionChangedAction.Replace)
        {
            var changed = this[(int)e.NewStartingIndex];
            VerifyType(changed);
        }
    }

    private static void VerifyType(BindableObject bindable)
    {
        if (bindable is not IAction)
        {
            throw new InvalidOperationException("Non-IAction added to IAction collection");
        }
    }
}