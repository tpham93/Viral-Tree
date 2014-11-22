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
    public class CharacterSelection : AGameState
    {
        List<SelectButton> buttonList;

        int curButton = 0;

        Text title;

        Text coopText;

        Text scoutDesc;

        Font font;

        Sprite checkButton1;
        Sprite checkButton2;
        Sprite checkMark;

        bool playCoop = false;

        const int START_NUM = 0;
        const int CHECK_BUTTON_NUM = 1;
        const int MAX_BUTTON_NUM = 2;

        Entity playerScout;
        Entity playerTank;

        private int scoutPlayerId = -1;
        private int tankPlayerId = -1;

        private Text scoutChoosenText;
        private Text tankChoosenText;

        public CharacterSelection()
        {

        }

        public override void Init(AGameState lastGameState)
        {
            buttonList = new List<SelectButton>();

            font = Game.content.Load<Font>("other/arial.ttf");

            SelectButton startButton = new SelectButton("Start", "", new Vector2f(Settings.WindowSize.X * 0.5f, Settings.WindowSize.Y * 0.5f), 0, ButtonType.Single);
            startButton.Position -= new Vector2f(startButton.GetSize().X * 0.5f, -Settings.WindowSize.Y * 0.33f);
            buttonList.Add(startButton);
         
            

            checkButton1 = new Sprite(Game.content.Load<Texture>("gfx/GUI/checkButton.png"));
            checkButton1.Position = new Vector2f(Settings.WindowSize.X * 0.5f - checkButton1.Texture.Size.X * 0.5f, Settings.WindowSize.Y * 0.25f);
            checkButton1.Color = new Color(255, 255, 255, 127);

            checkMark = new Sprite(Game.content.Load<Texture>("gfx/GUI/checkMark.png"));
            checkMark.Position = checkButton1.Position;

            coopText = new Text("Coop", font);
            coopText.Position = new Vector2f(checkButton1.Position.X + coopText.GetLocalBounds().Width * 0.5f, checkButton1.Position.Y + checkButton1.Texture.Size.Y);

            title = new Text("Choose your character!", font);
            title.Position = new Vector2f(Settings.WindowSize.X * 0.5f - title.GetLocalBounds().Width * 0.5f, Settings.WindowSize.Y * 0.125f - title.GetLocalBounds().Height);


            scoutChoosenText = new Text("", font);
            tankChoosenText = new Text("", font);

            object[] arr = {null};
            playerScout = EntityFactory.Create(EntityType.Scout, new Vector2f(Settings.WindowSize.X * 0.175f, Settings.WindowSize.Y * 0.5f), new CircleCollider(64), arr);

            scoutDesc = new Text("Scout", font);
            scoutDesc.Position = new Vector2f(playerScout.Collider.Position.X - scoutDesc.GetLocalBounds().Width * 0.5f, playerScout.Collider.Position.Y + playerScout.Collider.Radius);

            scoutChoosenText.Position = playerScout.Collider.Position;
           // tankChoosenText.Position = playerTank.Collider.Position;


            //tankChoosenText.Position = 
        }

        public override void ShutDown()
        {
            
        }

        public override void Update()
        {
            if (KInput.IsClicked(Keyboard.Key.Down) || KInput.IsClicked(Keyboard.Key.S))
                curButton++;

            else if (KInput.IsClicked(Keyboard.Key.Up) || KInput.IsClicked(Keyboard.Key.W))
                curButton--;

            if (KInput.IsClicked(Keyboard.Key.Left) || KInput.IsClicked(Keyboard.Key.A))
            {
                if (scoutPlayerId == -1)
                {
                    scoutPlayerId = 0;
                    scoutChoosenText.DisplayedString = "1";
                }
            }

            else if (KInput.IsClicked(Keyboard.Key.Right) || KInput.IsClicked(Keyboard.Key.D))
            {
                if(tankPlayerId == -1)
                {
                    tankPlayerId = 0;;
                    tankChoosenText.DisplayedString = "1";
                }


            }

      




            if (curButton < 0)
                curButton = MAX_BUTTON_NUM - 1;

            else if (curButton >= MAX_BUTTON_NUM)
                curButton = 0;



            foreach (SelectButton b in buttonList)
            {
                b.Update(curButton);
            }

            ChooseOverCheckButton();


            if (KInput.IsClicked(Keyboard.Key.Space))
            {
                if (curButton == START_NUM)
                    parent.SetGameState(new LevelSelection());

                else if (curButton == CHECK_BUTTON_NUM)
                {
                    playCoop = !playCoop;
                }
                  
            }

            else if (KInput.IsClicked(Keyboard.Key.Escape))
                parent.SetGameState(new MainMenu());

            playerScout.Drawer.Update(parent.gameTime, null);
        }

        public override void Draw()
        {
            parent.window.Clear(Color.Black);

            foreach(SelectButton b in buttonList)
                b.Draw(parent.window);


            parent.window.Draw(title);

            parent.window.Draw(checkButton1);

            parent.window.Draw(coopText);

            parent.window.Draw(scoutDesc);

            parent.window.Draw(scoutChoosenText);

            parent.window.Draw(tankChoosenText);

            if (playCoop)
                parent.window.Draw(checkMark);


            playerScout.Draw(parent.gameTime, parent.window);
        }

        public void ChooseOverCheckButton()
        {
            if (curButton == CHECK_BUTTON_NUM)
            {
                checkButton1.Color = Color.White;
            }
            else
            {
                checkButton1.Color = new Color(255, 255, 255, 127);
            }
        }
    }
}
