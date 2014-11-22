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
        private ACollider colliderPrototype;
        private float damage;
        private float speed;
        public ShooterWeapon(float spawnOffsetDistance, TimeSpan coolDown, ACollider colliderPrototype, float ammo, float damage, float speed)
            :base(coolDown, ammo)
        {
            this.spawnOffsetDistance = spawnOffsetDistance;
            this.colliderPrototype = colliderPrototype;
            this.damage = damage;
            this.speed = speed;
        }

        public override float nextAttack()
        {
             return (float)(CoolDown.TotalSeconds/MaxCoolDown.TotalSeconds); 

        }

        private void SpawnProjectile(World.GameWorld world)
        {
            Vector2f position = Owner.Collider.Position + spawnOffsetDistance * Owner.Collider.Direction;
            Fraction projectileFraction = Owner.Fraction == Fraction.Cell ? Fraction.CellProjectile : Fraction.VirusProjectile;
            CollidingFractions projectileCollidingFraction = Owner.Fraction == Fraction.Cell ? CollidingFractions.VirusProjectile : CollidingFractions.CellProjectile;

            world.AddEntity(World.EntityFactory.Create(EntityType.Projectile, position, colliderPrototype.Copy(), new object[] { projectileFraction, projectileCollidingFraction, Owner.Collider.Direction, speed, damage, "gfx/Projectiles/BasicProjectile.png" }));
        }

        public override void Update(GameTime gameTime, GameWorld world)
        {
            base.Update(gameTime, world);
            Console.WriteLine(MaxCoolDown);
        }

        public override void Attack(World.GameWorld world)
        {
            if (CoolDown <= TimeSpan.Zero && Ammo > 0)
            {
                --Ammo;
                SpawnProjectile(world);
                CoolDown = MaxCoolDown;
            }
        }
    }
}
