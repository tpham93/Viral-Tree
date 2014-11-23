using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViralTree.GameStates;
using ViralTree.World;

namespace ViralTree
{
    public class PlayerInfo
    {
        EntityType character;

        GInput gInput;

        public List<String> finishedLevels = new List<string>();

        public PlayerInfo(PlayerControls control, PlayerCharacters character, GInput gInput)
        {
           /*
                Console.WriteLine(character);
                Console.WriteLine(control);

                if (gInput != null)
                    Console.WriteLine(gInput);
                else
                    Console.WriteLine("nicht gesetzt");

                Console.WriteLine();
            */
         

            if (character == PlayerCharacters.none)
                this.character = EntityType.None;

            else if (character == PlayerCharacters.Scout)
                this.character = EntityType.Scout;

            else if (character == PlayerCharacters.Tank)
                this.character = EntityType.Tank;

            this.gInput = gInput;

            if (control == PlayerControls.Keyboard)
                this.gInput = null;
        }

        public Entity GetPlayer()
        {
            ACollider collider = null;

            if(character == EntityType.Tank)
                collider = PolygonFactory.GetElipse(10,55, 46);
            else if(character == EntityType.Scout)
                  collider = new CircleCollider(64);

            object[] objs = {gInput};

            if (collider == null)
                return null;

            //Console.WriteLine(reader.spawnPos);
            else
                return EntityFactory.Create(character, Vec2f.Zero, collider, objs);
        }
    }
}
