using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Draws.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DrawInstrumentControl
    {
        public static readonly BindableProperty IsSelectedProperty =
            BindableProperty.Create(nameof(IsSelected), typeof(bool), 
                typeof(DrawInstrumentControl), false, 
                propertyChanged: OnIsSelectedChanged, defaultBindingMode: BindingMode.TwoWay);
        
        public DrawInstrumentControl()
        {
            InitializeComponent();
        }
        
        public bool IsSelected
        {
            get => (bool) GetValue(IsSelectedProperty);
            set => SetValue(IsSelectedProperty, value);
        }
        
        private static void OnIsSelectedChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (!(bindable is DrawInstrumentControl instrumentItem))
                return;
            
            if (!(newValue is bool isSelected))
                return;

            instrumentItem.ItemFrame.Style = isSelected
                ? (Style) instrumentItem.Resources["SelectedFrameStyle"]
                : (Style) instrumentItem.Resources["UnselectedFrameStyle"];
            
            instrumentItem.ItemIcon.Style = isSelected
                ? (Style) instrumentItem.Resources["SelectedLabelStyle"]
                : (Style) instrumentItem.Resources["UnselectedLabelStyle"];
        }
    }
}