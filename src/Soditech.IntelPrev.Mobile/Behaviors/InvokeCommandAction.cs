using System;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Internals;
using Microsoft.Extensions.Logging;

namespace Soditech.IntelPrev.Mobile.Behaviors;

[Preserve(AllMembers = true)]
public sealed class InvokeCommandAction : BindableObject, IAction
{
	private readonly ILogger<InvokeCommandAction> _logger =
	Core.Dependency.DependencyResolver.GetRequiredService<ILogger<InvokeCommandAction>>();
	public static readonly BindableProperty CommandProperty = BindableProperty.Create("Command", typeof(ICommand), typeof(InvokeCommandAction), null);
	public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create("CommandParameter", typeof(object), typeof(InvokeCommandAction), null);
	public static readonly BindableProperty InputConverterProperty = BindableProperty.Create("Converter", typeof(IValueConverter), typeof(InvokeCommandAction), null);

	public ICommand Command
	{
		get => (ICommand)GetValue(CommandProperty);
		set => SetValue(CommandProperty, value);
	}

	public object CommandParameter
	{
		get => GetValue(CommandParameterProperty);
		set => SetValue(CommandParameterProperty, value);
	}

	public IValueConverter Converter
	{
		get => (IValueConverter)GetValue(InputConverterProperty);
		set => SetValue(InputConverterProperty, value);
	}

	public bool Execute(object sender, object parameter)
	{
		try
		{
			if (Command == null)
			{
				return false;
			}

			object resolvedParameter;
			if (CommandParameter != null)
			{
				resolvedParameter = CommandParameter;
			}
			else if (Converter != null)
			{
				resolvedParameter = Converter.Convert(parameter, typeof(object), null, null);
			}
			else
			{
				resolvedParameter = parameter;
			}

			if (!Command.CanExecute(resolvedParameter))
			{
				return false;
			}

			Command.Execute(resolvedParameter);
			return true;
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Error executing command in InvokeCommandAction");
			return false;
		}
	}
}