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
    public class Enemy
    {
        const int WIDTH = 30; //X += 64
        const int HEIGHT = 48;//Y += 64
        const float SPEEDCAP = 2;
        const int ANIMATION_FRAME_RATE = 124;

        public Game game;
        public Texture2D sprite;
        public Vector2 velocity;
        public BoundingRectangle hitBox;

        public bool hit;

        public TimeSpan animationTimer;
        public int frame;
        public int face;

        public Enemy(Game game, Texture2D sprite, Vector2 spawn)
        {
            this.game = game;
            this.sprite = sprite;
            velocity = new Vector2(0, 0);
            hitBox = new BoundingRectangle(spawn.X, spawn.Y, WIDTH, HEIGHT);
            hit = false;
            animationTimer = new TimeSpan();
            frame = 0;
            face = 0;
        }

        public void Hit()
        {
            hit = true;
        }

        private void TrackPlayer(Player player)
        {
            //Console.WriteLine(player.hitBox.X);

            Vector2 playerDistence = new Vector2((player.hitBox.X + player.hitBox.Width / 2) - (hitBox.X + hitBox.Width / 2), (player.hitBox.Y + player.hitBox.Height / 2) - (hitBox.Y + hitBox.Height / 2));
            velocity.X = SPEEDCAP * (playerDistence.X / (Math.Abs(playerDistence.X) + Math.Abs(playerDistence.Y)));
            velocity.Y = SPEEDCAP * (playerDistence.Y / (Math.Abs(playerDistence.X) + Math.Abs(playerDistence.Y)));
        }

        public void Update(Player player, GameTime gameTime)
        {
            //Player Tracking
            TrackPlayer(player);


            //Enemy Restrictions

            if (hitBox.Y < 0 - hitBox.Height)
            {
                hitBox.Y = 0 - hitBox.Height;
            }
            if (hitBox.Y > game.GraphicsDevice.Viewport.Height)
            {
                hitBox.Y = game.GraphicsDevice.Viewport.Height;
            }
            if (hitBox.X < 0 - hitBox.Width)
            {
                hitBox.X = 0 - hitBox.Width;
            }
            if (hitBox.X > game.GraphicsDevice.Viewport.Width)
            {
                hitBox.X = game.GraphicsDevice.Viewport.Width;
            }

            while (animationTimer.TotalMilliseconds > ANIMATION_FRAME_RATE)
            {
                Console.WriteLine(frame);
                // increase by one frame
                frame++;
                // reduce the timer by one frame duration
                animationTimer -= new TimeSpan(0, 0, 0, 0, ANIMATION_FRAME_RATE);
            }
            frame %= 8;
            if (Math.Abs(velocity.X) > 1 || Math.Abs(velocity.Y) > 1)
            {
                animationTimer += gameTime.ElapsedGameTime;
            }
            if(Math.Abs(velocity.X) > Math.Abs(velocity.Y))
            {
                if(velocity.X >= 0)
                {
                    face = 3;
                }
                else
                {
                    face = 1;
                }
            }
            else
            {
                if (velocity.Y >= 0)
                {
                    face = 2;
                }
                else
                {
                    face = 0;
                }
            }

            //Final Update
            //Console.WriteLine(playerDistence);
            hitBox.X += velocity.X;
            hitBox.Y += velocity.Y;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite, new Vector2(hitBox.X, hitBox.Y), new Rectangle(
                                                        81 + (frame * 64),
                                                        15 + ((int)face * 64),
                                                        WIDTH,
                                                        HEIGHT), Color.White);
            //spriteBatch.Draw(sprite, hitBox, Color.Red);
        }
    }
}
