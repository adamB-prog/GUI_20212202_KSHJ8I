using GravityDash.Logic;
using System;
using System.Diagnostics;
using System.Linq;
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
            if (model is not null)
            {
                //Background
                drawingContext.DrawRectangle(new LinearGradientBrush(Color.FromArgb(255, 249, 194, 199), Color.FromArgb(255, 152, 185, 231), new Point(1, 0), new Point(1, 1)), null, new Rect(0, 0, areaWidth, areaHeight));

                //Level
                DrawLevel(drawingContext);

                //DrawCannonss
                //TODO: paraméterekbe is vp
                if (model.LevelRepository.ReadCbToShoot() != null)
                {
                    //drawingContext.PushTransform(new RotateTransform(model.LevelRepository.ReadCbToShoot().Angle - 180, model.LevelRepository.ReadCbToShoot().X, model.LevelRepository.ReadCbToShoot().Y));
                    //drawingContext.DrawGeometry(model.LevelRepository.ReadCbToShoot().Character, null, model.LevelRepository.ReadCbToShoot().Area);
                    //drawingContext.Pop();

                    drawingContext.PushTransform(new RotateTransform(model.LevelRepository.ReadCbToShoot().Angle - 180, model.LevelRepository.ReadCbToShoot().X * vp.Zoom + vp.X, model.LevelRepository.ReadCbToShoot().Y * vp.Zoom + vp.Y));
                    drawingContext.DrawEllipse(model.LevelRepository.ReadCbToShoot().Character, null, new Point(model.LevelRepository.ReadCbToShoot().X * vp.Zoom + vp.X, model.LevelRepository.ReadCbToShoot().Y * vp.Zoom + vp.Y), model.LevelRepository.ReadCbToShoot().Radius, model.LevelRepository.ReadCbToShoot().Radius);
                    drawingContext.Pop();

                }
                foreach (var item in model.LevelRepository.level.CannonBalls.Where(cb => !cb.Ignore))
                {
                    //drawingContext.PushTransform(new RotateTransform(item.Angle - 180 + item.DisplayAngle, item.X, item.Y));
                    //drawingContext.DrawGeometry(item.Character, null, item.Area);
                    //drawingContext.Pop();

                    drawingContext.PushTransform(new RotateTransform(item.Angle - 180 + item.DisplayAngle, item.X * vp.Zoom + vp.X, item.Y * vp.Zoom + vp.Y));
                    drawingContext.DrawEllipse(item.Character, null, new Point(item.X * vp.Zoom + vp.X, item.Y * vp.Zoom + vp.Y), model.LevelRepository.ReadCbToShoot().Radius, model.LevelRepository.ReadCbToShoot().Radius);
                    drawingContext.Pop();

                }

                //player
                drawingContext.DrawRectangle(model.PlayerRepository.ReadPlayer(1).Character, null, new Rect(model.PlayerRepository.ReadPlayer(1).X * vp.Zoom + vp.X - model.PlayerRepository.ReadPlayer(1).Radius, model.PlayerRepository.ReadPlayer(1).Y * vp.Zoom + vp.Y - model.PlayerRepository.ReadPlayer(1).Radius, 32 * vp.Zoom, 32 * vp.Zoom));
                //drawingContext.DrawRectangle(model.PlayerRepository.ReadPlayer(1).Character, null, new Rect(model.PlayerRepository.ReadPlayer(1).X - model.PlayerRepository.ReadPlayer(1).Radius, model.PlayerRepository.ReadPlayer(1).Y - model.PlayerRepository.ReadPlayer(1).Radius, 32, 32));


            }

        }

        public void Resize(double width, double height)
        {
            areaWidth = width;
            areaHeight = height;
            if (vp is not null)
            {
                vp.ResizeViewPort((int)width, (int)height);

            }
           
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
                    //drawingContext.DrawRectangle((ImageBrush)model.LevelRepository.level.Brushes[model.LevelRepository.level.DrawingLayer[j, i] - 1],
                    //   null,
                    //   new Rect(i * 32,
                    //   j * 32,
                    //   32,
                    //   32));
                }
            }

        }

    }
}