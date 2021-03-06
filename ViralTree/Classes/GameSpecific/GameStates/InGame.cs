﻿using ViralTree.World;
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

        PlayerInfo info1;
        PlayerInfo info2;

        public InGame(string levelName, PlayerInfo info1, PlayerInfo info2)
        {
            this.levelName = levelName;

            this.info1 = info1;
            this.info2 = info2;
        }

        public override void Init(AGameState lastGameState)
        {
            noiseTexture = Game.content.Load<Texture>("gfx/noise.png");
            noiseTexture.Smooth = true;
            noiseTexture.Repeated = true;
          
            states.Shader.SetParameter("worldSize", new Vector2f(Settings.WindowSize.X, Settings.WindowSize.Y));
            states.BlendMode = BlendMode.Add;
            worldTarget = new RenderTexture((uint)Settings.WindowSize.X, (uint)Settings.WindowSize.Y);
            worldTarget.SetView(parent.window.GetView());




            world = new GameWorld(levelName, info1.GetPlayer(), info2.GetPlayer(), worldTarget);

       

            
         
      //      fogTexture = new RenderTexture((uint)Settings.WindowSize.X, (uint)Settings.WindowSize.Y);
 


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
                parent.SetGameState(new MainMenu());
                return;
            }

            if (KInput.IsClicked(Keyboard.Key.F1))
                Settings.DrawFog = !Settings.DrawFog;
            

            world.Update(parent.gameTime, worldTarget);

            if (world.playerDied)
            {
                parent.SetGameState(new InGame(this.levelName, info1, info2));
            }

            if (world.finishedLevel)
            {
                info1.finishedLevels.Add(this.levelName); 
                info2.finishedLevels.Add(this.levelName);
                parent.SetGameState(new LevelSelection(info1, info2));
            }

          //  worldTarget.SetView(world.Cam.currentView);

        }

        public override void Draw()
        {
            parent.window.Clear();

            worldTarget.Clear();

            world.Draw(parent.gameTime, worldTarget);


            if (Settings.DrawFog && world.Cam.followingEntities.Count > 0)
            {
                states.Shader.SetParameter("noise", noiseTexture);
                states.Shader.SetParameter("offset", world.Cam.Position);
                states.Shader.SetParameter("scale", (float)world.Cam.currentView.Size.X / (float)Settings.WindowSize.X);
                states.Shader.SetParameter("playerPos", world.Cam.targetPos);
            }


         //   worldTarget.Draw(new Sprite(fogTexture.Texture), states);

            worldTarget.Display();

            if (Settings.DrawFog)
                parent.window.Draw(new Sprite(worldTarget.Texture), states);
            else
                parent.window.Draw(new Sprite(worldTarget.Texture));

        }
    }
}
