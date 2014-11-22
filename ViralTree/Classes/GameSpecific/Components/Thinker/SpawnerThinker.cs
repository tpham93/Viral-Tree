using SFML.Graphics;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViralTree.Tiled;
using ViralTree.World;

namespace ViralTree.Components
{
    public class SpawnerThinker : AThinker
    {
        private int remainingSpawn;
        private double totalCooldown;
        private double currentCooldown;

        private double startTime;

        private FloatRect bounding;

        private EntityAttribs attribs;



        public SpawnerThinker(FloatRect bounding, int totalSpawns, double cooldown, double startTime, EntityAttribs attribs)
        {
            this.bounding = bounding;

            this.remainingSpawn = totalSpawns;

            this.currentCooldown = cooldown;
            this.totalCooldown = cooldown;

            this.startTime = startTime;

            this.attribs = attribs;
        }

        public override void Update(GameTime gameTime, World.GameWorld world)
        {
            Console.WriteLine(this);

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
                        world.QueueRemovingEntity(Owner);
                }
            }
        }

        private void SpawnEntity(World.GameWorld world)
        {
            Vector2f pos = MathUtil.Rand.NextVec2f(bounding.Left, bounding.Left + bounding.Width, bounding.Top, bounding.Top + bounding.Height);
            Entity e = EntityFactory.Create(attribs.type, pos, attribs.collider, MathUtil.ToArray<object>(attribs.additionalAttribs));
            e.LoadContent();
            world.AddEntity(e);
        }


        public override string ToString()
        {
            return "remaining Spawns: " + remainingSpawn + ", currentCD: " + currentCooldown + ", startTime: " + startTime;
        }

    }
}
