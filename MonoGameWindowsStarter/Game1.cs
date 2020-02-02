using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoGameWindowsStarter
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        static int MAX_ENEMIES = 100;
        static int MAX_PROJECTILES = 500;
        static int PROJECTILE_SIZE = 10;
        static int PROJECTILE_SPEED = 5;
        static int SHOT_RATE = 250;
        static int ENEMY_SPAWN_RATE = 2000;
        static int PLAYER_SIZE = 32;


        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Random random = new Random();

        Texture2D projectileSprite;
        Texture2D enemySprite;
        Texture2D dummySprite;

        Player player;
        Enemy[] enemies;
        Projectile[] projectiles;


        KeyboardState oldKeyboardState;
        KeyboardState newKeyboardState;

        double enemyCounter = 0;


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            graphics.PreferredBackBufferWidth = 1042;
            graphics.PreferredBackBufferHeight = 768;
            graphics.ApplyChanges();

            player = new Player(dummySprite, new Rectangle(graphics.PreferredBackBufferWidth / 2,
                                            graphics.PreferredBackBufferHeight / 2, PLAYER_SIZE, PLAYER_SIZE), new Vector2(0, 0));

            projectiles = new Projectile[MAX_PROJECTILES];
            enemies = new Enemy[MAX_ENEMIES];


            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            projectileSprite = Content.Load<Texture2D>("ball");
            enemySprite = Content.Load<Texture2D>("OnePixel");
            player.setSprite(Content.Load<Texture2D>("OnePixel"));
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            newKeyboardState = Keyboard.GetState();


            // Input
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            //Handle Enemy Creation
            if(gameTime.TotalGameTime.TotalMilliseconds - enemyCounter > ENEMY_SPAWN_RATE)
            {
                Vector2 spawnPoint = calcEnemySpawn();
                for(int i = 0; i < MAX_ENEMIES; i++)
                {
                    if(enemies[i] == null || enemies[i].Hit)
                    {
                        enemies[i] = new Enemy(enemySprite, new Rectangle((int)spawnPoint.X, (int)spawnPoint.Y, PLAYER_SIZE, PLAYER_SIZE));
                        break;
                    }
                }
                enemyCounter = gameTime.TotalGameTime.TotalMilliseconds;
            }

            //Handle Projectile Creation
            createProjectiles(gameTime);

            //Update Positions
            if (!player.Hit)
            {
                player.update(newKeyboardState, oldKeyboardState, graphics);
                foreach (Enemy e in enemies)
                {
                    if(e != null)
                    {
                        e.update(player, graphics);
                    }
                }
                foreach (Projectile p in projectiles)
                {
                    if(p != null)
                    {
                        p.update(graphics);
                    }
                }

                manageCollision();
            }



            oldKeyboardState = newKeyboardState;
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            // spriteBatch.Draw(ball, new Rectangle((int)ballPosition.X, (int)ballPosition.Y, 100, 100), Color.White);
            spriteBatch.Draw(player.Sprite, player.Rect, Color.Green);
            foreach(Enemy e in enemies)
            {
                if(e != null && !e.Hit)
                {
                    spriteBatch.Draw(e.Sprite, e.Rect, Color.Red);
                }
            }
            foreach(Projectile p in projectiles)
            {
                if(p != null && !p.OffScreen)
                {
                    spriteBatch.Draw(p.sprite, p.Rect, Color.White);
                }
            }

            // All draws in here
            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void createProjectiles(GameTime gameTime)
        {
            //Shoot Right
            if (newKeyboardState.IsKeyDown(Keys.Right) && !oldKeyboardState.IsKeyDown(Keys.Right) && gameTime.TotalGameTime.TotalMilliseconds - player.shotTime > SHOT_RATE)
            {
                for (int i = 0; i < MAX_PROJECTILES; i++)
                {
                    if (projectiles[i] == null || projectiles[i].OffScreen)
                    {
                        projectiles[i] = new Projectile(projectileSprite, player.Rect.X + player.Rect.Width, player.Rect.Y + (player.Rect.Height / 2) - (PROJECTILE_SIZE), PROJECTILE_SIZE, new Vector2(Math.Max(player.Velocity.X, 0) + PROJECTILE_SPEED, player.Velocity.Y), Shot.Player);
                        break;
                    }
                    else if (i == MAX_PROJECTILES - 1)
                    {
                        projectiles[0] = new Projectile(projectileSprite, player.Rect.X + player.Rect.Width, player.Rect.Y + (player.Rect.Height / 2) - (PROJECTILE_SIZE), PROJECTILE_SIZE, new Vector2(Math.Max(player.Velocity.X, 0) + PROJECTILE_SPEED, player.Velocity.Y), Shot.Player);
                    }
                }
                player.shotTime = gameTime.TotalGameTime.TotalMilliseconds;
            }
            //Shoot Left
            if (newKeyboardState.IsKeyDown(Keys.Left) && !oldKeyboardState.IsKeyDown(Keys.Left) && gameTime.TotalGameTime.TotalMilliseconds - player.shotTime > SHOT_RATE)
            {
                for (int i = 0; i < MAX_PROJECTILES; i++)
                {
                    if (projectiles[i] == null || projectiles[i].OffScreen)
                    {
                        projectiles[i] = new Projectile(projectileSprite, player.Rect.X - 2 * PROJECTILE_SIZE, player.Rect.Y + (player.Rect.Height / 2) - (PROJECTILE_SIZE), PROJECTILE_SIZE, new Vector2(Math.Min(player.Velocity.X, 0) - PROJECTILE_SPEED, player.Velocity.Y), Shot.Player);
                        break;
                    }
                    else if (i == MAX_PROJECTILES - 1)
                    {
                        projectiles[0] = new Projectile(projectileSprite, player.Rect.X - 2 * PROJECTILE_SIZE, player.Rect.Y + (player.Rect.Height / 2) - (PROJECTILE_SIZE), PROJECTILE_SIZE, new Vector2(Math.Min(player.Velocity.X, 0) - PROJECTILE_SPEED, player.Velocity.Y), Shot.Player);
                    }
                }
                player.shotTime = gameTime.TotalGameTime.TotalMilliseconds;
            }
            //Shoot Up
            if (newKeyboardState.IsKeyDown(Keys.Up) && !oldKeyboardState.IsKeyDown(Keys.Up) && gameTime.TotalGameTime.TotalMilliseconds - player.shotTime > SHOT_RATE)
            {
                for (int i = 0; i < MAX_PROJECTILES; i++)
                {
                    if (projectiles[i] == null || projectiles[i].OffScreen)
                    {
                        projectiles[i] = new Projectile(projectileSprite, player.Rect.X + (player.Rect.Width / 2) - (PROJECTILE_SIZE), player.Rect.Y - 2 * PROJECTILE_SIZE, PROJECTILE_SIZE, new Vector2(player.Velocity.X, Math.Min(player.Velocity.Y, 0) - PROJECTILE_SPEED), Shot.Player);
                        break;
                    }
                    else if (i == MAX_PROJECTILES - 1)
                    {
                        projectiles[0] = new Projectile(projectileSprite, player.Rect.X + (player.Rect.Width / 2) - (PROJECTILE_SIZE), player.Rect.Y - 2 * PROJECTILE_SIZE, PROJECTILE_SIZE, new Vector2(player.Velocity.X, Math.Min(player.Velocity.Y, 0) - PROJECTILE_SPEED), Shot.Player);
                    }
                }
                player.shotTime = gameTime.TotalGameTime.TotalMilliseconds;
            }
            //Shoot Down
            if (newKeyboardState.IsKeyDown(Keys.Down) && !oldKeyboardState.IsKeyDown(Keys.Down) && gameTime.TotalGameTime.TotalMilliseconds - player.shotTime > SHOT_RATE)
            {
                for (int i = 0; i < MAX_PROJECTILES; i++)
                {
                    if (projectiles[i] == null || projectiles[i].OffScreen)
                    {
                        projectiles[i] = new Projectile(projectileSprite, player.Rect.X + (player.Rect.Width / 2) - (PROJECTILE_SIZE), player.Rect.Y + player.Rect.Height, PROJECTILE_SIZE, new Vector2(player.Velocity.X, Math.Max(player.Velocity.Y, 0) + PROJECTILE_SPEED), Shot.Player);
                        break;
                    }
                    else if (i == MAX_PROJECTILES - 1)
                    {
                        projectiles[0] = new Projectile(projectileSprite, player.Rect.X + (player.Rect.Width / 2) - (PROJECTILE_SIZE), player.Rect.Y + player.Rect.Height, PROJECTILE_SIZE, new Vector2(player.Velocity.X, Math.Max(player.Velocity.Y, 0) + PROJECTILE_SPEED), Shot.Player);
                    }
                }
                player.shotTime = gameTime.TotalGameTime.TotalMilliseconds;
            }
        }

        private void manageCollision()
        {
            foreach(Projectile p in projectiles)
            {
                if(p != null)
                {
                    if (p.ShotType == Shot.Enemy)
                    {
                        if (p.hitBox.CollidesWith(player.HitBox))
                        {
                            player.Hit = true;
                        }
                    }
                    else
                    {
                        foreach (Enemy e in enemies)
                        {
                            if (e != null && p.hitBox.CollidesWith(e.HitBox))
                            {
                                e.hit();
                            }
                        }
                    }
                }
            }

            foreach(Enemy e in enemies)
            {
                if (e != null && e.HitBox.CollidesWith(player.HitBox))
                {
                    player.hit();
                }
            }
        }

        public Vector2 calcEnemySpawn()
        {
            int side = random.Next(3);
            //Top
            if(side == 0)
            {
                return new Vector2(random.Next(0 - PLAYER_SIZE, graphics.PreferredBackBufferWidth + PLAYER_SIZE*2), 0 - PLAYER_SIZE);
            }
            //Right
            else if (side == 0)
            {
                return new Vector2(graphics.PreferredBackBufferHeight + PLAYER_SIZE, random.Next(0 - PLAYER_SIZE, graphics.PreferredBackBufferHeight + PLAYER_SIZE));
            }
            //Bottom
            else if (side == 0)
            {
                return new Vector2(random.Next(0 - PLAYER_SIZE, graphics.PreferredBackBufferWidth + PLAYER_SIZE * 2), graphics.PreferredBackBufferHeight + PLAYER_SIZE);
            }
            //Left
            else
            {
                return new Vector2(0 - PLAYER_SIZE, random.Next(0 - PLAYER_SIZE, graphics.PreferredBackBufferHeight + PLAYER_SIZE));
            }
        }
    }
}
