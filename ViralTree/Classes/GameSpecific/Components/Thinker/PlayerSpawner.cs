using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViralTree.Components;
using ViralTree.World;

namespace ViralTree.Components
{
    public class PlayerSpawner : AThinker
    {
        Entity scoutPlayer;
        Entity tankPlayer;

        double respawnTime = 0.5;

        public PlayerSpawner(Entity scoutPlayer, Entity tankPlayer)
        {
            this.scoutPlayer = scoutPlayer;
            this.tankPlayer = tankPlayer;
        }

        public override void Update(GameTime gameTime, World.GameWorld world)
        {
            respawnTime -= gameTime.ElapsedTime.TotalSeconds;

            if(respawnTime <= 0)
            {
                if (scoutPlayer != null)
                    world.AddEntity(scoutPlayer);

                if (tankPlayer != null)
                    world.AddEntity(tankPlayer);

                this.Owner.CurrentLife = 0;
            }
        }
    }
}
