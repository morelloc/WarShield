﻿using System;
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
    /*class Sprite
    {
        private Microsoft.Xna.Framework.Vector2 vector2;
        private Microsoft.Xna.Framework.Graphics.Texture2D enemySheet;
        private Microsoft.Xna.Framework.Rectangle rectangle;
        private Microsoft.Xna.Framework.Vector2 vector2_2;

        public Sprite(Microsoft.Xna.Framework.Vector2 vector2, Microsoft.Xna.Framework.Graphics.Texture2D enemySheet, Microsoft.Xna.Framework.Rectangle rectangle, Microsoft.Xna.Framework.Vector2 vector2_2)
        {
            // TODO: Complete member initialization
            this.vector2 = vector2;
            this.enemySheet = enemySheet;
            this.rectangle = rectangle;
            this.vector2_2 = vector2_2;
        }*/
        class Sprite
        {
            public Texture2D Texture;

            protected List<Rectangle> frames = new List<Rectangle>();
            private int frameWidth = 0;
            private int frameHeight = 0;
            private int currentFrame;
            private float frameTime = 0.1f;
            private float timeForCurrentFrame = 0.0f;

            private Color tintColor = Color.White;
            private float rotation = 0.0f;

            public int HandledBlock = -1;

            public int Health = 100;

            public float towerShootInterval = 2f;
            public float towerShootElapsedTime = 0f;

            public int CollisionRadius = 0;
            public int BoundingXPadding = 0;
            public int BoundingYPadding = 0;

            public bool TowerInRange = false;

            public object tag;

            public List<Sprite> bullets = new List<Sprite>();


            protected Vector2 location = Vector2.Zero;
            protected Vector2 velocity = Vector2.Zero;


            public enum Turn
            {
                 Right,
                 Left,
                 Null
            }

            public Dictionary<int, Turn> PivotPoints = new Dictionary<int, Turn>();
        

            public enum Direction
            {
                Up, Down, Left, Right
            }

            public Direction direction = new Direction();
          

            public Sprite(
                Vector2 location,
                Texture2D texture,
                Rectangle initialFrame,
                Vector2 velocity)
            {
                this.location = location;
                Texture = texture;
                this.velocity = velocity;

                frames.Add(initialFrame);
                frameWidth = initialFrame.Width;
                frameHeight = initialFrame.Height;

                LoadPivotPoints();
                tag = null;
                direction = Direction.Up;
            }

            public bool towerCanShoot(GameTime gameTime)
            {
                towerShootElapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (towerShootElapsedTime > towerShootInterval)
                {
                    towerShootElapsedTime = 0;
                    return true;
                }
                else
                    return false;
            }

            public bool DeleteThisEnemy()
            {
                if (Health <= 0)
                    return true;
                else
                    return false;
            }


            public void LoadPivotPoints()
            {
                /*string line;
                using (StreamReader reader = new StreamReader("Movement Points.txt"))
                {
                
                    line = reader.ReadLine();
                }*/
                PivotPoints.Add(238, Turn.Left);
                PivotPoints.Add(236, Turn.Left);
                PivotPoints.Add(276, Turn.Right);
                PivotPoints.Add(261, Turn.Right);
                PivotPoints.Add(221, Turn.Right);
                PivotPoints.Add(234, Turn.Left);
                PivotPoints.Add(154, Turn.Left);
                PivotPoints.Add(141, Turn.Right);
                PivotPoints.Add(101, Turn.Right);
                PivotPoints.Add(116, Turn.Right);
                PivotPoints.Add(156, Turn.Left);
                PivotPoints.Add(158, Turn.Left);
                PivotPoints.Add(38, Turn.Left);
            }

            public Vector2 Location
            {
                get { return location; }
                set { location = value; }
            }

            public Vector2 Velocity
            {
                get { return velocity; }
                set { velocity = value; }
            }

            public Color TintColor
            {
                get { return tintColor; }
                set { tintColor = value; }
            }

            public float Rotation
            {
                get { return rotation; }
                set { rotation = value % MathHelper.TwoPi; }
            }

            public int Frame
            {
                get { return currentFrame; }
                set
                {
                    currentFrame = (int)MathHelper.Clamp(value, 0,
                    frames.Count - 1);
                }
            }

            public float FrameTime
            {
                get { return frameTime; }
                set { frameTime = MathHelper.Max(0, value); }
            }

            public Rectangle Source
            {
                get { return frames[currentFrame]; }
            }

            public Rectangle Destination
            {
                get
                {
                    return new Rectangle(
                        (int)location.X,
                        (int)location.Y,
                        frameWidth,
                        frameHeight);
                }
            }

            public Vector2 Center
            {
                get
                {
                    return location +
                        new Vector2(frameWidth / 2, frameHeight / 2);
                }
            }

            public Rectangle BoundingBoxRect
            {
                get
                {
                    return new Rectangle(
                        (int)location.X + BoundingXPadding,
                        (int)location.Y + BoundingYPadding,
                        frameWidth - (BoundingXPadding * 2),
                        frameHeight - (BoundingYPadding * 2));
                }
            }

            public Rectangle EnemyBoundingBox
            {
                get
                {
                    return new Rectangle(
                        (int)location.X + 20,
                        (int)location.Y + 20,
                        24,
                        24);
                }
            }

            public bool IsBoxColliding(Rectangle OtherBox)
            {
                return BoundingBoxRect.Intersects(OtherBox);
            }

            public bool IsEnemyBoxColliding(Rectangle OtherBox)
            {
                return EnemyBoundingBox.Intersects(OtherBox);
            }

            public bool IsCircleColliding(Vector2 otherCenter, float otherRadius)
            {
                if (Vector2.Distance(Center, otherCenter) <
                    (CollisionRadius + otherRadius))
                    return true;
                else
                    return false;
            }

            public void AddFrame(Rectangle frameRectangle)
            {
                frames.Add(frameRectangle);
            }

            public virtual void Update(GameTime gameTime)
            {
                float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

                timeForCurrentFrame += elapsed;

                if (timeForCurrentFrame >= FrameTime)
                {
                    currentFrame = (currentFrame + 1) % (frames.Count);
                    timeForCurrentFrame = 0.0f;
                }


                location += (velocity * elapsed);
            }

            public virtual void Draw(SpriteBatch spriteBatch)
            {
                spriteBatch.Draw(
                    Texture,
                    Center,
                    Source,
                    tintColor,
                    rotation,
                    new Vector2(frameWidth / 2, frameHeight / 2),
                    1.0f,
                    SpriteEffects.None,
                    0.0f);
            }

        }
    }
