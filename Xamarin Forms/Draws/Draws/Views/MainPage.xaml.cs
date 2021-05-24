using Draws.Helpers.Attributes;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Draws.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [Page("main")]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }
    }
}