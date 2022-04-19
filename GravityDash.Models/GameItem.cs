using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace GravityDash.Models
{
    public abstract class GameItem
    {
        public abstract Geometry Area { get; }

        public bool IsCollision(GameItem other)
        {
            return Geometry.Combine(this.Area, other.Area,
                GeometryCombineMode.Intersect, null).GetArea() > 0;
        }
    }
}
