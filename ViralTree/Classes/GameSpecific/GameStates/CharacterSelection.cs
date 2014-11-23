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


        GInput pad1;
        GInput pad2;

        int curButton = 0;

        Text title;

        Text joinText1;
        Text joinText2;

        Text player1CharacterText;
        Text player2CharacterText;

        int player1CharacterID = 0;
        int player2CharacterID = 0;

        Font font;

        Sprite checkButton1;
        Sprite checkButton2;
        Sprite checkMark;

        Sprite buttonA1;
        Sprite buttonA2;

        Sprite keyW;

        bool player1LoggedIn = false;

        bool keybordUsed = false;

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

            Joystick.Update();

            List<uint> padlist =  GInput.getConnectedGamepads();
            if (padlist.Count > 0)
                pad1 = new GInput(padlist[0]);

            SelectButton startButton = new SelectButton("Start", "", new Vector2f(Settings.WindowSize.X * 0.5f, Settings.WindowSize.Y * 0.5f), 0, ButtonType.Single);
            startButton.Position -= new Vector2f(startButton.GetSize().X * 0.5f, -Settings.WindowSize.Y * 0.33f);
            buttonList.Add(startButton);

            title = new Text("Choose your character!", font);
            title.Position = new Vector2f(Settings.WindowSize.X * 0.5f - title.GetLocalBounds().Width * 0.5f, Settings.WindowSize.Y * 0.125f - title.GetLocalBounds().Height);

            checkButton1 = new Sprite(Game.content.Load<Texture>("gfx/GUI/checkButton.png"));
            checkButton1.Position = new Vector2f(Settings.WindowSize.X * 0.25f - checkButton1.Texture.Size.X * 0.5f, Settings.WindowSize.Y * 0.40f);
            checkButton1.Color = new Color(255, 255, 255, 127);

            checkButton2 = new Sprite(Game.content.Load<Texture>("gfx/GUI/checkButton.png"));
            checkButton2.Position = new Vector2f(Settings.WindowSize.X * 0.75f - checkButton1.Texture.Size.X * 0.5f, Settings.WindowSize.Y * 0.40f);
            checkButton2.Color = new Color(255, 255, 255, 127);

            checkMark = new Sprite(Game.content.Load<Texture>("gfx/GUI/checkMark.png"));
            checkMark.Position = checkButton1.Position;

            joinText1 = new Text("Press    or    to join", font);
            joinText1.Position = new Vector2f(checkButton1.Position.X - joinText1.GetLocalBounds().Width * 0.25f, checkButton1.Position.Y + checkButton1.Texture.Size.Y * 1.1f);

            joinText2 = new Text("Press    to join", font);
            joinText2.Position = new Vector2f(checkButton2.Position.X - joinText2.GetLocalBounds().Width * 0.15f, checkButton2.Position.Y + checkButton2.Texture.Size.Y * 1.1f);


            buttonA1 = new Sprite(Game.content.Load<Texture>("gfx/GUI/AButton.png"));
            buttonA1.Position = new Vector2f(joinText1.Position.X + joinText1.GetLocalBounds().Width * 0.315f, joinText1.Position.Y + joinText1.GetLocalBounds().Height * 0.28f);
            buttonA1.Scale = new Vector2f(0.45f, 0.45f);

            buttonA2 = new Sprite(Game.content.Load<Texture>("gfx/GUI/AButton.png"));
            buttonA2.Position = new Vector2f(joinText2.Position.X + joinText2.GetLocalBounds().Width * 0.41f, joinText2.Position.Y + joinText2.GetLocalBounds().Height * 0.28f);
            buttonA2.Scale = new Vector2f(0.45f, 0.45f);

            keyW = new Sprite(Game.content.Load<Texture>("gfx/GUI/WKey.png"));
            keyW.Position = new Vector2f(joinText1.Position.X + joinText1.GetLocalBounds().Width * 0.555f, joinText1.Position.Y + joinText1.GetLocalBounds().Height * 0.28f);
            keyW.Scale = new Vector2f(0.43f, 0.43f);

            

            scoutChoosenText = new Text("", font);
            tankChoosenText = new Text("", font);

            object[] arr = {null};
            playerScout = EntityFactory.Create(EntityType.Scout, new Vector2f(Settings.WindowSize.X * 0.50f, Settings.WindowSize.Y * 0.5f), new CircleCollider(64), arr);
            playerTank  = EntityFactory.Create(EntityType.Scout, new Vector2f(Settings.WindowSize.X * 0.75f, Settings.WindowSize.Y * 0.5f), new CircleCollider(64), arr);

            player1CharacterText = new Text("", font);
            player1CharacterText.Position = new Vector2f(checkButton1.Position.X + player1CharacterText.GetLocalBounds().Width * 0.3f, checkButton1.Position.Y - checkButton1.Texture.Size.Y * 0.5f);

            scoutChoosenText.Position = playerScout.Collider.Position;
           // tankChoosenText.Position = playerTank.Collider.Position;


            //tankChoosenText.Position = 

            playerScout.Collider.Position = new Vector2f(-1000, 0);
            playerScout.Drawer.Initialize();
        }

        public override void ShutDown()
        {
             
        }

        public override void Update()
        {
            Joystick.Update();
            if (pad1 != null)
                pad1.update();

            if (!player1LoggedIn)
            {
                if (KInput.IsClicked(Keyboard.Key.W))
                {
                    scoutPlayerId = 1;
                    playerScout.Collider.Position = new Vector2f(Settings.WindowSize.X * 0.25f, Settings.WindowSize.Y * 0.51f);
                    playerScout.Drawer.Initialize();
                    player1LoggedIn = true;
                    keybordUsed = true;
                    
                }
                else if (pad1 != null && pad1.isClicked(GInput.EButton.A))
                {                    
                    scoutPlayerId = 1;
                    player1LoggedIn = true;                    
                }
                else if (pad2 != null && pad2.isClicked(GInput.EButton.A))
                {       
                    scoutPlayerId = 1;
                    player1LoggedIn = true;                  
                }

            }

            if (scoutPlayerId == 1)
            {
                playerScout.Collider.Position = new Vector2f(Settings.WindowSize.X * 0.25f, Settings.WindowSize.Y * 0.51f);
                playerScout.Drawer.Initialize();
            }
            else if (scoutPlayerId == 2 && playCoop)
            {
                playerScout.Collider.Position = new Vector2f(Settings.WindowSize.X * 0.75f, Settings.WindowSize.Y * 0.51f);
                playerScout.Drawer.Initialize();
            }
            else
            {
                playerScout.Collider.Position = new Vector2f(-1000f, 0f);
                playerScout.Drawer.Initialize();
            }

            if (KInput.IsClicked(Keyboard.Key.D) || KInput.IsClicked(Keyboard.Key.A))
            {
                if (scoutPlayerId == 1)
                {
                    scoutPlayerId = 0;
                }
                else if (scoutPlayerId == 0)
                {
                    scoutPlayerId = 1;
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

            if (player1LoggedIn == true)
            {
                if (KInput.IsClicked(Keyboard.Key.Space))
                    parent.SetGameState(new LevelSelection());

                else if (pad1 != null && pad1.isClicked(GInput.EButton.Start))
                    parent.SetGameState(new LevelSelection());

            }
            

            if (KInput.IsClicked(Keyboard.Key.Escape))
                parent.SetGameState(new MainMenu());

            if (pad1 != null && pad1.isClicked(GInput.EButton.B))
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
            parent.window.Draw(checkButton2);

            parent.window.Draw(buttonA1);
            parent.window.Draw(buttonA2);
            parent.window.Draw(keyW);

            parent.window.Draw(joinText1);
            parent.window.Draw(joinText2);

            parent.window.Draw(player1CharacterText);

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
