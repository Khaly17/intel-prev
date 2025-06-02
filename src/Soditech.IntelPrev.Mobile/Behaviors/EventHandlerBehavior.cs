using System.Reflection;
using Microsoft.Maui.Controls.Internals;
using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace Soditech.IntelPrev.Mobile.Behaviors;

[Preserve(AllMembers = true)]
[ContentProperty("Actions")]
public sealed class EventHandlerBehavior : BehaviorBase<VisualElement>
{
	private readonly ILogger<EventHandlerBehavior> _logger =
        Core.Dependency.DependencyResolver.GetRequiredService<ILogger<EventHandlerBehavior>>();
	private Delegate _eventHandler;

	public static readonly BindableProperty EventNameProperty = BindableProperty.Create("EventName", typeof(string), typeof(EventHandlerBehavior), null, propertyChanged: OnEventNameChanged);

	public static readonly BindableProperty ActionsProperty = BindableProperty.Create("Actions", typeof(ActionCollection), typeof(EventHandlerBehavior), null);

	private static readonly MethodInfo OnEventMethodInfo = typeof(EventHandlerBehavior).GetTypeInfo().GetDeclaredMethod("OnEvent");

	public string EventName
	{
		get => (string)GetValue(EventNameProperty);
		set => SetValue(EventNameProperty, value);
	}

	public ActionCollection Actions
	{
		get
		{
			var actionCollection = (ActionCollection)GetValue(ActionsProperty);
			if (actionCollection == null)
			{
				actionCollection = new ActionCollection();
				SetValue(ActionsProperty, actionCollection);
			}
			return actionCollection;
		}
	}

	protected override void OnAttachedTo(VisualElement bindable)
	{
		base.OnAttachedTo(bindable);
		RegisterEvent(EventName);
	}

	protected override void OnDetachingFrom(VisualElement bindable)
	{
		DeregisterEvent(EventName);
		base.OnDetachingFrom(bindable);
	}

	private void RegisterEvent(string name)
	{
		if (string.IsNullOrWhiteSpace(name))
		{
			return;
		}

		var eventInfo = AssociatedObject.GetType().GetRuntimeEvent(EventName);
		if (eventInfo == null)
		{
			throw new ArgumentException($"EventHandlerBehavior: Can't register the '{EventName}' event.");
		}

		_eventHandler = OnEventMethodInfo.CreateDelegate(eventInfo.EventHandlerType, this);
		eventInfo.AddEventHandler(AssociatedObject, _eventHandler);
	}

	private void DeregisterEvent(string name)
	{
		if (string.IsNullOrWhiteSpace(name))
		{
			return;
		}

		if (_eventHandler == null)
		{
			return;
		}

		var eventInfo = AssociatedObject.GetType().GetRuntimeEvent(EventName);
		if (eventInfo == null)
		{
			throw new ArgumentException($"EventHandlerBehavior: Can't de-register the '{EventName}' event.");
		}

		eventInfo.RemoveEventHandler(AssociatedObject, _eventHandler);
		_eventHandler = null;
	}

	private void OnEvent(object sender, object eventArgs)
	{
		try
		{
			// Check if we still have a valid context and associated object
			if (AssociatedObject == null || Actions == null)
			{
				return;
			}

			foreach (var bindable in Actions)
			{
				if (bindable == null) continue;

				try
				{
					bindable.BindingContext = BindingContext;

					if (bindable is IAction action)
					{
						if (action is InvokeCommandAction commandAction &&
							commandAction.Command == null)
						{
							// Skip if command is null
							continue;
						}

						action.Execute(sender, eventArgs);
					}
				}
				catch (Exception actionEx)
				{
					// Log but don't crash if an individual action fails
                    _logger.LogError(actionEx, "Error executing action in EventHandlerBehavior");
                }
			}
		}
		catch (Exception ex)
		{
			// Log but don't crash if event handler fails
            _logger.LogError(ex, "Error in event handler");
        }
	}

	private static void OnEventNameChanged(BindableObject bindable, object oldValue, object newValue)
	{
		var behavior = (EventHandlerBehavior)bindable;
		if (behavior.AssociatedObject == null)
		{
			return;
		}

		var oldEventName = (string)oldValue;
		var newEventName = (string)newValue;

		behavior.DeregisterEvent(oldEventName);
		behavior.RegisterEvent(newEventName);
	}
}