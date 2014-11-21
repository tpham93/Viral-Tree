using SFML.Graphics;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViralTree.Components;
using ViralTree.Objects;

namespace ViralTree.World
{
    public enum EntitiyType
    {
        Player,
        Spawner
    }

    public static class EntityFactory
    {
        public static Entity Create(EntitiyType type, Vector2f position, object[] additionalInfos = null)
        {
            Entity entity = null;
            switch (type)
            {
                case EntitiyType.Player:
                    entity = CreateNewPlayer(new CircleCollider(64), position);
                    break;

                case EntitiyType.Spawner:
                    entity = CreateSpawner((FloatRect)additionalInfos[0], (EntitiyType)additionalInfos[1], (double)additionalInfos[2], (double)additionalInfos[3], (int)additionalInfos[4]);
                    break;

                default:
                    break;
            }

            entity.LoadContent();

            return entity;
        }

        private static Entity CreateSpawner(FloatRect bounding, EntitiyType type, double firstStart, double cooldown, int numSpawns)
        {
            Vector2f[] vertices = { new Vector2f(bounding.Left, bounding.Top), new Vector2f(bounding.Left, bounding.Top + bounding.Height), new Vector2f(bounding.Left + bounding.Width, bounding.Top + bounding.Height), new Vector2f(bounding.Left + bounding.Width, bounding.Top) };
            return new Entity(new ConvexCollider(vertices, true), new Vector2f(bounding.Left, bounding.Top), new SpawnerThinker(bounding, numSpawns, cooldown, firstStart), EmptyResponse.Instance, EmptyActivator.Instance, EmptyActivatable.Instance, null);
        }

        public static Entity CreateNewPlayer(ACollider collider, Vector2f position)
        {
            return new Entity(collider, position, new Components.PlayerThinker(), new Components.BasicPushResponse(true), new Components.BasicActivator(), Components.EmptyActivatable.Instance, new Components.PlayerDrawer());
        }
    }
}
