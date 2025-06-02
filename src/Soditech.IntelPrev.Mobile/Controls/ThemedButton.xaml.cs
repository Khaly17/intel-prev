using Soditech.IntelPrev.Mobile.Helpers;

namespace Soditech.IntelPrev.Mobile.Controls;

public partial class ThemedButton : Button
{
    public enum ButtonStyle
    {
        Primary,
        Secondary,
        Outline,
        Danger
    }

    public static readonly BindableProperty ButtonStyleProperty = 
        BindableProperty.Create(nameof(Style), typeof(ButtonStyle), typeof(ThemedButton), ButtonStyle.Primary, 
            propertyChanged: OnButtonStyleChanged);

    public ButtonStyle ButtonStyleType
    {
        get => (ButtonStyle)GetValue(ButtonStyleProperty);
        set => SetValue(ButtonStyleProperty, value);
    }

    public ThemedButton()
    {
        InitializeComponent();
        ApplyStyle(ButtonStyleType);
    }

    private static void OnButtonStyleChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is ThemedButton button && newValue is ButtonStyle style)
        {
            button.ApplyStyle(style);
        }
    }

    private void ApplyStyle(ButtonStyle style)
    {
        switch (style)
        {
            case ButtonStyle.Primary:
                BackgroundColor = ThemeHelper.Primary;
                TextColor = ThemeHelper.PureWhite;
                BorderWidth = 0;
                break;
            
            case ButtonStyle.Secondary:
                BackgroundColor = ThemeHelper.Secondary;
                TextColor = ThemeHelper.PureWhite;
                BorderWidth = 0;
                break;
            
            case ButtonStyle.Outline:
                BackgroundColor = Microsoft.Maui.Graphics.Colors.Transparent;
                TextColor = ThemeHelper.Primary;
                BorderColor = ThemeHelper.Primary;
                BorderWidth = 1;
                break;
            
            case ButtonStyle.Danger:
                BackgroundColor = ThemeHelper.Danger;
                TextColor = ThemeHelper.PureWhite;
                BorderWidth = 0;
                break;
        }
    }
}
