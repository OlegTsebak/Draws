using System;
using System.Threading.Tasks;
using System.Timers;
using Draws.Helpers;
using Draws.Helpers.Attributes;
using Draws.Helpers.Enums;
using Draws.ViewModels;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Draws.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [Page("draw-page")]
    public partial class DrawPage : ContentPage
    {
        private DrawPageViewModel _viewModel;
        
        private SKSurface _surface;
        private SKCanvas _canvas;
        private SKTouchEventArgs _touchEventArgs;

        private SKImage _image;

        private SKData _skData;

        private bool _isClearing;

        private readonly SKPaint _blackDot = new SKPaint 
        {
            Style = SKPaintStyle.Stroke,
            Color = SKColor.Parse(CanvasInstruments.InstrumentColor),
            StrokeWidth = CanvasInstruments.InstrumentsThickness
        };
        
        private readonly SKPaint _eraserDot = new SKPaint 
        {
            Style = SKPaintStyle.Fill,
            Color = SKColor.Parse(CanvasInstruments.CanvasColor),
            StrokeWidth = CanvasInstruments.InstrumentsThickness
        };
        
        public DrawPage()
        {
            InitializeComponent();

            CanvasView.BackgroundColor = Color.FromHex(CanvasInstruments.CanvasColor);
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            
            if (BindingContext is DrawPageViewModel drawPageViewModel)
                _viewModel = drawPageViewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            
            var sendImageTimer = new Timer(1000);
            sendImageTimer.Elapsed += OnsendImageTimerEvent;
            sendImageTimer.AutoReset = true;
            sendImageTimer.Enabled = true;
        }
        
        private void OnsendImageTimerEvent(Object source, ElapsedEventArgs e)
        {
            Task.Run(async () =>
            {
                if (_image == null)
                    return;
                
                _skData = _image.Encode(SKEncodedImageFormat.Png, 50);
                await _viewModel.SendPictureToServer(_skData);
            });
        }

        private async void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
        {
            var info = args.Info;
            _surface = args.Surface;
            _canvas = _surface.Canvas;
            
            if (_isClearing)
            {
                _isClearing = false;
                using (SKPaint paint = new SKPaint())
                {
                    var rect = new SKRect(0, 0, info.Width, info.Height);
                    paint.Color = SKColor.Parse(CanvasInstruments.CanvasColor);
                    _canvas.DrawRect(rect, paint);
                }
                _canvas.Clear();
                
                _image = _surface.Snapshot();
                _skData = _image.Encode(SKEncodedImageFormat.Png, 100);
                await _viewModel.SendPictureToServer(_skData);
                return;
            }

            if (_touchEventArgs == null)
                return;
            
            _blackDot.Color = SKColor.Parse(CanvasInstruments.InstrumentColor);
            _blackDot.StrokeWidth = CanvasInstruments.InstrumentsThickness;
            _eraserDot.StrokeWidth = CanvasInstruments.InstrumentsThickness;

            switch (CanvasInstruments.InstrumentType)
            {
                case DrawInstrumentType.Circle:
                    _canvas.DrawCircle(new SKPoint(_touchEventArgs.Location.X, _touchEventArgs.Location.Y), 
                        CanvasInstruments.InstrumentsThickness / 2, _blackDot);
                    break;
                case DrawInstrumentType.Square:
                    var squareRect = new SKRect();
                    squareRect.Size = new SKSize(CanvasInstruments.InstrumentsThickness, CanvasInstruments.InstrumentsThickness);
                    squareRect.Location = new SKPoint(_touchEventArgs.Location.X - CanvasInstruments.InstrumentsThickness / 2, 
                        _touchEventArgs.Location.Y- CanvasInstruments.InstrumentsThickness / 2);
                    _canvas.DrawRect(squareRect, _blackDot);
                    break;
                case DrawInstrumentType.Eraser:
                    var eraserRect = new SKRect();
                    eraserRect.Size = new SKSize(CanvasInstruments.InstrumentsThickness * 2, CanvasInstruments.InstrumentsThickness * 2);
                    eraserRect.Location = new SKPoint(_touchEventArgs.Location.X - CanvasInstruments.InstrumentsThickness / 2, 
                        _touchEventArgs.Location.Y- CanvasInstruments.InstrumentsThickness / 2);
                    _canvas.DrawRect(eraserRect, _eraserDot);
                    break;
                case DrawInstrumentType.Filling:
                    using (SKPaint paint = new SKPaint())
                    {
                        var rect = new SKRect(0, 0, info.Width, info.Height);
                        paint.Color = SKColor.Parse(CanvasInstruments.InstrumentColor);
                        _canvas.DrawRect(rect, paint);
                    }
                    break;
            }
            
            _image = _surface.Snapshot();
        } 
        
        private void OnTouch(object sender, SKTouchEventArgs e)
        {
            Console.WriteLine(e.ActionType);
            
            switch (e.ActionType)
            {
                case SKTouchAction.Pressed:
                    _touchEventArgs = e;
                    CanvasView.InvalidateSurface();
                    break;
                case SKTouchAction.Moved:
                    _touchEventArgs = e;
                    CanvasView.InvalidateSurface();
                    break;
            }
            e.Handled = true;
        }
        
        private void OnClearButtonClicked(object sender, EventArgs e)
        {
            _isClearing = true;
            CanvasView.InvalidateSurface();
        }
    }
}