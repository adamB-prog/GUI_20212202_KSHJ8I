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
        public int vmx { get; set; } = 0;
        private int vmy { get; set; } = 0;
        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            //Background
            drawingContext.DrawRectangle(new LinearGradientBrush(Color.FromArgb(255, 249, 194, 199), Color.FromArgb(255, 152, 185, 231), new Point(1,0), new Point(1,1)), null, new Rect(0, 0, areaWidth, areaHeight));
            
            //Level
            DrawLevel(drawingContext);

            

            //player
            drawingContext.DrawRectangle(model.PlayerRepository.ReadPlayer(1).Character, null, new Rect(model.PlayerRepository.ReadPlayer(1).X, model.PlayerRepository.ReadPlayer(1).Y, 64, 64));

        }

        public void Resize(double width, double height)
        {
            areaWidth = width;
            areaHeight = height;
            this.InvalidateVisual();
        }
        public void SetupModel(IGameModel model)
        {
            this.model = model;
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
                    drawingContext.DrawRectangle((ImageBrush)model.LevelRepository.level.Brushes[model.LevelRepository.level.DrawingLayer[j, i] - 1], null, new Rect(i * 32 + vmx, j * 32 + vmy, 32, 32));
                }
            }

        }
        //private Geometry GetLevelGeometry()
        //{
        //    GeometryGroup gg = new GeometryGroup();

        //    for (int i = 0; i < model.LevelRepository.level.DrawingLayer.GetLength(1); i++)
        //    {
        //        for (int j = 0; j < model.LevelRepository.level.DrawingLayer.GetLength(0); j++)
        //        {
        //            if (model.LevelRepository.level.DrawingLayer[j, i] == 0)
        //            {
        //                continue;
        //            }
        //            gg.Children.Add(new GeometryDrawing((SolidColorBrush)model.LevelRepository.level.Brushes[model.LevelRepository.level.DrawingLayer[j, i]], null,);
        //        }
        //    }




        //    return gg;
        //}
    }
}
