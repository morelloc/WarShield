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
        public enum Direction
        {
            Right,
            Left
        }
        public Dictionary<int, Direction> PivotPoints = new Dictionary<int, Direction>();
        
        public MovementManager()
        {
            
        }

        public void LoadPivotPoints()
        {
            /*string line;
            using (StreamReader reader = new StreamReader("Movement Points.txt"))
            {
                
                line = reader.ReadLine();
            }*/
                PivotPoints.Add(238, Direction.Left);
                PivotPoints.Add(236, Direction.Left);
                PivotPoints.Add(276, Direction.Right);
                PivotPoints.Add(261, Direction.Right);
                PivotPoints.Add(221, Direction.Right);
                PivotPoints.Add(234, Direction.Left);
                PivotPoints.Add(154, Direction.Left);
                PivotPoints.Add(141, Direction.Right);
                PivotPoints.Add(101, Direction.Right);
                PivotPoints.Add(116, Direction.Right);
                PivotPoints.Add(156, Direction.Left);
                PivotPoints.Add(158, Direction.Left);
                PivotPoints.Add(38, Direction.Left);
        }
    }
}
