﻿using SFML.Graphics;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViralTree.Components;

namespace ViralTree.World
{
    public enum EntityType
    {
        Player,
        Spawner,
        Collision,
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
                    entity = CreateNewPlayer(collider, position, (GInput)additionalInfos[0]);
                    break;

                case EntityType.Spawner:
                    entity = CreateSpawner((FloatRect)additionalInfos[0], (EntityType)additionalInfos[1], (double)additionalInfos[2], (double)additionalInfos[3], (int)additionalInfos[4]);
                    break;

                case EntityType.Collision:
                    entity = CreateBlocker(position, collider);
                    break;

                case EntityType.Projectile:
                    entity = CreateProjectile(collider, position, (Vector2f)additionalInfos[0], (float)additionalInfos[1], (String)additionalInfos[2]);
                    break;

                default:
                    break;
            }

            return entity;
        }

        private static Entity CreateBlocker(Vector2f position, ACollider collider)
        {
            return new Entity(collider, position, float.PositiveInfinity, EmptyThinker.Instance, new BasicPushResponse(false), EmptyActivator.Instance, EmptyActivatable.Instance, null);
        }

        private static Entity CreateSpawner(FloatRect bounding, EntityType type, double firstStart, double cooldown, int numSpawns)
        {
            Vector2f[] vertices = { new Vector2f(bounding.Left, bounding.Top), new Vector2f(bounding.Left, bounding.Top + bounding.Height), new Vector2f(bounding.Left + bounding.Width, bounding.Top + bounding.Height), new Vector2f(bounding.Left + bounding.Width, bounding.Top) };
            return new Entity(new ConvexCollider(vertices, true), new Vector2f(bounding.Left, bounding.Top), float.PositiveInfinity, new SpawnerThinker(bounding, numSpawns, cooldown, firstStart), EmptyResponse.Instance, EmptyActivator.Instance, EmptyActivatable.Instance, null);
        }

        private static Entity CreateProjectile(ACollider collider, Vector2f position, Vector2f direction, float speed, String filepath)
        {
            return new Entity(new CircleCollider(16), position, float.PositiveInfinity, new ProjectileThinker(direction, speed), new BasicPushResponse(true), EmptyActivator.Instance, EmptyActivatable.Instance, new TextureDrawer(filepath));
        }

        private static Entity CreateNewPlayer(ACollider collider, Vector2f position, GInput input)
        {
            float startHealth = 100;
            return new Entity(collider, position, startHealth, new Components.PlayerThinker(input), new Components.BasicPushResponse(true), new Components.BasicActivator(), Components.EmptyActivatable.Instance, new Components.PlayerDrawer());
        }
    }
}
