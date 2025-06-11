using Microsoft.Maui.Graphics;

namespace Soditech.IntelPrev.Mobile.Controls;

public partial class ThemedCard : Border
{
	/// <summary>
	/// Gets or sets whether this card has a shadow
	/// </summary>
	public bool HasShadow
	{
		get => Shadow?.Opacity > 0;
		set
		{
			if (Shadow != null)
			{
				Shadow.Opacity = value ? 0.5f : 0;
			}
		}
	}

	public enum CardStyle
	{
		Default,
		Elevated,
		Bordered,
		Flat
	}

	public static readonly BindableProperty CardStyleProperty =
		BindableProperty.Create(nameof(CardStyleType), typeof(CardStyle), typeof(ThemedCard), CardStyle.Default,
			propertyChanged: OnCardStyleChanged);

	public CardStyle CardStyleType
	{
		get => (CardStyle)GetValue(CardStyleProperty);
		set => SetValue(CardStyleProperty, value);
	}

	public ThemedCard()
	{
		InitializeComponent();
		ApplyStyle(CardStyleType);
	}

	private static void OnCardStyleChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (bindable is ThemedCard card && newValue is CardStyle style)
		{
			card.ApplyStyle(style);
		}
	}

	private void ApplyStyle(CardStyle style)
	{
		switch (style)
		{
			case CardStyle.Default:
				HasShadow = true;
				StrokeThickness = 0;
				Stroke = null;
				Margin = new Thickness(0, 0, 0, 16);
				break;

			case CardStyle.Elevated:
				HasShadow = true;
				if (Shadow != null)
				{
					Shadow.Radius = 12;
					Shadow.Offset = new Point(0, 4);
				}
				StrokeThickness = 0;
				Stroke = null;
				Margin = new Thickness(4, 4, 4, 16);
				break;

			case CardStyle.Bordered:
				HasShadow = false;
				StrokeThickness = 1;
				Stroke = Application.Current.Resources.TryGetValue("Border", out var borderColor)
					? (Color)borderColor
					: Colors.LightGray;
				Margin = new Thickness(0, 0, 0, 16);
				break;

			case CardStyle.Flat:
				HasShadow = false;
				StrokeThickness = 0;
				Stroke = null;
				Margin = new Thickness(0, 0, 0, 16);
				break;

			default:
				throw new ArgumentOutOfRangeException(nameof(style), $"Unhandled card style: {style}");
		}

	}
}
