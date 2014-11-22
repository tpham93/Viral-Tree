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
        Text categoryStuffNotWorking;
        Text categoryOtherStuffNotWorking;
        Text categoryStuffThatWorks;
        Text contentStuffNotWorking;
        Text contentOtherStuffNotWorking;
        Text contentStuffThatWorks;

        Font font;

        uint titelSize = 50;
        uint categorySize = 35;
        uint contentSize = 25;

        int smallOffset = 100;
        int bigOffset = 300;

        float time = 0;
        float speed = 0.01f;

        int posX = 50;
        
        public Credits()
        {

        }

        public override void Init(AGameState lastGameState)
        {
            font = new Font( Game.content.Load<Font>("other/arial.ttf"));

            gametitel                           = new Text();
            categoryStuffNotWorking             = new Text();
            categoryOtherStuffNotWorking        = new Text();
            categoryStuffThatWorks              = new Text();
            contentStuffNotWorking              = new Text();
            contentOtherStuffNotWorking         = new Text();
            contentStuffThatWorks               = new Text();

            gametitel.Font                      = font;
            categoryStuffNotWorking.Font        = font;
            categoryOtherStuffNotWorking.Font   = font;
            categoryStuffThatWorks.Font         = font;
            contentStuffNotWorking.Font         = font;
            contentOtherStuffNotWorking.Font    = font;
            contentStuffThatWorks.Font          = font;


            gametitel.CharacterSize                         = titelSize;
            categoryStuffNotWorking.CharacterSize           = categorySize;
            categoryOtherStuffNotWorking.CharacterSize      = categorySize;
            categoryStuffThatWorks.CharacterSize            = categorySize;        
            contentStuffNotWorking.CharacterSize            = contentSize;
            contentOtherStuffNotWorking.CharacterSize       = contentSize;
            contentStuffThatWorks.CharacterSize             = contentSize;

            gametitel.Position = new Vector2f(posX, 650);

            gametitel.DisplayedString                       = "Some Game with a Tree in it";
            
            categoryStuffNotWorking.DisplayedString             = "Stuff that doesn't work";
           
            contentStuffNotWorking.DisplayedString                  = " Tuan";

            categoryOtherStuffNotWorking.DisplayedString        = "Other stuff that doesn't work";

            contentOtherStuffNotWorking.DisplayedString             = "Kai";

            categoryStuffThatWorks.DisplayedString              = "Stuff that works perfectly well";

            contentStuffThatWorks.DisplayedString                   = "Jarek";
            

            


            


        }


        public override void ShutDown()
        {

        }

        public override void Update()
        {
            time -= (float)parent.gameTime.ElapsedTime.TotalSeconds * speed;

            if (KInput.IsClicked(Keyboard.Key.Escape))
                this.parent.SetGameState(new MainMenu());

            gametitel.Position = new Vector2f(posX, gametitel.Position.Y - speed );

            categoryStuffNotWorking.Position            = gametitel.Position + new Vector2f(0, bigOffset);
            contentStuffNotWorking.Position             = categoryStuffNotWorking.Position + new Vector2f(0, smallOffset);

            categoryOtherStuffNotWorking.Position       = contentStuffNotWorking.Position + new Vector2f(0, bigOffset);
            contentOtherStuffNotWorking.Position        = categoryOtherStuffNotWorking.Position + new Vector2f(0, smallOffset);

            categoryStuffThatWorks.Position             = contentOtherStuffNotWorking.Position + new Vector2f(0, bigOffset);
            contentStuffThatWorks.Position              = categoryStuffThatWorks.Position + new Vector2f(0, smallOffset);
            
                
        }

        public override void Draw()
        {
            parent.window.Clear();



            parent.window.Draw(gametitel);
            parent.window.Draw(categoryStuffNotWorking);
            parent.window.Draw(contentStuffNotWorking);
            parent.window.Draw(categoryOtherStuffNotWorking);
            parent.window.Draw(contentOtherStuffNotWorking);
            parent.window.Draw(categoryStuffThatWorks);
            parent.window.Draw(contentStuffThatWorks);
            

            
            }
        }
    }


