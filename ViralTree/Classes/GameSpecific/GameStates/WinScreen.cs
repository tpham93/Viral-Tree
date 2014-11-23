using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ViralTree.GameStates;

namespace ViralTree.GameStates
{
    public class WinScreen : AGameState
    {
        Font font;
        Text text;

        public WinScreen()
        {

        }

        public override void Init(AGameState lastGameState)
        {
            font = Game.content.Load<Font>("other/arial.ttf");
            text = new Text("You won!", font);
        }

        public override void ShutDown()
        {
           
        }

        public override void Update()
        {
           
        }

        public override void Draw()
        {
            parent.window.Clear();

            parent.window.Draw(text);
        }
    }
}
