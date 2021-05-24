using System.Reflection;
using Draws.Helpers.Extensions;
using Draws.Managers;
using Draws.Services;
using Prism;
using Prism.DryIoc;
using Prism.Ioc;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]

namespace Draws
{
    public partial class App : PrismApplication
    {
        public App(IPlatformInitializer initializer = null) : base(initializer) { }

        protected override async void OnInitialized()
        {
            InitializeComponent();
            await StartupManager.Initialize();
        }
        
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            RegisterTypesAndServices(containerRegistry);
            containerRegistry.RegisterForNavigation<NavigationPage>("navigation");
            containerRegistry.AutoRegisterMvvmComponents(typeof(App).GetTypeInfo().Assembly);
        }
        
        protected void RegisterTypesAndServices(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IDataTransferService, DataTransferService>();
        }

        protected override void OnSleep()
        {
            base.OnSleep();

            App.Current.Container.Resolve<IDataTransferService>().DisconnectUser();
        }
    }
}