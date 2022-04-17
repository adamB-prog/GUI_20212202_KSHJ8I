using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace GravityDash.Models
{
    public class Level
    {
        public int[,] DrawingLayer { get; set; }
        public int[,] CollisionLayer { get; set; }

        public Brush[] Brushes { get; set; }

        public int Width { get; set; }
        public int Height { get; set; }
    }
}
