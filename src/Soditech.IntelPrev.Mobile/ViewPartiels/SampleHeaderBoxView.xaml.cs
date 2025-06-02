namespace Soditech.IntelPrev.Mobile.ViewPartiels;

public partial class SampleHeaderBoxView : ContentView
{
    public SampleHeaderBoxView ()
    {
        InitializeComponent ();

        BindingContext = this;
    }

    public string Instruction
    {
        get => (string)GetValue(InstructionProperty);
        set => SetValue(InstructionProperty, value);
    }

    public static readonly BindableProperty InstructionProperty = BindableProperty.Create(
        propertyName: nameof(Instruction),
        returnType: typeof(string),
        declaringType: typeof(SampleHeaderBoxView),
        defaultValue: string.Empty,
        defaultBindingMode: BindingMode.OneWay
    );
    
    public string SubInstruction
    {
        get => (string)GetValue(SubInstructionProperty);
        set => SetValue(SubInstructionProperty, value);
    }

    public static readonly BindableProperty SubInstructionProperty = BindableProperty.Create(
        propertyName: nameof(SubInstruction),
        returnType: typeof(string),
        declaringType: typeof(SampleHeaderBoxView),
        defaultValue: string.Empty,
        defaultBindingMode: BindingMode.OneWay
    );
}