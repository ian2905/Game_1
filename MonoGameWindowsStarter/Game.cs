﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoGameWindowsStarter
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game : Microsoft.Xna.Framework.Game
    {
        static int MAX_ENEMIES = 100;
        static int MAX_PROJECTILES = 500;
        static int PROJECTILE_SIZE = 10;
        static int PROJECTILE_SPEED = 5;
        static int SPAWN_SPEED = 40;
        static int SHOT_RATE = 250;
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

        int enemySpawnRate = 2000;


        KeyboardState oldKeyboardState;
        KeyboardState newKeyboardState;

        double enemyCounter = 0;


        public Game()
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

            player = new Player(this);

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
            player.LoadContent(Content);
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
            if(gameTime.TotalGameTime.TotalMilliseconds - enemyCounter > enemySpawnRate)
            {
                Vector2 spawnPoint = calcEnemySpawn();
                for(int i = 0; i < MAX_ENEMIES; i++)
                {
                    if(enemies[i] == null || enemies[i].hit)
                    {
                        enemies[i] = new Enemy(this, enemySprite, new Vector2(spawnPoint.X, spawnPoint.Y));
                        break;
                    }
                }
                enemyCounter = gameTime.TotalGameTime.TotalMilliseconds;
            }

            //Handle Projectile Creation
            if (!player.hit)
            {
                createProjectiles(gameTime);
            }

            //Update Positions
            if (!player.hit)
            {
                player.Update(gameTime);
                foreach (Enemy e in enemies)
                {
                    if(e != null && !e.hit)
                    {
                        e.Update(player);
                    }
                }
                foreach (Projectile p in projectiles)
                {
                    if(p != null && !p.offScreen)
                    {
                        p.Update(graphics);
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
            
            //Player
            player.Draw(spriteBatch);
            
            //Enemies
            foreach(Enemy e in enemies)
            {
                if(e != null && !e.hit)
                {
                    e.Draw(spriteBatch);
                }
            }
            
            //Projectiles
            foreach(Projectile p in projectiles)
            {
                if(p != null && !p.offScreen)
                {
                    p.Draw(spriteBatch);
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
                    if (projectiles[i] == null || projectiles[i].offScreen)
                    {
                        projectiles[i] = new Projectile(projectileSprite, new BoundingCircle(player.hitBox.X + player.hitBox.Width, player.hitBox.Y + (player.hitBox.Height / 2) - (PROJECTILE_SIZE), PROJECTILE_SIZE), new Vector2(Math.Max(player.velocity.X, 0) + PROJECTILE_SPEED, player.velocity.Y / 2), Shot.Player);
                        break;
                    }
                    else if (i == MAX_PROJECTILES - 1)
                    {
                        projectiles[0] = new Projectile(projectileSprite, new BoundingCircle(player.hitBox.X + player.hitBox.Width, player.hitBox.Y + (player.hitBox.Height / 2) - (PROJECTILE_SIZE), PROJECTILE_SIZE) , new Vector2(Math.Max(player.velocity.X, 0) + PROJECTILE_SPEED, player.velocity.Y / 2), Shot.Player);
                    }
                }
                player.shotTime = gameTime.TotalGameTime.TotalMilliseconds;
            }
            //Shoot Left
            if (newKeyboardState.IsKeyDown(Keys.Left) && !oldKeyboardState.IsKeyDown(Keys.Left) && gameTime.TotalGameTime.TotalMilliseconds - player.shotTime > SHOT_RATE)
            {
                for (int i = 0; i < MAX_PROJECTILES; i++)
                {
                    if (projectiles[i] == null || projectiles[i].offScreen)
                    {
                        projectiles[i] = new Projectile(projectileSprite, new BoundingCircle(player.hitBox.X - 2 * PROJECTILE_SIZE, player.hitBox.Y + (player.hitBox.Height / 2) - (PROJECTILE_SIZE), PROJECTILE_SIZE), new Vector2(Math.Min(player.velocity.X, 0) - PROJECTILE_SPEED, player.velocity.Y / 2), Shot.Player);
                        break;
                    }
                    else if (i == MAX_PROJECTILES - 1)
                    {
                        projectiles[0] = new Projectile(projectileSprite, new BoundingCircle(player.hitBox.X - 2 * PROJECTILE_SIZE, player.hitBox.Y + (player.hitBox.Height / 2) - (PROJECTILE_SIZE), PROJECTILE_SIZE), new Vector2(Math.Min(player.velocity.X, 0) - PROJECTILE_SPEED, player.velocity.Y / 2), Shot.Player);
                    }
                }
                player.shotTime = gameTime.TotalGameTime.TotalMilliseconds;
            }
            //Shoot Up
            if (newKeyboardState.IsKeyDown(Keys.Up) && !oldKeyboardState.IsKeyDown(Keys.Up) && gameTime.TotalGameTime.TotalMilliseconds - player.shotTime > SHOT_RATE)
            {
                for (int i = 0; i < MAX_PROJECTILES; i++)
                {
                    if (projectiles[i] == null || projectiles[i].offScreen)
                    {
                        projectiles[i] = new Projectile(projectileSprite, new BoundingCircle(player.hitBox.X + (player.hitBox.Width / 2) - (PROJECTILE_SIZE), player.hitBox.Y - 2 * PROJECTILE_SIZE, PROJECTILE_SIZE), new Vector2(player.velocity.X / 2, Math.Min(player.velocity.Y, 0) - PROJECTILE_SPEED), Shot.Player);
                        break;
                    }
                    else if (i == MAX_PROJECTILES - 1)
                    {
                        projectiles[0] = new Projectile(projectileSprite, new BoundingCircle(player.hitBox.X + (player.hitBox.Width / 2) - (PROJECTILE_SIZE), player.hitBox.Y - 2 * PROJECTILE_SIZE, PROJECTILE_SIZE), new Vector2(player.velocity.X / 2, Math.Min(player.velocity.Y, 0) - PROJECTILE_SPEED), Shot.Player);
                    }
                }
                player.shotTime = gameTime.TotalGameTime.TotalMilliseconds;
            }
            //Shoot Down
            if (newKeyboardState.IsKeyDown(Keys.Down) && !oldKeyboardState.IsKeyDown(Keys.Down) && gameTime.TotalGameTime.TotalMilliseconds - player.shotTime > SHOT_RATE)
            {
                for (int i = 0; i < MAX_PROJECTILES; i++)
                {
                    if (projectiles[i] == null || projectiles[i].offScreen)
                    {
                        projectiles[i] = new Projectile(projectileSprite, new BoundingCircle(player.hitBox.X + (player.hitBox.Width / 2) - (PROJECTILE_SIZE), player.hitBox.Y + player.hitBox.Height, PROJECTILE_SIZE), new Vector2(player.velocity.X / 2, Math.Max(player.velocity.Y, 0) + PROJECTILE_SPEED), Shot.Player);
                        break;
                    }
                    else if (i == MAX_PROJECTILES - 1)
                    {
                        projectiles[0] = new Projectile(projectileSprite, new BoundingCircle(player.hitBox.X + (player.hitBox.Width / 2) - (PROJECTILE_SIZE), player.hitBox.Y + player.hitBox.Height, PROJECTILE_SIZE), new Vector2(player.velocity.X / 2, Math.Max(player.velocity.Y, 0) + PROJECTILE_SPEED), Shot.Player);
                    }
                }
                player.shotTime = gameTime.TotalGameTime.TotalMilliseconds;
            }
        }

        private void manageCollision()
        {
            foreach(Projectile p in projectiles)
            {
                if(p != null && !p.offScreen)
                {
                    if (p.shotType == Shot.Enemy)
                    {
                        if (p.hitBox.CollidesWith(player.hitBox))
                        {
                            player.Hit();
                        }
                    }
                    else
                    {
                        foreach(Enemy e in enemies)
                        {
                            if (e != null && !e.hit && p.hitBox.CollidesWith(e.hitBox))
                            {
                                e.Hit();
                                p.Delete();
                                enemySpawnRate = Math.Max(enemySpawnRate - SPAWN_SPEED, SHOT_RATE + 100);
                            }
                        }
                    }
                }
            }

            foreach(Enemy e in enemies)
            {
                if (e != null && !e.hit && e.hitBox.CollidesWith(player.hitBox))
                {
                    player.Hit();
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