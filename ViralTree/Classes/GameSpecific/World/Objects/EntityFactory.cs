using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViralTree.World
{
    public enum EntitiyType
    {
        Player
    }

    public static class EntityFactory
    {
        public static Entity Create(EntitiyType type, Vector2f position, ContentManager contentManager)
        {
            Entity entity = null;
            switch (type)
            {
                case EntitiyType.Player:
                    entity = CreateNewPlayer(new CircleCollider(64), position);
                    break;
                default:
                    break;
            }

            entity.LoadContent(contentManager);

            return entity;
        }

        public static Entity CreateNewPlayer(ACollider collider, Vector2f position)
        {
            return new Entity(collider, position, new Components.PlayerThinker(), new Components.BasicPushResponse(true), new Components.BasicActivator(), Components.EmptyActivatable.Instance, new Components.PlayerDrawer());
        }
    }
}
