using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Draws.Helpers;
using Draws.Helpers.Enums;
using Draws.Helpers.Extensions;
using Draws.Managers;
using Draws.Models;
using Draws.Services;
using Draws.ViewModels.Bases;
using Prism.Navigation;
using SkiaSharp;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace Draws.ViewModels
{
    public class DrawPageViewModel : BaseViewModel
    {
        private readonly IDataTransferService _dataTransferService;
        
        private readonly INavigationService _navigationService;

        private DrawInstrument _selectedDrawInstrument;
        
        protected DrawPageViewModel(INavigationService navigationService,
            IDataTransferService dataTransferService) : base(navigationService)
        {
            _navigationService = navigationService;
            _dataTransferService = dataTransferService;
            
            SelectColorCommand = new AsyncCommand<HelpInstrument>(async helpInstrument 
                => await OnSelectColor(helpInstrument), allowsMultipleExecutions: false);
            SelectLineThickness = new AsyncCommand<HelpInstrument>(async helpInstrument 
                => await OnLineThickness(helpInstrument), allowsMultipleExecutions: false);
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            
            DrawInstrumentsList = new ObservableCollection<DrawInstrument>(InstrumentManager.GetDrawInstruments());
            RaisePropertyChanged(nameof(DrawInstrumentsList));
            SelectedDrawInstrument = DrawInstrumentsList.FirstOrDefault();

            var helpInstrumentsList = InstrumentManager.GetHelpInstruments();
            foreach (var helpInstrument in helpInstrumentsList)
            {
                switch (helpInstrument.HelpInstrumentType)
                {
                    case HelpInstrumentType.Color:
                        helpInstrument.ClickCommand = SelectColorCommand;
                        break;
                    case HelpInstrumentType.Thickness:
                        helpInstrument.ClickCommand = SelectLineThickness;
                        break;
                }
            }
            HelpInstrumentsList = new ObservableCollection<HelpInstrument>(helpInstrumentsList);
            RaisePropertyChanged(nameof(HelpInstrumentsList));
        }

        public ObservableCollection<DrawInstrument> DrawInstrumentsList { get; set; }
        
        public ObservableCollection<HelpInstrument> HelpInstrumentsList { get; set; }
        
        public IAsyncCommand<HelpInstrument> SelectColorCommand { get; set; }
        public IAsyncCommand<HelpInstrument> SelectLineThickness { get; set; }
        
        public DrawInstrument SelectedDrawInstrument
        {
            get => _selectedDrawInstrument;
            set
            {
                if (value == null)
                    return;
                
                if (_selectedDrawInstrument == value)
                    return;
                
                if(_selectedDrawInstrument != null)
                    _selectedDrawInstrument.IsSelected = false;
                
                CanvasInstruments.InstrumentType = value.InstrumentType;
                _selectedDrawInstrument = value;
                _selectedDrawInstrument.IsSelected = true;

                RaisePropertyChanged();
            }
        }
        
        private async Task OnSelectColor(HelpInstrument helpInstrument)
        {
            await this.DisplayColorPicker("Select color");
            helpInstrument.IconColor = Color.FromHex(CanvasInstruments.InstrumentColor);
        }
        
        private async Task OnLineThickness(HelpInstrument helpInstrument)
        {
            await this.DisplayLineThickness("Select line thickness");
        }

        public async Task SendPictureToServer(SKData data)
        {
            var bytes = data.ToArray();
            var imageInBase64 = Convert.ToBase64String(bytes);
            var picture = new Picture
            {
                ImageInBase64 = imageInBase64
            };
            await _dataTransferService.SendPictureToUser(picture);
        }
    }
}