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
    class TimeManager
    {
        public float enemySpawnElapsedTime = 0f;
        public float secondelapsedtimer = 0f;
        public float enemySpawnInterval = 2000f;
        public int SpawnCount = 0;





        public bool enemyCanSpawn(GameTime gameTime, ref int CastleBudget, ref bool HealthIncrease)
        {
            enemySpawnElapsedTime += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (enemySpawnElapsedTime > enemySpawnInterval)
            {
                SpawnCount++;
                enemySpawnElapsedTime = 0;
                return true;
            }
            if (SpawnCount  % 20 == 0)
            {
                CastleBudget += 5;
                SpawnCount = 0;
            }
            /*if (SpawnCount == 10)
            {
                secondelapsedtimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

                if (secondelapsedtimer > 10000)
                {
                    \
                }
                return false;
            }*/
            return false;
        }

    }
}
