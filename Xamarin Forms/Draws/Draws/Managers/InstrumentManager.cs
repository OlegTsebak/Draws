using System.Collections.Generic;
using Draws.Helpers;
using Draws.Helpers.Enums;
using Draws.Models;
using Xamarin.Forms;

namespace Draws.Managers
{
    public static class InstrumentManager
    {
        public static IEnumerable<DrawInstrument> GetDrawInstruments()
        {
            var instrumentCircle = new DrawInstrument(DrawInstrumentType.Circle, FontAwesomeIcons.Circle,
                Color.Red, Color.White);
            
            var instrumentSquare = new DrawInstrument(DrawInstrumentType.Square, FontAwesomeIcons.SquareFull,
                Color.Red, Color.White);
            
            var instrumentEraser = new DrawInstrument(DrawInstrumentType.Eraser, FontAwesomeIcons.Eraser,
                Color.Red, Color.White);
            
            var instrumentFilling = new DrawInstrument(DrawInstrumentType.Filling, FontAwesomeIcons.FillDrip,
                Color.Red, Color.White);

            return new[] { instrumentCircle, instrumentSquare, instrumentEraser, instrumentFilling };
        }
        
        public static IEnumerable<HelpInstrument> GetHelpInstruments()
        {
            var instrumentColor = new HelpInstrument("Колір", HelpInstrumentType.Color, FontAwesomeIcons.SquareFull,
                Color.FromHex(CanvasInstruments.InstrumentColor));
            
            var instrumentThickness = new HelpInstrument("Товщина", HelpInstrumentType.Thickness, FontAwesomeIcons.GripLines,
                Color.Black);

            return new[] { instrumentColor, instrumentThickness };
        }
    }
}