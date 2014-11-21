using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViralTree.GameStates
{
    public abstract class AGameState
    {
        public Game parent;

        public void setParent(Game parent)
        {
            this.parent = parent;
        }

        public abstract void Init(AGameState lastGameState);

        public abstract void ShutDown();

        public abstract void Update();

        public abstract void Draw();
    }
}
