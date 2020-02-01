using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoGameWindowsStarter
{
    public struct Projectile
    {
        static int SPEED = 20;

        public Texture2D sprite;
        public float X;
        public float Y;
        public float Radius;

        public Vector2 Velocity;
        public BoundingCircle hitBox;

        public bool OffScreen;

        public Vector2 Center
        {
            get => new Vector2(X, Y);
            set
            {
                X = value.X;
                Y = value.Y;
            }
        }

        public Projectile(Texture2D sprite, float x, float y, float radius, Vector2 velocity)
        {
            this.sprite = sprite;
            this.X = x;
            this.Y = y;
            this.Radius = radius;
            this.Velocity = velocity;
            this.OffScreen = false;
            this.hitBox = new BoundingCircle(x, y, radius);
        }

        public void update(GraphicsDeviceManager graphics)
        {
            //Projectile Restrictions

            if (this.Y < 0 - this.Radius)
            {
                OffScreen = true;
            }
            if (this.Y > graphics.PreferredBackBufferHeight + this.Radius)
            {
                OffScreen = true;
            }
            if (this.X < 0 - this.Radius)
            {
                OffScreen = true;
            }
            if (this.X > graphics.PreferredBackBufferWidth + this.Radius)
            {
                OffScreen = true;
            }

            //Physics

            //Final Update

            this.Y += SPEED * (Velocity.Y / (Velocity.X + Velocity.Y));
            this.X += SPEED * (Velocity.X / (Velocity.X + Velocity.Y));
        }
    }
}
