using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SFML.Window;
using ViralTree.World;

namespace ViralTree.Components
{
    class AoeWeapon : AWeapon
    {
        private float spawnOffsetDistance;
        private ACollider colliderPrototype;
        private float damage;
        private float speed;
        private float minRadius;
        private float maxRadius;
        public AoeWeapon(TimeSpan coolDown, ACollider colliderPrototype, float ammo, float damage, float speed, float maxRadius, float minRadius)
            : base(coolDown, ammo)
        {
            this.colliderPrototype = colliderPrototype;
            this.damage = damage;
            this.speed = speed;
            this.minRadius = minRadius;
            this.maxRadius = maxRadius;
        }

        public override float nextAttack()
        {
            return (float)(CoolDown.TotalSeconds / MaxCoolDown.TotalSeconds);

        }

        private void SpawnCloud(GameWorld world)
        {
            Vector2f position = Owner.Collider.Position + spawnOffsetDistance * Owner.Collider.Direction;
            Fraction projectileFraction = Owner.Fraction == Fraction.Cell ? Fraction.CellProjectile : Fraction.VirusProjectile;
            CollidingFractions projectileCollidingFraction = Owner.Fraction == Fraction.Cell ? CollidingFractions.VirusProjectile : CollidingFractions.CellProjectile;

            world.AddEntity(World.EntityFactory.Create(EntityType.Projectile, position, colliderPrototype.Copy(), new object[] { projectileFraction, projectileCollidingFraction, Owner.Collider.Direction, speed, damage, "gfx/Projectiles/BasicProjectile.png" }));
        }

        public override void Update(GameTime gameTime, GameWorld world)
        {
            base.Update(gameTime, world);
        }

        public override void Attack(World.GameWorld world, GameTime gameTime)
        {
            if (CoolDown <= TimeSpan.Zero && Ammo > 0)
            {
                --Ammo;
                SpawnCloud(world);
                CoolDown = MaxCoolDown;
            }
        }
    }
}
