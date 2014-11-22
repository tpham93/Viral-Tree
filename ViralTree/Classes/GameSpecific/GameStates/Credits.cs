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

        int posX = 50;
        
        public Credits()
        {

        }

        public override void Init(AGameState lastGameState)
        {
            font = new Font( Game.content.Load<Font>("other/arial.ttf"));

            gametitel = new Text();
            //categories = new Text();
            //content = new Text();

            gametitel.Font  = font;
            //categories.Font = font;
            //content.Font    = font;

            gametitel.CharacterSize = 50;
            //categories.CharacterSize = 35;
            //content.CharacterSize = 15;

            gametitel.Position = new Vector2f(posX, 800);
            gametitel.DisplayedString = "Some Game with a Tree in it";


            


        }


        public override void ShutDown()
        {

        }

        public override void Update()
        {
            if (KInput.IsClicked(Keyboard.Key.Escape))
                this.parent.SetGameState(null);

            gametitel.Position = new Vector2f(posX, gametitel.Position.Y - 1);



            
                
        }

        public override void Draw()
        {
            parent.window.Clear();



            parent.window.Draw(gametitel);
            

            
            }
        }
    }


