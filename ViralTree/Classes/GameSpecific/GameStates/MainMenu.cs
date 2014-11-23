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
    public sealed class MainMenu : AGameState
    {
        GInput pad;

        List<SelectButton> buttonList;

        Texture backgroundTexture;

        Sprite backgroundSprite;

        int curButton = 0;
        int maxButton;

        public MainMenu()
        {
            
        }

        public override void Init(AGameState lastGameState)
        {
            Joystick.Update();

            List<uint> padlist = GInput.getConnectedGamepads();
            if (padlist.Count > 0)
                pad = new GInput(padlist[0]);

            if (pad != null)
                curButton--;

            backgroundTexture = Game.content.Load<Texture>("gfx/menuscreen.png");

            backgroundSprite = new Sprite(backgroundTexture, new IntRect(0, 0, (int)backgroundTexture.Size.X, (int)backgroundTexture.Size.Y));

            buttonList = new List<SelectButton>();

            buttonList.Add(new SelectButton("   Start",     "", new Vector2f(200, 100), 0, ButtonType.Single));
            buttonList.Add(new SelectButton("   Credits",    "", new Vector2f(200, 200), 1, ButtonType.Single));
       //     buttonList.Add(new SelectButton("   Settings",   "", new Vector2f(200, 300), 2, ButtonType.Single));
            buttonList.Add(new SelectButton("     Quit",     "", new Vector2f(200, 400), 2, ButtonType.Single));

            maxButton = buttonList.Count - 1;

            foreach (SelectButton b in buttonList)
            {
                b.Init();
            }

        }


        public override void ShutDown()
        {
            
        }

        public override void Update()
        {

            Joystick.Update();
            if (pad != null)
                pad.update();

            if (KInput.IsClicked(Keyboard.Key.Escape))
                this.parent.SetGameState(null);

            if (KInput.IsClicked(Keyboard.Key.Down) || KInput.IsClicked(Keyboard.Key.S))
                curButton++;

            if (KInput.IsClicked(Keyboard.Key.Up) || KInput.IsClicked(Keyboard.Key.W))
                curButton--;

            if(pad != null)
            {
                if (pad.isClicked(GInput.EStick.LDown))
                    curButton--;
                else if (pad.isClicked(GInput.EStick.LUp))
                    curButton++;
            }
            

            if (curButton < 0)
                curButton = maxButton;
            else if (curButton > maxButton)
                curButton = 0;



            foreach (SelectButton b in buttonList)
            {
                b.Update(curButton);
            }


            if (KInput.IsClicked(Keyboard.Key.Space) || (pad != null && pad.isReleased(GInput.EButton.A)))
            {
                if (curButton == 0)
                    parent.SetGameState(new CharacterSelection());  

                else if (curButton == 1)
                    this.parent.SetGameState(new Credits());

                else if (curButton == 2)
                    this.parent.SetGameState(null);

            }
                
        }

        public override void Draw()
        {
            

            parent.window.Draw(backgroundSprite);


            foreach (SelectButton b in buttonList)
            {
                b.Draw(parent.window);
            }
        }
    }
}

