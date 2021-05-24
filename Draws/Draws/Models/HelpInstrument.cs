using Draws.Helpers.Enums;
using Prism.Mvvm;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace Draws.Models
{
    public class HelpInstrument : BindableBase
    {
        private Color _iconColor;
        public HelpInstrument(string text, 
            HelpInstrumentType helpInstrumentType, 
            string icon, 
            Color iconColor)
        {
            Text = text;
            HelpInstrumentType = helpInstrumentType;
            Icon = icon;
            IconColor = iconColor;
        }
        
        public IAsyncCommand<HelpInstrument> ClickCommand { get; set; }
        
        public string Text { get; }

        public Color IconColor
        {
            get => _iconColor;
            set
            {
                _iconColor = value;
                RaisePropertyChanged();
            }
        }

        public string Icon { get; }
        
        public HelpInstrumentType HelpInstrumentType { get; }
    }
}