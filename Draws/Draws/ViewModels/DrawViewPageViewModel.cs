using System;
using System.IO;
using Draws.Models;
using Draws.Services;
using Draws.ViewModels.Bases;
using Prism.Navigation;
using Xamarin.Forms;

namespace Draws.ViewModels
{
    public class DrawViewPageViewModel : BaseViewModel
    {
        private IDataTransferService _dataTransferService;

        private ImageSource _picture;
        
        public DrawViewPageViewModel(INavigationService navigationService,
            IDataTransferService dataTransferService) : base(navigationService)
        {
            _dataTransferService = dataTransferService;
        }
        
        public ImageSource Picture
        {
            get => _picture;
            set
            {
                _picture = value;
                RaisePropertyChanged();
            }
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            _dataTransferService.PictureReceived += OnPictureReceived;
        }

        private void OnPictureReceived(Picture picture)
        {
            Picture = ImageSource.FromStream(
                () => new MemoryStream(Convert.FromBase64String(picture.ImageInBase64)));
        }
    }
}