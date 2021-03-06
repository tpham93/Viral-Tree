﻿using SFML.Graphics;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViralTree.Components;
using ViralTree.Drawer;
using ViralTree.Tiled;
using ViralTree.Weapons;

namespace ViralTree.World
{
    public enum EntityType
    {
        None,
        Scout,
        Tank,
        Spawner,
        Collision,
        Fungus,
        Veinball,
        Anorism,
        Projectile,
        Cloud,
        Melee,
        Key,
        LeavePoint,
        Health,
    }

    public static class EntityFactory
    {
        public static Entity Create(EntityType type, Vector2f position, ACollider collider, object[] additionalInfos = null)
        {
            Entity entity = null;

            collider.Position = position;

            switch (type)
            {
                case EntityType.LeavePoint:
                    entity = CreateExit(collider, position, additionalInfos);
                    break;

                case EntityType.Key:
                    entity = CreateKey(collider, position, additionalInfos);
                    break;

                case EntityType.Scout:
                    entity = CreateNewScout(collider, position, additionalInfos);
                    break;

                case EntityType.Tank:
                    entity = CreateNewTank(collider, position, additionalInfos);
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

                case EntityType.Veinball:
                    entity = CreateVeinball(collider, position, additionalInfos);
                    break;

                case EntityType.Anorism:
                    entity = CreateAnorism(collider, position, additionalInfos);
                    break;

                case EntityType.Projectile:
                    entity = CreateProjectile(collider, position, additionalInfos);
                    break;

                case EntityType.Cloud:
                    entity = CreateCloud(collider, position, additionalInfos);
                    break;

                case EntityType.Melee:
                    entity = CreateMelee(collider, position, additionalInfos);
                    break;

                case EntityType.Health:
                    entity = CreateHealth(collider, position, additionalInfos);
                    break;

                default:
                    break;
            }
             
            return entity;
        }
        private static Entity CreateHealth(ACollider collider, Vector2f position, object[] additionalInfos)
        {
            if (additionalInfos[0] != null)
            {
                Entity result = new Entity(collider, position, float.PositiveInfinity, Fraction.CellProjectile, CollidingFractions.VirusProjectile, EmptyThinker.Instance, new CollectibleResponse(EntityType.Health, (float)additionalInfos[0]), EmptyActivatable.Instance, new TextureDrawer("gfx/Collectibles/healthpack.png"));
                return result;
            }
            else
            {
                Entity result2 = new Entity(collider, position, float.PositiveInfinity, Fraction.CellProjectile, CollidingFractions.VirusProjectile, EmptyThinker.Instance, new CollectibleResponse(EntityType.Health, 100.0f), EmptyActivatable.Instance, new TextureDrawer("gfx/Collectibles/healthpack.png"));
                return result2;
            }

            
        }

        private static Entity CreateExit(ACollider collider, Vector2f position, object[] additionalInfos)
        {
            Entity result = new Entity(collider, position, float.PositiveInfinity, Fraction.Neutral, CollidingFractions.None, EmptyThinker.Instance, new ExitResponse((int)additionalInfos[0], null, null), EmptyActivatable.Instance, new TextureDrawer("gfx/exit.png"));
            return result;
        }

        private static Entity CreateKey(ACollider collider, Vector2f position, object[] additionalInfos)
        {
            Entity result = new Entity(collider, position, 1.0f, Fraction.Neutral, CollidingFractions.None, EmptyThinker.Instance, new KeyResponse((ExitResponse)additionalInfos[0], (double)additionalInfos[1]), EmptyActivatable.Instance, new TextureDrawer("gfx/key.png"));
            return result;
        }

        private static Entity CreateMelee(ACollider collider, Vector2f position, object[] additionalInfos)
        {
            Entity result = new Entity(collider, position, float.PositiveInfinity, (Fraction)additionalInfos[1], (CollidingFractions)additionalInfos[2], new PierceThinker((Entity)additionalInfos[0], (double)additionalInfos[3], (float)additionalInfos[4], (float)additionalInfos[5]), new ProjectileResponse((float)additionalInfos[6]), EmptyActivatable.Instance, new TextureDrawer("gfx/Projectiles/tankAttack.png"));
            return result;
        }

        private static Entity CreateAnorism(ACollider collider, Vector2f position, object[] additionalInfos)
        {
            collider.Scale = GameplayConstants.ANORISM_SCALE;
            return new Entity(collider, position, GameplayConstants.ANORISM_LIFE, Fraction.Virus, CollidingFractions.Cell, new Follower(GameplayConstants.ANORISM_FOLLOW_RADIUS, GameplayConstants.ANORISM_SPEED), new TouchDamageResponse(Fraction.Virus, GameplayConstants.ANORISM_TOUCH_DAMAGE, true, true), EmptyActivatable.Instance, new MultiTextureDrawer("gfx/Enemies/anorism.png", "gfx/Enemies/anorismShuriken.png"));
        }

        private static Entity CreateFungus(ACollider collider, Vector2f position, object[] additionalInfos)
        {
            collider.Scale = GameplayConstants.FUNGUS_SCALE;
            ACollider cloudCollider = new CircleCollider(GameplayConstants.FUNGUS_AOE_MIN);
            AWeapon weapon = new AoeWeapon(TimeSpan.FromMilliseconds(GameplayConstants.FUNGUS_AOE_FREQ), cloudCollider, float.PositiveInfinity, GameplayConstants.FUNGUS_AOE_DAMAGE, TimeSpan.FromMilliseconds(GameplayConstants.FUNGUS_AOE_DURATION), GameplayConstants.FUNGUS_AOE_MIN, GameplayConstants.FUNGUS_AOE_MAX);
            AThinker thinker = new Follower(GameplayConstants.FUNGUS_CHASE_RADIUS, GameplayConstants.FUNGUS_SPEED, weapon, GameplayConstants.FUNGUS_AOE_ATTACK_RADIUS);
            ACollisionResponse response = new BasicPushResponse(true);
            return new Entity(collider, position, GameplayConstants.FUNGUS_LIFE, Fraction.Virus, CollidingFractions.Cell, thinker, response, EmptyActivatable.Instance, new TextureDrawer("gfx/Enemies/fungus.png"));
        }

        private static Entity CreateVeinball(ACollider collider, Vector2f position, object[] additionalInfos)
        {
            collider.Scale = GameplayConstants.VEINBALL_SCALE;
            return new Entity(collider, position, GameplayConstants.VEINBALL_LIFE, Fraction.Virus, CollidingFractions.Cell, new Shooter(GameplayConstants.VEINBALL_CHASE_RADIUS, GameplayConstants.VEINBALL_SPEED, GameplayConstants.VEINBALL_SHOOT_RADIUS), new BasicPushResponse(true), EmptyActivatable.Instance, new TextureDrawer("gfx/Enemies/veinball.png"));
        }

        private static Entity CreateBlocker(ACollider collider, Vector2f position, object[] additionalInfos)
        {
            Entity result = new Entity(collider, position, float.PositiveInfinity, Fraction.Neutral, CollidingFractions.All, EmptyThinker.Instance, new BasicPushResponse(false), EmptyActivatable.Instance, null);

            result.Collider.SetColor(new Color(10, 10, 10, 255));

            return result;
        }

        private static Entity CreateProjectile(ACollider collider, Vector2f position, object[] additionalInfos)
        {
            return new Entity(new CircleCollider(16), position, 1.0f, (Fraction)additionalInfos[0], (CollidingFractions)additionalInfos[1], new ProjectileThinker((Vector2f)additionalInfos[2], (float)additionalInfos[3]), new ProjectileResponse((float)additionalInfos[4]), EmptyActivatable.Instance, new TextureDrawer((String)additionalInfos[5]));
        }

        private static Entity CreateSpawner(ACollider collider, Vector2f pos, object[] additionalInfos)
        {
            Entity e = new Entity(collider, pos, float.PositiveInfinity, Fraction.Neutral, CollidingFractions.None, new SpawnerThinker((FloatRect)additionalInfos[0], (int)additionalInfos[3], (double)additionalInfos[1], (double)additionalInfos[4], (EntityAttribs)additionalInfos[5]), EmptyResponse.Instance, EmptyActivatable.Instance, null);
            e.Drawable = false;
            return e;
        }

        private static Entity CreateCloud(ACollider collider, Vector2f position, object[] additionalInfos)
        {
            AThinker cloudThinker = new CloudThinker((TimeSpan)additionalInfos[2], (float)additionalInfos[3], (float)additionalInfos[4]);
            ACollisionResponse response = new TouchDamageResponse((Fraction)additionalInfos[0], (float)additionalInfos[5], false, false);
            ADrawer drawer = new CloudDrawer((float)additionalInfos[3], (float)additionalInfos[4], (String)additionalInfos[6]);
            Entity e = new Entity(collider, position, 1.0f, (Fraction)additionalInfos[0], (CollidingFractions)additionalInfos[1], cloudThinker, response, EmptyActivatable.Instance, drawer);
            return e;
        }

        private static Entity CreateNewScout(ACollider collider, Vector2f position, object[] additionalObjects)
        {
            collider.Scale = GameplayConstants.PLAYER_SCALE;
            AWeapon weapon = weapon = new ShooterWeapon(30, TimeSpan.FromMilliseconds(GameplayConstants.SCOUT_SHOOTER_FREQ), new CircleCollider(16), float.PositiveInfinity, GameplayConstants.SCOUT_SHOOTER_DAMAGE, GameplayConstants.SCOUT_SHOOTER_SPEED);
            AWeapon specialWeapon = new ScoutSpecial(TimeSpan.FromMilliseconds(GameplayConstants.SCOUT_SPECIAL_FREQ), TimeSpan.FromMilliseconds(GameplayConstants.SCOUT_SPECIAL_DURATION), TimeSpan.FromMilliseconds(GameplayConstants.SCOUT_DECREASED_FREQ), weapon);

            return new Entity(collider, position, GameplayConstants.SCOUT_START_LIFE, Fraction.Cell, CollidingFractions.Virus, new Components.PlayerThinker(weapon, specialWeapon, GameplayConstants.SCOUT_MAX_SPEED, (GInput)additionalObjects[0]), new BasicPushResponse(true), Components.EmptyActivatable.Instance, new Components.ScoutDrawer(specialWeapon));
        }

        private static Entity CreateNewTank(ACollider collider, Vector2f position, object[] additionalObjects)
        {
            collider.Scale = GameplayConstants.PLAYER_SCALE;
            AWeapon weapon = weapon = new MeeleWeapon(TimeSpan.FromSeconds(GameplayConstants.TANK_ATTACK_COOLDOWN), GameplayConstants.TANK_ATTACK_DURATION, 0, Fraction.CellProjectile, CollidingFractions.VirusProjectile, GameplayConstants.TANK_MIN_RANGE, GameplayConstants.TANK_MAX_RANGE, GameplayConstants.TANK_NUM_ATTACKS, GameplayConstants.TANK_ATTACK_SERIES_DURATION);
            AWeapon specialWeapon = new MeleeSpecial(TimeSpan.FromMilliseconds(GameplayConstants.TANK_SPECIAL_FREQ), TimeSpan.FromMilliseconds(GameplayConstants.TANK_SPECIAL_DURATION), 50);

            return new Entity(collider, position, GameplayConstants.TANK_START_LIFE, Fraction.Cell, CollidingFractions.Virus, new Components.PlayerThinker(weapon, specialWeapon, GameplayConstants.TANK_MAX_SPEED, (GInput)additionalObjects[0]), new Components.BasicPushResponse(true), Components.EmptyActivatable.Instance, new Components.TankDrawer(specialWeapon));
        }
    }
}
