using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace WarShield
{
    class TowerManager
    {
        public List<Sprite> Towers = new List<Sprite>();
        Random rand = new Random();

        public Vector2 VelocityforBullet(int TowerNumber, List<Sprite> Enemies)
        {
            Vector2 ClosestDistance = new Vector2(100,100);
            for (int i = 0; i < Enemies.Count; i++)
            {

                //wtf code below

                Vector2 DifferenceofCenters = new Vector2();
                DifferenceofCenters = Enemies[i].Center - Towers[TowerNumber].Center;


                if (Math.Sqrt((DifferenceofCenters.X * DifferenceofCenters.X) + (DifferenceofCenters.Y * DifferenceofCenters.Y)) <= Math.Sqrt((ClosestDistance.X * ClosestDistance.X) + (ClosestDistance.Y * ClosestDistance.Y)))
                {
                    ClosestDistance = DifferenceofCenters;
                }
            }
            float PlusorMinus = new float();
            while(true)
            {
                int Random1 = rand.Next(-5, 5);
                int Random2 = rand.Next(-5, 5);
                if (Random1 != 0 && Random2 != 0)
                {
                    PlusorMinus = Random1 * Random2;
                    PlusorMinus /= Math.Abs(PlusorMinus);
                    break;
                }
            }

            float angle = (float)Math.Atan2(ClosestDistance.Y, ClosestDistance.X);// +(MathHelper.PiOver4) * PlusorMinus * (rand.Next(1, 500) / rand.Next(500, 1000));
            Vector2 Velocity = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * 200;

            //end wtf
            
            return Velocity;
        }

        public void CheckifBulletsHitting(ref List<Sprite> Enemies, ref List<Sprite> Bullets)
        {
            List<int> BulletstoDelete = new List<int>();
            for (int i = 0; i < Enemies.Count; i++)
            {
                for (int j = 0; j < Bullets.Count; j++)
                {
                    if (Enemies[i].IsEnemyBoxColliding(Bullets[j].BoundingBoxRect))
                    {
                        Enemies[i].Health -= 20;
                        BulletstoDelete.Add(j);
                    }
                }
            }
            for (int i = 0; i < BulletstoDelete.Count; i++)
            {
                Bullets.RemoveAt(BulletstoDelete[i]);
            }
        }
    }
}
