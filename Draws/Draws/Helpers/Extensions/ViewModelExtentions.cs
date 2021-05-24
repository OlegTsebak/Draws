using System.Threading.Tasks;
using Draws.Controls.Dialogs;
using Draws.ViewModels.Bases;
using Rg.Plugins.Popup.Services;

namespace Draws.Helpers.Extensions
{
    public static class ViewModelExtensions
    {
        public static async Task<bool> DisplayColorPicker(this BaseViewModel baseViewModel, string title)
        {
            var taskCompletionSource = new TaskCompletionSource<bool>();
            var colorPickerDialog = new ColorPickerDialog
            {
                Title = title, 
                Ntcs = taskCompletionSource, 
            };
            await PopupNavigation.Instance.PushAsync(colorPickerDialog);

            return await taskCompletionSource.Task;
        }
        
        public static async Task<bool> DisplayLineThickness(this BaseViewModel baseViewModel, string title)
        {
            var taskCompletionSource = new TaskCompletionSource<bool>();
            var lineThicknessDialog = new LineThicknessDialog
            {
                Title = title, 
                Ntcs = taskCompletionSource, 
            };
            await PopupNavigation.Instance.PushAsync(lineThicknessDialog);

            return await taskCompletionSource.Task;
        }
    }
}