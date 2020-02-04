using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace MonoGameWindowsStarter
{
    public struct BoundingRectangle
    {
        public float X;
        public float Y;
        public float Width;
        public float Height;

        public BoundingRectangle(float x, float y, float width, float height)
        {
            this.X = x;
            this.Y = y;
            this.Width = width;
            this.Height = height;
        }

        public bool CollidesWith(BoundingRectangle other)
        {
            return !(this.X > this.X + other.Width
                  || this.X + this.Width < other.X
                  || this.Y > other.Y + other.Height
                  || this.Y + this.Height < other.Y);
        }

        public bool CollidesWith(BoundingCircle other)
        {
            float nearestX = Math.Max(other.X, Math.Min(other.Center.X, this.X + this.Width));
            float nearestY = Math.Max(other.Y, Math.Min(other.Center.Y, this.Y + this.Height));
            return Math.Pow((other.Center.X - nearestX), 2) + Math.Pow((other.Center.Y - nearestY), 2) < Math.Pow(other.Radius, 2);
        }

        public static implicit operator Rectangle(BoundingRectangle br)
        {
            return new Rectangle(
                (int)br.X,
                (int)br.Y,
                (int)br.Width,
                (int)br.Height);
        }
    }
}
