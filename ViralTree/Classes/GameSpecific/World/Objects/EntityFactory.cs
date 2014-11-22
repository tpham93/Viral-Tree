using SFML.Graphics;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViralTree.Components;
using ViralTree.Tiled;

namespace ViralTree.World
{
    public enum EntityType
    {
        None,
        Player,
        Spawner,
        Collision,
        Fungus,
        Projectile    
    }

    public static class EntityFactory
    {
        public static Entity Create(EntityType type, Vector2f position, ACollider collider, object[] additionalInfos = null)
        {
            Entity entity = null;

            switch (type)
            {
                case EntityType.Player:
                    entity = CreateNewPlayer(collider, position, additionalInfos);
                    break;

                case EntityType.Spawner:
                    entity = CreateSpawner(collider, position, additionalInfos);
                    break;

                case EntityType.Collision:
                    entity = CreateBlocker(collider, position, additionalInfos);
                    break;

                case EntityType.Fungus:
                    entity = CreateFungus(collider, position, additionalInfos);
                    break;

                case EntityType.Projectile:
                    entity = CreateProjectile(collider, position, additionalInfos);
                    break;

                default:
                    break;
            }

            return entity;
        }

        private static Entity CreateFungus(ACollider collider, Vector2f position, object[] additionalInfos)
        {
            return new Entity(collider, position, 100.0f, Fraction.Virus, CollidingFractions.All, EmptyThinker.Instance, new BasicPushResponse(true), EmptyActivatable.Instance , new TextureDrawer("gfx/fungus.png"));
        }

        private static Entity CreateBlocker(ACollider collider, Vector2f position, object[] additionalInfos)
        {
            return new Entity(collider, position, float.PositiveInfinity, Fraction.Neutral, CollidingFractions.All, EmptyThinker.Instance, new BasicPushResponse(false), EmptyActivatable.Instance, null);
        }

        private static Entity CreateProjectile(ACollider collider, Vector2f position, object[] additionalInfos)
        {
            return new Entity(new CircleCollider(16), position, float.PositiveInfinity, (Fraction)additionalInfos[0], (CollidingFractions)additionalInfos[1], new ProjectileThinker((Vector2f)additionalInfos[2], (float)additionalInfos[3]), new ProjectileResponse(10), EmptyActivatable.Instance, new TextureDrawer((String)additionalInfos[4]));
        }

        private static Entity CreateSpawner(ACollider collider, Vector2f pos, object[] additionalInfos)
        {
            Entity e = new Entity(collider, pos, float.PositiveInfinity, Fraction.Neutral, CollidingFractions.None, new SpawnerThinker((FloatRect)additionalInfos[0], (int)additionalInfos[3], (double)additionalInfos[1], (double)additionalInfos[4], (EntityAttribs)additionalInfos[5]), EmptyResponse.Instance, EmptyActivatable.Instance, null);
            e.Drawable = false;
            return e;
        }

        private static Entity CreateNewPlayer(ACollider collider, Vector2f position, object[] additionalObjects)
        {
            float startHealth = 100;
            return new Entity(collider, position, startHealth, Fraction.Cell, CollidingFractions.Virus, new Components.PlayerThinker((GInput)additionalObjects[0]), new Components.BasicPushResponse(true), Components.EmptyActivatable.Instance, new Components.PlayerDrawer());
        }
    }
}
