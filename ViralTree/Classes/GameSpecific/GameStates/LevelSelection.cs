using SFML.Graphics;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViralTree.Classes.GameSpecific.Components.Drawables;
using ViralTree.World;

namespace ViralTree.GameStates
{
    public sealed class LevelSelection : AGameState
    {
        List<SelectButton> buttonList;

        Texture treeTexture;

        Sprite treeSprite;

        int curLevel = 0;
        int maxLevel;

        PlayerInfo info1;
        PlayerInfo info2;

        public LevelSelection(GInput playerOneInput, GInput playerTwoInput, PlayerCharacters playerOneType, PlayerCharacters playerTwoType, PlayerControls p1Controls, PlayerControls p2Controls)
        {
            info1 = new PlayerInfo(p1Controls, playerOneType, playerOneInput);

            info2 = new PlayerInfo(p2Controls, playerTwoType, playerTwoInput);

        }

        public override void Init(AGameState lastGameState)
        {
            treeTexture = Game.content.Load<Texture>("gfx/awesome_tree.png");

            treeSprite = new Sprite(treeTexture, new IntRect(0, 0, (int)treeTexture.Size.X, (int)treeTexture.Size.Y));

            buttonList = new List<SelectButton>();

            buttonList.Add(new SelectButton(" Testlevel", "lv1", new Vector2f(100, 100), 0, ButtonType.Single));
            buttonList.Add(new SelectButton("      Aca", "testLevelAca", new Vector2f(300, 500), 1, ButtonType.Single));

            maxLevel = buttonList.Count - 1;

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
            if (KInput.IsClicked(Keyboard.Key.Escape))
                this.parent.SetGameState(new MainMenu());

            if (KInput.IsClicked(Keyboard.Key.Right))
                curLevel++;

            if (KInput.IsClicked(Keyboard.Key.Left))
                curLevel--;

            if (curLevel < 0)
                curLevel = maxLevel;
            else if (curLevel > maxLevel)
                curLevel = 0;



            foreach (SelectButton b in buttonList)
            {
                b.Update(curLevel);
            }


            if (KInput.IsClicked(Keyboard.Key.Space))
            {
                parent.SetGameState(new InGame(buttonList[curLevel].getLevel(), info1, info2));
            }
                
        }

        public override void Draw()
        {
            

            parent.window.Draw(treeSprite);


            foreach (SelectButton b in buttonList)
            {
                b.Draw(parent.window);
            }
        }
    }
}
