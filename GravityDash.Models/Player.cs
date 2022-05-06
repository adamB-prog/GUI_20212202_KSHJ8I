using GravityDash.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
//using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace GravityDash.Models
{
    public class Player : GameItem, IPosition
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public ImageBrush Character { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public Vector2 Velocity { get; set; }
        public int Radius { get; set; }
        public bool CanJump { get; set; }
        static public List<ImageBrush> Skins = new List<ImageBrush>()
        {
            new ImageBrush(new BitmapImage(new Uri(Path.Combine("Asset", "Player", "player.png"), UriKind.RelativeOrAbsolute))),
            new ImageBrush(new BitmapImage(new Uri(Path.Combine("Asset", "Player", "Biker_run (1).png"), UriKind.RelativeOrAbsolute))),
            new ImageBrush(new BitmapImage(new Uri(Path.Combine("Asset", "Player", "Biker_run (2).png"), UriKind.RelativeOrAbsolute))),
            new ImageBrush(new BitmapImage(new Uri(Path.Combine("Asset", "Player", "Biker_run (3).png"), UriKind.RelativeOrAbsolute))),
            new ImageBrush(new BitmapImage(new Uri(Path.Combine("Asset", "Player", "Biker_run (4).png"), UriKind.RelativeOrAbsolute))),
            new ImageBrush(new BitmapImage(new Uri(Path.Combine("Asset", "Player", "Biker_run (5).png"), UriKind.RelativeOrAbsolute))),
            new ImageBrush(new BitmapImage(new Uri(Path.Combine("Asset", "Player", "Biker_run (6).png"), UriKind.RelativeOrAbsolute))),
            new ImageBrush(new BitmapImage(new Uri(Path.Combine("Asset", "Player", "Biker_run_left (1).png"), UriKind.RelativeOrAbsolute))),
            new ImageBrush(new BitmapImage(new Uri(Path.Combine("Asset", "Player", "Biker_run_left (2).png"), UriKind.RelativeOrAbsolute))),
            new ImageBrush(new BitmapImage(new Uri(Path.Combine("Asset", "Player", "Biker_run_left (3).png"), UriKind.RelativeOrAbsolute))),
            new ImageBrush(new BitmapImage(new Uri(Path.Combine("Asset", "Player", "Biker_run_left (4).png"), UriKind.RelativeOrAbsolute))),
            new ImageBrush(new BitmapImage(new Uri(Path.Combine("Asset", "Player", "Biker_run_left (5).png"), UriKind.RelativeOrAbsolute))),
            new ImageBrush(new BitmapImage(new Uri(Path.Combine("Asset", "Player", "Biker_run_left (6).png"), UriKind.RelativeOrAbsolute))),
        };
        

        public void Collision(GameItem other)
        {
            if(other is Platform platform)
            {
                if (X > platform.X + platform.Width)
                {
                    X = platform.X + platform.Width + Radius;
                    Velocity = new Vector2(0, Velocity.Y);
                    CanJump = true;
                }
                else if (X < platform.X)
                {
                    X = platform.X - Radius;
                    Velocity = new Vector2(0, Velocity.Y);
                    CanJump = true;
                }
                else if (Y < platform.Y)
                {
                    Y = platform.Y - (Radius);
                    Velocity = new Vector2(Velocity.X, 0);
                    if (CanJump == false)
                        CanJump = true;
                }
                else if (Y > platform.Y)
                {
                    Y = platform.Y + platform.Height + Radius;
                    Velocity = new Vector2(Velocity.X, 0);
                }
            }
        }

        public override Geometry Area
        {
            get
            {
                return new EllipseGeometry(new Point(X, Y), Radius, Radius);
            }
        }
    }
}
