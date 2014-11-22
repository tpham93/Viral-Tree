using SFML.Graphics;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViralTree.Classes.GameSpecific.Components.Drawables
{
    public class SelectButton
    {
        bool selected = false;

        Font font;

        Texture boxTexture;

        Sprite boxSprite;

        Vector2f pos;

        Text text;

        int offset = 5;
        int ID;

        string name;
        string level;

        public SelectButton(string name, string level, Vector2f pos, int ID)
        {
            this.name  = name;
            this.level = level;
            this.pos   = pos;
            this.ID    = ID;
        }

        public void Init()
        {
            boxTexture = Game.content.Load<Texture>("gfx/textbox.png");

            font = new Font( Game.content.Load<Font>("other/arial.ttf"));

            text = new Text();

            text.Font = font;
            text.DisplayedString = name;
            text.Color = Color.White;
            text.Position = new Vector2f(pos.X + offset, pos.Y + offset);

            boxSprite = new Sprite(boxTexture);

            boxSprite.Position = pos;
        }

        public  void Update(int curLevel)
        {
            if (curLevel == ID)
                selected = true;
            else
                selected = false;

            if (!selected)
                boxSprite.Color = new Color(255, 255, 255, 128);
            else
                boxSprite.Color = new Color(255, 255, 255, 255);

        }

        public void Draw(RenderTarget window)
        {
            window.Draw(boxSprite);
            window.Draw(text);  
        }
        public string getLevel()         
        {
            return level;
        }

    }
}
