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
        private string levelName;

        public RenderTexture worldTarget;
   //     public RenderTexture fogTexture;

        private RenderStates states = new RenderStates(new Shader(null, "Content/other/fog.frag"));

        private Texture noiseTexture;

        public InGame(string levelName)
        {
            this.levelName = levelName;


            states.BlendMode = BlendMode.Add;
        }

        public override void Init(AGameState lastGameState)
        {
            noiseTexture = Game.content.Load<Texture>("gfx/noise.png");
            noiseTexture.Smooth = true;
            noiseTexture.Repeated = true;
          
            states.Shader.SetParameter("worldSize", new Vector2f(Settings.WindowSize.X, Settings.WindowSize.Y));
            world = new GameWorld(levelName);
            
            worldTarget = new RenderTexture((uint)Settings.WindowSize.X, (uint)Settings.WindowSize.Y);
      //      fogTexture = new RenderTexture((uint)Settings.WindowSize.X, (uint)Settings.WindowSize.Y);
 
            worldTarget.SetView(parent.window.GetView());
            world.initCam(worldTarget);
         //   worldTarget = new RenderTexture()

        }

        public override void ShutDown()
        {

        }

        public override void Update()
        {
            Joystick.Update();

            if (KInput.IsClicked(Keyboard.Key.Escape))
            {
                parent.SetGameState(null);
                return;
            }

            if (KInput.IsClicked(Keyboard.Key.F1))
                Settings.DrawFog = !Settings.DrawFog;
            

            world.Update(parent.gameTime, worldTarget);

          //  worldTarget.SetView(world.Cam.currentView);

        }

        public override void Draw()
        {
            parent.window.Clear();
            worldTarget.Clear();
        //    fogTexture.Clear();

            world.Draw(parent.gameTime, worldTarget);

            states.Shader.SetParameter("noise", noiseTexture);
            states.Shader.SetParameter("offset", world.Cam.Position);
            states.Shader.SetParameter("scale", (float)world.Cam.currentView.Size.X / (float)Settings.WindowSize.X);
            states.Shader.SetParameter("playerPos", world.GetEntity(0).Collider.Position);

         //   worldTarget.Draw(new Sprite(fogTexture.Texture), states);

            worldTarget.Display();

            if (Settings.DrawFog)
                parent.window.Draw(new Sprite(worldTarget.Texture), states);
            else
                parent.window.Draw(new Sprite(worldTarget.Texture));

        }
    }
}
