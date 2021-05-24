using System;
using System.Threading.Tasks;
using Draws.Helpers;
using Draws.Helpers.Extensions;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace Draws.Controls.Dialogs
{
    public partial class ColorPickerDialog : PopupPage
    {
        private string _title;

        public ColorPickerDialog()
        {
            InitializeComponent();
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
            CanvasInstruments.InstrumentColor = ColorPicker.SelectedColor.GetHexString();
            await PopupNavigation.Instance.PopAsync();
            Ntcs?.TrySetResult(true);
        }

        private async void OnClose(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync();
            Ntcs?.TrySetResult(false);
        }
    }
}