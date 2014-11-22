using SFML.Graphics;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ViralTree.World;

namespace ViralTree.Components
{
    class ShooterThinker : AThinker
    {
        private float spawnOffsetDistance;
        private TimeSpan maxCoolDown;
        private TimeSpan coolDown;
        private ACollider colliderPrototype;
        private float ammo;
        public ShooterThinker(float spawnOffsetDistance, TimeSpan coolDown, ACollider colliderPrototype, float ammo)
        {
            this.spawnOffsetDistance = spawnOffsetDistance;
            this.coolDown = coolDown;
            this.maxCoolDown = coolDown;
            this.colliderPrototype = colliderPrototype;
            this.ammo = ammo;
        }

        public override void Update(GameTime gameTime, World.GameWorld world)
        {
            if(coolDown > TimeSpan.Zero)
            {
                coolDown -= gameTime.ElapsedTime;
            }
        }

        private void SpawnProjectile(World.GameWorld world)
        {
            Vector2f position = Owner.Collider.Position + spawnOffsetDistance * Owner.Collider.Direction;
            world.AddEntity(World.EntityFactory.Create(EntityType.Projectile, position, colliderPrototype.Copy(), new object[] { Owner.Fraction, Owner.Fraction == Fraction.Cell ? CollidingFractions.Virus : CollidingFractions.Cell, Owner.Collider.Direction, 2000.0f, "gfx/Projectiles/BasicProjectile.png" }));
        }

        public void Attack(World.GameWorld world)
        {
            if(coolDown <= TimeSpan.Zero && ammo > 0)
            {
                --ammo;
                SpawnProjectile(world);
                coolDown = maxCoolDown;
            }
        }
    }
}
