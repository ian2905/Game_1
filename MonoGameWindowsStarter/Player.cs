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
    public class Player
    {
        static float FRICTION = (float).4;
        static float ACCELERATION = (float).05;
        static int SPEEDCAP = 40;
        static int SIZE = 32;
        public Game game;

        public Texture2D sprite;
        public Vector2 velocity;
        public BoundingRectangle hitBox;
        public bool hit;
        public double shotTime;

        public Player(Game game)
        {
            this.game = game;
            this.velocity = new Vector2(0, 0);
            this.hitBox = new BoundingRectangle(((game.GraphicsDevice.Viewport.Height / 2) - (SIZE / 2)), 
                                                ((game.GraphicsDevice.Viewport.Width / 2) - (SIZE / 2)), 
                                                SIZE, 
                                                SIZE);
            this.hit = false;
            this.shotTime = 0;
        }

        public void LoadContent(ContentManager content)
        {
            sprite = content.Load<Texture2D>("OnePixel");
        }

        public void Hit()
        {
            hit = true;
        }

        public void Update(GameTime gameTime)
        {
            var keyboardState = Keyboard.GetState();

            //Keyboard Input
            if (velocity.Y > -1*SPEEDCAP) 
            {
                if (keyboardState.IsKeyDown(Keys.W))
                {
                    velocity.Y -= ACCELERATION * (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                }
            }
            if (velocity.Y < SPEEDCAP)
            {
                if (keyboardState.IsKeyDown(Keys.S))
                {
                    velocity.Y += ACCELERATION * (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                }
            }
            if (velocity.X > -1*SPEEDCAP)
            {
                if (keyboardState.IsKeyDown(Keys.A))
                {
                    velocity.X -= ACCELERATION * (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                }
            }
            if (velocity.X < SPEEDCAP)
            {
                if (keyboardState.IsKeyDown(Keys.D))
                {
                    velocity.X += ACCELERATION * (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                }
            }
            

            //Player Restrictions

            if (hitBox.Y < 0)
            {
                velocity.Y = 0;
                hitBox.Y = 0;
            }
            if (hitBox.Y > game.GraphicsDevice.Viewport.Height - hitBox.Height)
            {
                velocity.Y = 0;
                hitBox.Y = game.GraphicsDevice.Viewport.Height - hitBox.Height;
            }
            if (hitBox.X < 0)
            {
                velocity.X = 0;
                hitBox.X = 0;
            }
            if (hitBox.X > game.GraphicsDevice.Viewport.Width - hitBox.Width)
            {
                velocity.X = 0;
                hitBox.X = game.GraphicsDevice.Viewport.Width - hitBox.Width;
            }

            //Physics
            if (velocity.X > 0)
            {
                velocity.X -= FRICTION;
            }
            if (velocity.Y > 0)
            {
                velocity.Y -= FRICTION;
            }
            if (velocity.X < 0)
            {
                velocity.X += FRICTION;
            }
            if (velocity.Y < 0)
            {
                velocity.Y += FRICTION;
            }

            //Final Update
            hitBox.Y += (int)velocity.Y;
            hitBox.X += (int)velocity.X;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (!hit)
            {
                spriteBatch.Draw(sprite, hitBox, Color.Green);
            }
            else
            {
                spriteBatch.Draw(sprite, hitBox, Color.DarkRed);
            }
        }
    }
}
