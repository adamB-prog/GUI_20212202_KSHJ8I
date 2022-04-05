using System;
using System.Windows;
using System.Windows.Media;

namespace GravityDash.Renderer
{
    public class Display : FrameworkElement
    {

        private double areaWidth;
        private double areaHeight;


        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            
        }

        public void Resize(double width, double height)
        {
            areaWidth = width;
            areaHeight = height;
            this.InvalidateVisual();
        }
    }
}
