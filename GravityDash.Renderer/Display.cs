using GravityDash.Logic;
using System;
using System.Windows;
using System.Windows.Media;


namespace GravityDash.Renderer
{
    public class Display : FrameworkElement
    {
        
        private double areaWidth;
        private double areaHeight;
        IGameModel model;
        ViewPort vp;
        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            //Background
            drawingContext.DrawRectangle(new LinearGradientBrush(Color.FromArgb(255, 249, 194, 199), Color.FromArgb(255, 152, 185, 231), new Point(1,0), new Point(1,1)), null, new Rect(0, 0, areaWidth, areaHeight));
            
            //Level
            DrawLevel(drawingContext);

            

            //player
            drawingContext.DrawRectangle(model.PlayerRepository.ReadPlayer(1).Character, null, new Rect(model.PlayerRepository.ReadPlayer(1).X * vp.Zoom + vp.X - model.PlayerRepository.ReadPlayer(1).Radius, model.PlayerRepository.ReadPlayer(1).Y * vp.Zoom + vp.Y - model.PlayerRepository.ReadPlayer(1).Radius, 32 * vp.Zoom, 32 * vp.Zoom));

        }

        public void Resize(double width, double height)
        {
            areaWidth = width;
            areaHeight = height;
            vp.ResizeViewPort((int)width, (int)height);
            this.InvalidateVisual();
        }
        public void SetupModel(IGameModel model)
        {
            this.model = model;
        }
        public void SetupViewPort(ViewPort vp)
        {
            this.vp = vp;
        }

        private void DrawLevel(DrawingContext drawingContext)
        {
            for (int i = 0; i < model.LevelRepository.level.DrawingLayer.GetLength(1); i++)
            {
                for (int j = 0; j < model.LevelRepository.level.DrawingLayer.GetLength(0); j++)
                {
                    if (model.LevelRepository.level.DrawingLayer[j, i] == 0)
                    {
                        continue;
                    }
                    
                    drawingContext.DrawRectangle((ImageBrush)model.LevelRepository.level.Brushes[model.LevelRepository.level.DrawingLayer[j, i] - 1], 
                        null, 
                        new Rect(i * 32 * vp.Zoom + vp.X, 
                        j * 32 * vp.Zoom + vp.Y, 
                        32 * vp.Zoom, 
                        32 * vp.Zoom));
                }
            }

        }
        
     }
}
