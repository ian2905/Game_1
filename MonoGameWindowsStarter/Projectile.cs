using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace MonoGameWindowsStarter
{
    public enum Shot
    {
        Player,
        Enemy
    }
    public class Projectile
    {
        const int SIZE = 5;

        public Texture2D sprite;
        public BoundingCircle hitBox;
        public Rectangle rect;

        public Vector2 velocity;

        public Shot shotType;
        public bool offScreen;

        public Vector2 Center
        {
            get => new Vector2(hitBox.X, hitBox.Y);
        }

        public Projectile(Texture2D sprite, BoundingCircle hitBox, Vector2 velocity, Shot shotType)
        {
            this.sprite = sprite;
            this.hitBox = hitBox;
            this.velocity = velocity;
            this.offScreen = false;
            this.shotType = shotType;
            this.rect = new Rectangle((int)(hitBox.X - hitBox.Radius), (int)(hitBox.Y - hitBox.Radius), SIZE*2, SIZE*2);
        }

        public void Delete()
        {
            offScreen = true;
        }

        public void Update(GraphicsDeviceManager graphics)
        {
            //Projectile Restrictions

            if (hitBox.Y < 0 - hitBox.Radius)
            {
                offScreen = true;
            }
            if (hitBox.Y > graphics.PreferredBackBufferHeight + hitBox.Radius)
            {
                offScreen = true;
            }
            if (hitBox.X < 0 - hitBox.Radius)
            {
                offScreen = true;
            }
            if (hitBox.X > graphics.PreferredBackBufferWidth + hitBox.Radius)
            {
                offScreen = true;
            }

            //Physics

            //Final Update
            hitBox.X += velocity.X;
            hitBox.Y += velocity.Y;
            rect.X += (int)velocity.X;
            rect.Y += (int)velocity.Y;
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

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite, rect, Color.White);
        }
    }
}
