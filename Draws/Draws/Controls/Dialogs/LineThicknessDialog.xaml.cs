using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Draws.Helpers;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Draws.Controls.Dialogs
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LineThicknessDialog
    {
        private string _title;

        public LineThicknessDialog()
        {
            InitializeComponent();

            Slider.Value = CanvasInstruments.InstrumentsThickness;
            LineThicknessBox.HeightRequest = CanvasInstruments.InstrumentsThickness;
            LineThicknessLabel.Text = $"Line thickness size: {CanvasInstruments.InstrumentsThickness}";
            LineThicknessBox.BackgroundColor = Color.FromHex(CanvasInstruments.InstrumentColor);
        }

        public TaskCompletionSource<bool> Ntcs { get; set; }

        public new string Title
        {
            get => _title;
            set
            {
                _title = value;
                TitleLabel.Text = value;
            }
        }

        private async void OnAccept(object sender, EventArgs e)
        {
            CanvasInstruments.InstrumentsThickness = (int)Slider.Value;
            await PopupNavigation.Instance.PopAsync();
            Ntcs?.TrySetResult(true);
        }

        private async void OnClose(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync();
            Ntcs?.TrySetResult(false);
        }
        
        void OnSliderValueChanged(object sender, ValueChangedEventArgs args)
        {
            var intValue = (int)args.NewValue;
            Slider.Value = intValue;
            
            LineThicknessBox.HeightRequest = intValue;
            LineThicknessLabel.Text = $"Line thickness size: {intValue}";
        }
    }
}