using ViralTree.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViralTree.Components
{
    public abstract class AComponent
    {
        public Entity Owner
        {
            get;
            set;
        }

        public bool IsActive
        {
            get;
            set;
        }

        public abstract void Update(GameTime gameTime, GameWorld world);

        public virtual void Initialize()
        {

        }
    }
}
