using System.Threading.Tasks;
using Draws.Helpers;
using Draws.ViewModels.Bases;
using Prism.Navigation;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Essentials;

namespace Draws.ViewModels
{
    public class LoginPageViewModel : BaseViewModel
    {
        private readonly INavigationService _navigationService;
        private string _userName;
        
        public LoginPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            _navigationService = navigationService;
            
            LoginCommand = new AsyncCommand(async () => await OnLogin(), allowsMultipleExecutions: false);
        }
        
        public IAsyncCommand LoginCommand { get; set; }
        
        public string UserName
        {
            get => _userName;
            set
            {
                _userName = value;
                RaisePropertyChanged();
            }
        }

        private async Task OnLogin()
        {
            if (string.IsNullOrEmpty(UserName))
            {
                App.Current.MainPage.DisplayAlert("Error", "User name must not be null", "Ok");
            }
            else
            {
                Preferences.Set(Constants.UserName, UserName);
                await _navigationService.NavigateAsync("draws://app/navigation/main");
            }
        }
    }
}