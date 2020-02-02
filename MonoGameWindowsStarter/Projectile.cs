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
    public enum Shot
    {
        Player,
        Enemy
    }
    public class Projectile
    {


        public Texture2D sprite;
        public float X;
        public float Y;
        public float Radius;
        public Rectangle Rect;

        public Vector2 Velocity;
        public BoundingCircle hitBox;

        public Shot ShotType;
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

        public Projectile(Texture2D sprite, float x, float y, float radius, Vector2 velocity, Shot shotType)
        {
            this.sprite = sprite;
            this.X = x;
            this.Y = y;
            this.Radius = radius;
            this.Velocity = velocity;
            Console.WriteLine(this.Velocity);
            this.OffScreen = false;
            this.ShotType = shotType;
            this.hitBox = new BoundingCircle(x, y, radius);
            this.Rect = new Rectangle((int)x, (int)y, (int)(radius * 2), (int)(radius * 2));
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
            //Console.WriteLine(Rect);
            //Console.WriteLine(Velocity.X);
            //Console.WriteLine(Velocity.Y);
            X += Velocity.X;
            Y += Velocity.Y;
            Rect.X += (int)Velocity.X;
            Rect.Y += (int)Velocity.Y;
            /*
            if (Velocity.X != 0)
            {
                Rect.X += (int)(SPEED * (Velocity.X / (Velocity.X + Velocity.Y)) + Velocity.X);
            }
            else
            {
                Rect.X += (int)Velocity.X;
            }

            if (Velocity.Y != 0)
            {
                Rect.Y += (int)(SPEED * (Velocity.Y / (Velocity.X + Velocity.Y)) + Velocity.Y);
            }
            else
            {
                Rect.Y += (int)Velocity.Y;
            }
            //Console.WriteLine(Rect);
            */
        }
    }
}
