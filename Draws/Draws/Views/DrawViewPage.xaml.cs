using Draws.Helpers.Attributes;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Draws.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [Page("draw-view-page")]
    public partial class DrawViewPage : ContentPage
    {
        public DrawViewPage()
        {
            InitializeComponent();
        }
    }
}