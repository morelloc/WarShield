using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using xTile;
using xTile.Display;
using xTile.Tiles;

namespace WarShield
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {

        public enum GameState
        {
            Paused,

            Running,
            GameOver
        }
        GraphicsDeviceManager graphics;
        public SpriteBatch spriteBatch;
        public Texture2D Spritesheet;
        Map map;
        IDisplayDevice xnaDisplayDevice;
        xTile.Dimensions.Rectangle viewport;
        private static Song menuMusic;
        MovementManager movementManager;
        TowerManager towerManager;
        TimeManager timeManager;

        int TowerBudget;
        GameState gameState;
        bool P_AlreadyPressed;
        bool StartSequence;
        int CastleHealth;
        bool HealthIncrease;

        MouseState MouseState;
        MouseState prevMouseState;

        //Enemy Variables
        Texture2D enemySheet;
        Texture2D towerSheet;
        Texture2D bulletSheet;
        //Sprite enemy;

        List<Sprite> enemy;
        

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 960;
            graphics.ApplyChanges();
            Window.AllowUserResizing = true;

            IsMouseVisible = true;

            Content.RootDirectory = "Content";
        }


        protected override void Initialize()
        {

            xnaDisplayDevice = new xTile.Display.XnaDisplayDevice(Content, GraphicsDevice);
            viewport = new xTile.Dimensions.Rectangle(new xTile.Dimensions.Size(this.Window.ClientBounds.Width, this.Window.ClientBounds.Height));

            movementManager = new MovementManager();
            towerManager = new TowerManager();
            timeManager = new TimeManager();
            enemy = new List<Sprite>();

            gameState = GameState.Paused;
            TowerBudget += 5;
            CastleHealth = 500;
            P_AlreadyPressed = false;
            HealthIncrease = false;
            StartSequence = true;

            

            this.IsMouseVisible = true;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            map = Content.Load<Map>("BasicMap");
            map.LoadTileSheets(xnaDisplayDevice);


            enemySheet = Content.Load<Texture2D>("Enemies");
            towerSheet = Content.Load<Texture2D>("Towers");
            bulletSheet = Content.Load<Texture2D>("Bullet");


            

            /*enemy = new Sprite(Convert(298),
                                 enemySheet,
                                 new Rectangle(0, 0, 64, 64),
                                 new Vector2(0, -64));*/
        }

      
        protected override void UnloadContent()
        {
           
        }


        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            MouseState ms = Mouse.GetState();
            int index = map.TileSheets[0].GetTileIndex(new xTile.Dimensions.Location(ms.X, ms.Y));
            //int index = Convert(new Vector2(ms.X, ms.Y));


            //check all pivot points for all enemies
                //change rotation and velocity if necessary

            Window.Title = "Castle Health: " + CastleHealth + "   Towers You Can Buy: " + TowerBudget;
            if (StartSequence)
            {
                Window.Title += "       Press P to Start the Wave of Enemies, After every wave of 10, you will get 5 more towers to place";
            }
            if (gameState == GameState.GameOver)
            {
                Window.Title = "Game Over";
            }

            KeyboardState kb = Keyboard.GetState();
            if (kb.IsKeyDown(Keys.Right))
            {
                viewport.X += 3;
            }
            if (kb.IsKeyDown(Keys.P) && !P_AlreadyPressed)
            {
                if (gameState == GameState.Running)
                {
                    gameState = GameState.Paused;
                }
                else
                {
                    if (StartSequence)
                    {
                        StartSequence = false;
                    }
                    gameState = GameState.Running;
                }
                P_AlreadyPressed = true;
            }
            if (kb.IsKeyUp(Keys.P))
            {
                P_AlreadyPressed = false;
            }

           
            
            //Add Towers


            if (ms.LeftButton == ButtonState.Pressed && ms.X > 0 && ms.X < this.Window.ClientBounds.Width && ms.Y > 0 && ms.Y < this.Window.ClientBounds.Height)
            {
                Tile tile = map.GetLayer("Level").Tiles[(int)(ms.X / 64), (int)(ms.Y / 64)];

                bool CanSpawnTower = true;
                for (int i = 0; i < towerManager.Towers.Count; i++)
                {
                    if (towerManager.Towers[i].IsBoxColliding(new Rectangle(ms.X, ms.Y, 1, 1)))
                    {
                        CanSpawnTower = false;
                    }
                }

                if (tile.TileIndex != 2 && tile.TileIndex != 4 && tile.TileIndex != 19 && CanSpawnTower && TowerBudget > 0)
                {
                    towerManager.Towers.Add(new Sprite(Convert(Convert(new Vector2(ms.X, ms.Y /*+ 130*/))), towerSheet, new Rectangle(0, 0, 64, 64), new Vector2(0, 0)));
                    TowerBudget--;
                }
            }


            if (gameState == GameState.Running)
            {
                if (timeManager.enemyCanSpawn(gameTime, ref TowerBudget, ref HealthIncrease))
                {
                    enemy.Add(new Sprite(Convert(298),
                                     enemySheet,
                                     new Rectangle(0, 0, 64, 64),
                                     new Vector2(0, -64)));
                }

                List<int> EnemiestoDelete = new List<int>();
                for (int i = 0; i < enemy.Count; i++)
                {
                    enemy[i].Update(gameTime);
                    if (enemy[i].DeleteThisEnemy())
                    {
                        EnemiestoDelete.Add(i);
                    }
                }
                for (int i = 0; i < EnemiestoDelete.Count; i++)
                {
                    enemy.RemoveAt(EnemiestoDelete[i]);
                }

                //enemy.Update(gameTime);
                List<int> EnemiestoRemove = new List<int>();
                for (int i = 0; i < enemy.Count; i++)
                {
                    enemy[i] = movementManager.CheckDirection(enemy[i]);
                    if (enemy[i].Location.X + 64 < 0)
                    {
                        EnemiestoRemove.Add(i);
                        CastleHealth -= enemy[i].Health;
                    }

                }
                for (int i = 0; i < EnemiestoRemove.Count; i++)
                {
                    enemy.RemoveAt(EnemiestoRemove[i]);
                }

                for (int j = 0; j < towerManager.Towers.Count; j++)
                {
                    for (int i = 0; i < enemy.Count; i++)
                    {
                        Vector2 DistanceBetween = enemy[i].Center - towerManager.Towers[j].Center;
                        if (Math.Sqrt((DistanceBetween.X * DistanceBetween.X) + (DistanceBetween.Y * DistanceBetween.Y)) < 143)
                        {
                            towerManager.Towers[j].TowerInRange = true;
                            break;
                        }
                        else
                        {
                            towerManager.Towers[j].TowerInRange = false;
                        }
                    }
                }
                for (int i = 0; i < towerManager.Towers.Count; i++)
                {
                    if (towerManager.Towers[i].towerCanShoot(gameTime) && towerManager.Towers[i].TowerInRange)
                    {
                        towerManager.Towers[i].bullets.Add(new Sprite(towerManager.Towers[i].Center - new Vector2(2, 2), bulletSheet, new Rectangle(0, 0, 6, 6), towerManager.VelocityforBullet(i, enemy)));
                    }
                    for (int j = 0; j < towerManager.Towers[i].bullets.Count; j++)
                    {
                        towerManager.Towers[i].bullets[j].Update(gameTime);
                        towerManager.CheckifBulletsHitting(ref enemy, ref towerManager.Towers[i].bullets);
                    }
                }

                if (CastleHealth <= 0)
                {
                    gameState = GameState.GameOver;
                }

                if (HealthIncrease)
                {
                    for (int i = 0; i < enemy.Count; i++)
                    {
                        enemy[i].Health += 20;
                    }
                    HealthIncrease = false;
                }

            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();

            GraphicsDevice.Clear(Color.CornflowerBlue);

            map.Draw(xnaDisplayDevice, viewport);

            //enemy.Draw(spriteBatch);

            for (int i = 0; i < towerManager.Towers.Count; i++)
            {
                towerManager.Towers[i].Draw(spriteBatch);
                for(int j = 0; j < towerManager.Towers[i].bullets.Count; j++)
                {
                    towerManager.Towers[i].bullets[j].Draw(spriteBatch);
                }
            }


            for (int i = 0; i < enemy.Count; i++)
            {
                enemy[i].Draw(spriteBatch);
            }


            base.Draw(gameTime);
            spriteBatch.End();
        }




        //Block and Coordinate Conversions


        public Vector2 Convert(int block)
        {
            return new Vector2(((block % 20) * 64), ((block / 20) * 64));

        }

        public int Convert(Vector2 coordinate)
        {

            int block = ((int)(coordinate.X / 64) + ((int)(coordinate.Y / 64) * 20));

            return block;
        }



    }
}
