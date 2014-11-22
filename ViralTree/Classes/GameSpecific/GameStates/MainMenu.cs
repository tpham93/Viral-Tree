﻿using SFML.Graphics;
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
            backgroundTexture = Game.content.Load<Texture>("gfx/menuscreen.png");

            backgroundSprite = new Sprite(backgroundTexture, new IntRect(0, 0, (int)backgroundTexture.Size.X, (int)backgroundTexture.Size.Y));

            buttonList = new List<SelectButton>();

            buttonList.Add(new SelectButton(" Select Lvl", "", new Vector2f(200, 100), 0));
            buttonList.Add(new SelectButton("     Quit", "", new Vector2f(200, 200), 1));

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
            if (KInput.IsClicked(Keyboard.Key.Escape))
                this.parent.SetGameState(null);

            if (KInput.IsClicked(Keyboard.Key.Right))
                curButton++;

            if (KInput.IsClicked(Keyboard.Key.Left))
                curButton--;

            if (curButton < 0)
                curButton = maxButton;
            else if (curButton > maxButton)
                curButton = 0;



            foreach (SelectButton b in buttonList)
            {
                b.Update(curButton);
            }


            if (KInput.IsClicked(Keyboard.Key.Space))
            {
                if (curButton == 0)
                    parent.SetGameState(new LevelSelection());
                
                else if (curButton == 1)
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
