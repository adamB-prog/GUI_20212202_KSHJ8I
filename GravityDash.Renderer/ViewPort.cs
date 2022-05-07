using GravityDash.Models.Interfaces;
using System;
using System.Diagnostics;
using System.Windows.Input;

namespace GravityDash.Renderer
{
    public class ViewPort
    {
        private int x;
        private int y;
        private int viewportWidth;
        private int viewportHeight;
        private double zoom;
        private IPosition follow;

        public int X { get { return x; } private set { x = value; } }
        public int Y { get { return y; } private set { y = value; } }

        public double Zoom { get { return zoom; } private set { zoom = value; } }

        //public int CenterX { get { return (int)((viewportWidth + X) / 2); } }
        //public int CenterY { get { return (int)((viewportHeight + Y) / 2); } }

        public int ViewPortWidthZoomed { get { return (int)(viewportWidth / zoom); } }
        public int ViewPortHeightZoomed { get { return (int)(viewportHeight / zoom); } }

        public ViewPort(int startX, int startY, int width, int height, IPosition follow)
        {
            x = startX;
            y = startY;
            viewportHeight = height;
            viewportWidth = width;
            zoom = 1;
            this.follow = follow;
        }
        public void ResizeViewPort(int width, int height)
        {
            viewportWidth = width;
            viewportHeight = height;
        }

        public void Follow()
        {
            
            x = Math.Clamp((int)(-follow.X * zoom + viewportWidth / 2), -1000, 0);
            y = Math.Clamp((int)(-follow.Y * zoom + viewportHeight / 2), -1000, 200);
            
        }
    }
}