using System.Threading.Tasks;
using Draws.Helpers;
using Prism.Ioc;
using Prism.Navigation;
using Xamarin.Essentials;

namespace Draws.Managers
{
    public static class StartupManager
    {
        public static async Task Initialize()
        {
            var userName = Preferences.Get(Constants.UserName, "");
            if (!string.IsNullOrEmpty(userName))
            {
                await App.Current.Container.Resolve<INavigationService>().NavigateAsync("draws://app/navigation/main");
            }
            else
            {
                await App.Current.Container.Resolve<INavigationService>().NavigateAsync("draws://app/navigation/login-page");
            }
        }
    }
}