using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace MonoGameWindowsStarter
{
    public struct BoundingCircle
    {
        public float X;
        public float Y;
        public float Radius;

        public Vector2 Center {
            get => new Vector2(X, Y);
            set {
                X = value.X;
                Y = value.Y;
            }
        }

        public BoundingCircle(float x, float y, float radius)
        {
            this.X = x;
            this.Y = y;
            this.Radius = radius;
        }

        public bool CollidesWith(BoundingCircle other)
        {
            return Math.Pow((this.Radius + other.Radius), 2) >= Math.Pow((this.Center.Y - other.Center.Y), 2) + Math.Pow((this.Center.X - other.Center.X), 2);
        }

        public bool CollidesWith(BoundingRectangle other)
        {
            float nearestX = Math.Max(other.X, Math.Min(this.Center.X, other.X + other.Width));
            float nearestY = Math.Max(other.Y, Math.Min(this.Center.Y, other.Y + other.Height));
            return Math.Pow((this.Center.X - nearestX), 2) + Math.Pow((this.Center.Y - nearestY), 2) < Math.Pow(this.Radius, 2);
        }


    }
}
