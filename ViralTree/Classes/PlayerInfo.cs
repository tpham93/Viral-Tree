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
        PlayerControls control;
        EntityType character;

        GInput gInput;

        public PlayerInfo(PlayerControls control, PlayerCharacters character, GInput gInput)
        {
            this.control = control;

            if (character == PlayerCharacters.none)
                this.character = EntityType.None;

            else if (character == PlayerCharacters.Scout)
                this.character = EntityType.Scout;

            else if (character == PlayerCharacters.Tank)
                this.character = EntityType.Tank;

            this.gInput = gInput;
        }

        public Entity GetPlayer()
        {
            ACollider collider = null;

            if(character == EntityType.Tank)
                collider = PolygonFactory.GetEllipse(10, 53, 42);

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
