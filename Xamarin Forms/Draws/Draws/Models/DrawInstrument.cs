using Draws.Helpers.Enums;
using Prism.Mvvm;
using Xamarin.Forms;

namespace Draws.Models
{
    public class DrawInstrument : BindableBase
    {
        private bool _isSelected;

        private string _icon;

        public DrawInstrument(DrawInstrumentType instrumentType, 
            string icon, 
            Color selectedIconColor, 
            Color unselectedIconColor)
        {
            InstrumentType = instrumentType;
            Icon = icon;
            SelectedIconColor = selectedIconColor;
            UnselectedIconColor = unselectedIconColor;
        }
        
        public Color SelectedIconColor { get; }
        
        public Color UnselectedIconColor { get; }
        
        public Color IconColor => IsSelected ? SelectedIconColor : UnselectedIconColor;

        public string Icon
        {
            get => _icon;
            set
            {
                _icon = value;
                RaisePropertyChanged();
            }
        }
        
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(IconColor));
            }
        }
        
        public DrawInstrumentType InstrumentType { get; }
    }
}