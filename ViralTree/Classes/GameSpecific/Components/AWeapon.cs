using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ViralTree.World;

namespace ViralTree.Components
{
    abstract class AWeapon
    {
        Entity owner;
        public Entity Owner
        {
            get { return owner; }
            set { owner = value; }
        }

        public abstract void Update(GameTime gameTime, World.GameWorld world);

        public abstract void Attack(World.GameWorld world);
    }
}
