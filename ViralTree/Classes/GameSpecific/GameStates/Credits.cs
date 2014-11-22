using SFML.Graphics;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViralTree.Classes.GameSpecific.Components.Drawables;

namespace ViralTree.GameStates
{
    public sealed class Credits : AGameState
    {
        Text gametitel;
        Text categories;
        Text content;

        Font font;

        int posX;
        
        public Credits()
        {

        }

        public override void Init(AGameState lastGameState)
        {
            font = new Font( Game.content.Load<Font>("other/arial.ttf"));

            gametitel.Position = new Vector2f(posX, 1000);

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
}

