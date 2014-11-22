using SFML.Graphics;
using SFML.Window;
using ViralTree.Template;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using ViralTree.GameStates;

namespace ViralTree
{
    public sealed class Game : AbstractGame, IDisposable
    {
        public static ContentManager content;

        private AGameState gameState;

        
   

        public Game()
        {
            content = new ContentManager();
            
            window.SetVerticalSyncEnabled(false);
            window.SetFramerateLimit(0);
            //init the keyboard with all keys:
            KInput.Init(null);

            

            //init the mouse with this game (needed for relative coords):
            MInput.Init(this);
   
            //create the gamestate to start with:
            gameState = new MainMenu();
            gameState.setParent(this);
            gameState.Init(null);

            
        }



        protected override void Update(GameTime gameTime)
        {
            KInput.Update();

            MInput.Update();

            gameState.Update();

        }

        //TODO: maybe remove and add Renderer
        protected override void Draw(GameTime gameTime, RenderWindow window)
        {
            gameState.Draw();
        }

        public int mouseWheel()
        {
            return this.wheelDelta;
        }

        /// <summary>
        /// Shuts down the old gamestate, then starts up the new gamestate. 
        /// </summary>
        /// <param name="newGameState">The new gamestate, closes the application when passing null.</param>
        public void SetGameState(AGameState newGameState)
        {
            this.gameState.ShutDown();

            if (newGameState == null)
            {
                this.window.Close();
                return;
            }

            else
            {
                AGameState lastGameState = this.gameState;

                this.gameState = newGameState;

                this.gameState.setParent(this);

                this.gameState.Init(lastGameState);
            }
        }


        public void Dispose()
        {
            content.DisposeAll();
        }
    }
}
