using ViralTree.World;
using SFML.Graphics;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViralTree.GameStates
{
    public sealed class InGame : AGameState
    {

        private GameWorld world;


        public InGame()
        {

        }

        public override void Init(AGameState lastGameState)
        {
            int numX = 20;
            int numY = 20;
            int sizeX = 256;
            int sizeY = 256;

            world = new GameWorld(numX, numY, sizeX, sizeY);
            world.initCam(parent.window);
        }

        public override void ShutDown()
        {

        }

        public override void Update()
        {
            if (KInput.IsClicked(Keyboard.Key.Escape))
            {
                parent.SetGameState(null);
                return;
            }

        


            world.Update(parent.gameTime, parent.window);

        }

        public override void Draw()
        {
            parent.window.Clear();

            world.Draw(parent.gameTime, parent.window);

        }
    }
}
