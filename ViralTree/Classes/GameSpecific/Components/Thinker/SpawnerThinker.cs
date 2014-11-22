using SFML.Graphics;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViralTree.Components
{
    public class SpawnerThinker : AThinker
    {
        private int remainingSpawn;
        private double totalCooldown;
        private double currentCooldown;

        private double startTime;

        private FloatRect bounding;

        public SpawnerThinker(FloatRect bounding, int totalSpawns, double cooldown, double startTime)
        {
            this.bounding = bounding;

            this.remainingSpawn = totalSpawns;

            this.currentCooldown = cooldown;
            this.totalCooldown = cooldown;

            this.startTime = startTime;
        }

        public override void Update(GameTime gameTime, World.GameWorld world)
        {
            if (startTime > 0)
                startTime -= gameTime.ElapsedTime.TotalSeconds;

            else
            {
                currentCooldown -= gameTime.ElapsedTime.TotalSeconds;

                if (currentCooldown <= 0)
                {
                    currentCooldown = totalCooldown;
                    SpawnEntity(world);
                    remainingSpawn--;

                    if (remainingSpawn <= 0)
                        world.RemoveEntity(Owner);
                }
            }
        }

        private void SpawnEntity(World.GameWorld world)
        {
            Vector2f pos = MathUtil.Rand.NextVec2f(bounding.Left, bounding.Left + bounding.Width, bounding.Top, bounding.Top + bounding.Height);
           // world.AddEntity(World.EntityFactory)
        }

        


    }
}
