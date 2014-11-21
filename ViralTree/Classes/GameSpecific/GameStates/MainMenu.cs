using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViralTree.GameStates
{
    public sealed class MainMenu : AGameState
    {
        public MainMenu()
        {

        }

        public override void Init(AGameState lastGameState)
        {

        }


        public override void ShutDown()
        {

        }

        public override void Update()
        {
            if (KInput.IsClicked(Keyboard.Key.Escape))
                this.parent.SetGameState(null);


        }

        public override void Draw()
        {

        }
    }
}
