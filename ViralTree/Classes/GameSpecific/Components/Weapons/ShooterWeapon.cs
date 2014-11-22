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
    class ShooterWeapon : AWeapon
    {
        private float spawnOffsetDistance;
        private TimeSpan maxCoolDown;
        private TimeSpan coolDown;
        private ACollider colliderPrototype;
        private float ammo;
        private float damage;
        public ShooterWeapon(float spawnOffsetDistance, TimeSpan coolDown, ACollider colliderPrototype, float ammo, float damage)
        {
            this.spawnOffsetDistance = spawnOffsetDistance;
            this.coolDown = coolDown;
            this.maxCoolDown = coolDown;
            this.colliderPrototype = colliderPrototype;
            this.ammo = ammo;
            this.damage = damage;
        }

        public override void Update(GameTime gameTime, World.GameWorld world)
        {
            if (coolDown > TimeSpan.Zero)
            {
                coolDown -= gameTime.ElapsedTime;
            }
        }

        private void SpawnProjectile(World.GameWorld world)
        {
            Vector2f position = Owner.Collider.Position + spawnOffsetDistance * Owner.Collider.Direction;
            Fraction projectileFraction = Owner.Fraction == Fraction.Cell ? Fraction.CellProjectile : Fraction.VirusProjectile;
            CollidingFractions projectileCollidingFraction = Owner.Fraction == Fraction.Cell ? CollidingFractions.VirusProjectile : CollidingFractions.CellProjectile;

            world.AddEntity(World.EntityFactory.Create(EntityType.Projectile, position, colliderPrototype.Copy(), new object[] { projectileFraction, projectileCollidingFraction, Owner.Collider.Direction, 2000.0f, damage, "gfx/Projectiles/BasicProjectile.png" }));
        }

        public override void Attack(World.GameWorld world)
        {
            if (coolDown <= TimeSpan.Zero && ammo > 0)
            {
                --ammo;
                SpawnProjectile(world);
                coolDown = maxCoolDown;
            }
        }
    }
}
