using GravityDash.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace GravityDash.Models
{
    public class CannonBall : GameItem, IPosition
    {
        public Brush Character { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public int Radius { get; set; }
        public Vector2 Velocity { get; set; }
        public int Angle { get; set; }
        public double DisplayAngle { get; set; }
        public bool Ignore { get; set; }
        const float restitution = 0.5f;
        bool canRotate = false;

        public CannonBall(double x, double y, int angle)
        {
            this.X = x;
            this.Y = y;
            this.Angle = angle;
            Radius = 32;
            Ignore = true;
            Character = new ImageBrush(new BitmapImage(new Uri(Path.Combine("Asset", "cb.png"), UriKind.RelativeOrAbsolute)));
        }

        public void SetToDefault(CannonBall cb)
        {
            X = cb.X;
            Y = cb.Y;
            Velocity = cb.Velocity;
            Angle = cb.Angle;
            DisplayAngle = cb.DisplayAngle;
            Ignore = cb.Ignore;
            canRotate = false;
        }

        public void Move()
        {
            X += Velocity.X;
            Y += Velocity.Y;
            Velocity = Vector2.Add(Velocity, new Vector2(0, 0.05f)).Y < 5 ? Vector2.Add(Velocity, new Vector2(0, 0.05f)) : new Vector2(Velocity.X, 5);
            if (canRotate)
            {
                if (Velocity.X < 0)
                {
                    DisplayAngle -= Math.Abs(Velocity.X);
                }
                if (Velocity.X > 0)
                {
                    DisplayAngle += Math.Abs(Velocity.X);
                }
            }
        }

        public void Collision(GameItem other)
        {
            if(other is Platform p)
            {
                if(Y < p.Y)
                {
                    Y = p.Y - Radius;
                    Velocity = new Vector2(Velocity.X, 0);
                    canRotate = true;
                }
            }
            if (other is CannonBall cb)
            {
                Vector2 delta = Vector2.Subtract(new Vector2((float)X, (float)Y), new Vector2((float)cb.X, (float)cb.Y));
                float d = delta.Length();

                Vector2 mtd = Vector2.Multiply(delta, ((Radius + cb.Radius) - d) / d);
                X = X + mtd.X / 2;
                Y = Y + mtd.Y / 2;
                cb.X = cb.X - mtd.X / 2;
                cb.Y = cb.Y - mtd.Y / 2;

                Vector2 v = Vector2.Subtract(Velocity, cb.Velocity);
                float vn = Vector2.Dot(v, Vector2.Normalize(mtd));
                if (vn > 0.0) return;

                float i = (-(1.0f + restitution) * vn) / 2;
                Vector2 impulse = Vector2.Multiply(Vector2.Normalize(mtd), i);

                Velocity = Vector2.Add(Velocity, impulse);
                cb.Velocity = Vector2.Subtract(cb.Velocity, impulse);
            }
            if (other is Player player)
            {
                if (player.Y < Y)
                    player.CanJump = true;
                else
                    player.CanJump = false;

                Vector2 delta = Vector2.Subtract(new Vector2((float)X, (float)Y), new Vector2((float)player.X, (float)player.Y));
                float d = delta.Length();

                Vector2 mtd = Vector2.Multiply(delta, ((Radius + player.Radius) - d) / d);

                float im1 = 1 / 1.2f; //cannon
                float im2 = 1 / 1; //player

                X = X + mtd.X / 2;
                Y = Y + mtd.Y / 2;
                player.X = player.X - mtd.X / 2;
                player.Y = player.Y - mtd.Y / 2;

                Vector2 v = Vector2.Subtract(Velocity, player.Velocity);
                float vn = Vector2.Dot(v, Vector2.Normalize(mtd));
                if (vn > 0.0) return;

                float i = (-(1.0f + restitution) * vn) / (im1 + im2);
                Vector2 impulse = Vector2.Multiply(Vector2.Normalize(mtd), i);

                //Velocity = Vector2.Add(Velocity, Vector2.Multiply(impulse, im1));
                player.Velocity = Vector2.Subtract(player.Velocity, Vector2.Multiply(impulse, im2));

            }
        }

        public bool Outside(Size levelArea)
        {
            return (X < -51 || X > levelArea.Width + 51 || Y < -51 || Y > levelArea.Height + 51);
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
