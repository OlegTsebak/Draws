using System;
using Draws.Models;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Draws.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HelpInstrumentControl
    {
        public static readonly BindableProperty CommandProperty =
            BindableProperty.Create(nameof(Command), typeof(IAsyncCommand<HelpInstrument>), 
                typeof(DrawInstrumentControl));

        private HelpInstrument _helpInstrument;
        
        public HelpInstrumentControl()
        {
            InitializeComponent();
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            if (BindingContext is HelpInstrument helpInstrument)
                _helpInstrument = helpInstrument;
        }

        public IAsyncCommand<HelpInstrument> Command
        {
            get => (IAsyncCommand<HelpInstrument>) GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }
        
        private void OnHelpInstrumentClicked(object sender, EventArgs e)
        {
            Command?.ExecuteAsync(_helpInstrument);
        }
    }
}