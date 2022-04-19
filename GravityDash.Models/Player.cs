using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace GravityDash.Models
{
    class Player
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public Brush Character { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public Vector2 Velocity { get; set; }
    }
}
