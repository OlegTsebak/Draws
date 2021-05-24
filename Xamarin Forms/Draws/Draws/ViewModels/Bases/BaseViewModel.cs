using System.Threading.Tasks;
using Prism.Mvvm;
using Prism.Navigation;

namespace Draws.ViewModels.Bases
{
    public class BaseViewModel : BindableBase, INavigationAware, IInitializeAsync, IInitialize
    {
        private bool _isBusy;
        
        protected BaseViewModel(INavigationService navigationService)
        {
            NavigationService = navigationService;
        }
        
        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                _isBusy = value;
                RaisePropertyChanged();
            }
        }
        
        protected internal INavigationService NavigationService { get; }
        
        public virtual void OnNavigatedFrom(INavigationParameters parameters)
        {
            
        }

        public virtual void OnNavigatedTo(INavigationParameters parameters)
        {
            
        }

        public virtual Task InitializeAsync(INavigationParameters parameters)
        {
            return Task.CompletedTask;
        }

        public virtual void Initialize(INavigationParameters parameters)
        {
            
        }
    }
}