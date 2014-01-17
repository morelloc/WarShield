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
using System.IO;

namespace WarShield
{
    class MovementManager
    {
        public Sprite CheckDirection(Sprite enemy)
        {
            //figuring when to turn based on calculated sides
            Vector2 temp = enemy.Location;
            if (enemy.direction == Sprite.Direction.Up)
            {
                temp = new Vector2(enemy.Location.X, enemy.Location.Y + 64);
            }
            else if (enemy.direction == Sprite.Direction.Left)
            {
                temp = new Vector2(enemy.Location.X + 64, enemy.Location.Y);
            }

            int CurrentBlock = Convert(temp);

            if (enemy.PivotPoints.ContainsKey(CurrentBlock) && CurrentBlock != enemy.HandledBlock)
            {
                if (enemy.PivotPoints[CurrentBlock] == Sprite.Turn.Left)
                    {
                        enemy.Rotation -= MathHelper.PiOver2;
                        if(enemy.direction == Sprite.Direction.Up)
                        {
                            enemy.Velocity = new Vector2(-64, 0);
                            enemy.direction = Sprite.Direction.Left;
                            enemy.Location = Convert(CurrentBlock);
                            
                            //enemy.PivotPoints[blocks] = Sprite.Turn.Null;
                        }
                        else if (enemy.direction == Sprite.Direction.Right)
                        {
                            enemy.Velocity = new Vector2(0, -64);
                            enemy.direction = Sprite.Direction.Up;
                            enemy.Location = Convert(CurrentBlock);
                            //enemy.PivotPoints[blocks] = Sprite.Turn.Null;
                        }
                        else if (enemy.direction == Sprite.Direction.Down)
                        {
                            enemy.Velocity = new Vector2(64, 0);
                            enemy.direction = Sprite.Direction.Right;
                            enemy.Location = Convert(CurrentBlock);
                            //enemy.PivotPoints[blocks] = Sprite.Turn.Null;
                        }
                        else if (enemy.direction == Sprite.Direction.Left)
                        {
                            enemy.Velocity = new Vector2(0, 64);
                            enemy.direction = Sprite.Direction.Down;
                            enemy.Location = Convert(CurrentBlock);
                            //enemy.PivotPoints[blocks] = Sprite.Turn.Null;
                        }
                    }
                else if (enemy.PivotPoints[CurrentBlock] == Sprite.Turn.Right)
                    {
                        enemy.Rotation += MathHelper.PiOver2;
                        if (enemy.direction == Sprite.Direction.Up)
                        {
                            enemy.Velocity = new Vector2(64, 0);
                            enemy.direction = Sprite.Direction.Right;
                            enemy.Location = Convert(CurrentBlock);
                            //enemy.PivotPoints[blocks] = Sprite.Turn.Null;
                        }
                        else if (enemy.direction == Sprite.Direction.Right)
                        {
                            enemy.Velocity = new Vector2(0, 64);
                            enemy.direction = Sprite.Direction.Down;
                            enemy.Location = Convert(CurrentBlock);
                            //enemy.PivotPoints[blocks] = Sprite.Turn.Null;
                        }
                        else if (enemy.direction == Sprite.Direction.Down)
                        {
                            enemy.Velocity = new Vector2(-64, 0);
                            enemy.direction = Sprite.Direction.Left;
                            enemy.Location = Convert(CurrentBlock);
                            //enemy.PivotPoints[blocks] = Sprite.Turn.Null;
                        }
                        else if (enemy.direction == Sprite.Direction.Left)
                        {
                            enemy.Velocity = new Vector2(0, -64);
                            enemy.direction = Sprite.Direction.Up;
                            enemy.Location = Convert(CurrentBlock);
                            //enemy.PivotPoints[blocks] = Sprite.Turn.Null;
                        }
                    }

                enemy.HandledBlock = CurrentBlock;
            }
            return enemy;
        }



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
