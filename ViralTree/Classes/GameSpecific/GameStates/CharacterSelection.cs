using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViralTree.Classes.GameSpecific.Components.Drawables;

namespace ViralTree.GameStates
{
    public class CharacterSelection : AGameState
    {
        List<SelectButton> buttonList;

        int curButton = 0;

        public CharacterSelection()
        {

        }

        public override void Init(AGameState lastGameState)
        {
            buttonList = new List<SelectButton>();

            SelectButton button = new SelectButton("Start", "", new Vector2f(Settings.WindowSize.X * 0.5f, Settings.WindowSize.Y * 0.5f), 0, ButtonType.Single);
        //    button.Position += new Vector2f(button.GetSize().X * 0.5f, button.GetSize().Y * 0.5f);
            buttonList.Add(button);
          //  buttonList.Add(new SelectButton("Back", "", new Vector2f(0, 100), 1, ButtonType.Single));



        }

        public override void ShutDown()
        {
            
        }

        public override void Update()
        {
            if (KInput.IsClicked(Keyboard.Key.Down) || KInput.IsClicked(Keyboard.Key.S))
                curButton++;

            if (KInput.IsClicked(Keyboard.Key.Up) || KInput.IsClicked(Keyboard.Key.W))
                curButton--;

            if (curButton < 0)
                curButton = buttonList.Count - 1;
            else if (curButton >= buttonList.Count)
                curButton = 0;



            foreach (SelectButton b in buttonList)
            {
                b.Update(curButton);
            }


            if (KInput.IsClicked(Keyboard.Key.Space))
            {
                if (curButton == 0)
                    parent.SetGameState(new LevelSelection());
            }

            else if (KInput.IsClicked(Keyboard.Key.Escape))
                parent.SetGameState(new MainMenu());
        }

        public override void Draw()
        {
            parent.window.Clear();

            foreach(SelectButton b in buttonList)
                b.Draw(parent.window);
        }
    }
}
