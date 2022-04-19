using GravityDash.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace GravityDash.Models
{
    public class Player : GameItem, IPosition
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public Brush Character { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public Vector2 Velocity { get; set; }
        public int Radius { get; set; }
        public bool CanJump { get; set; }

        public override Geometry Area
        {
            get
            {
                return new EllipseGeometry(new Point(X, Y), Radius, Radius);
            }
        }
    }
}
