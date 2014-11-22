using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViralTree.World;

namespace ViralTree.Components
{
    public class Shooter : AThinker
    {
        Entity chaser = null;
        Entity target = null;
        Fraction enemyFraction;
        float runRadius;
        float shootRadius;
        float speed;

        Vector2f moveDir;
        Vector2f shootDir;

        private AWeapon weapon;

        public override float nextAttack()
        {
            return weapon.nextAttack();
        }

        public Shooter(float runRadius, float speed, float shootRadius)
        {
            this.runRadius      = runRadius;
            this.speed          = speed;
            this.shootRadius    = shootRadius;
            this.weapon = new ShooterWeapon(30, TimeSpan.FromMilliseconds(GameplayConstants.VEINBALL_SHOOTER_FREQ), new CircleCollider(16), float.PositiveInfinity, GameplayConstants.VEINBALL_SHOOTER_DAMAGE, GameplayConstants.VEINBALL_SHOOTER_SPEED);
        }

        public override void Initialize()
        {
            enemyFraction = this.Owner.Fraction == Fraction.Virus ? Fraction.Cell : Fraction.Virus;
             
            weapon.Owner = this.Owner;
        }

        /*private void SpawnProjectile(World.GameWorld world, Vector2f dir)
        {
            if (target != null)
            {
                Vector2f position = Owner.Collider.Position + Owner.Collider.Direction *  spawnOffsetDistance;
                Fraction projectileFraction = Owner.Fraction == Fraction.Cell ? Fraction.CellProjectile : Fraction.VirusProjectile;
                CollidingFractions projectileCollidingFraction = Owner.Fraction == Fraction.Cell ? CollidingFractions.VirusProjectile : CollidingFractions.CellProjectile;

                world.AddEntity(World.EntityFactory.Create(EntityType.Projectile, position, Owner.Collider.Copy(), new object[] { projectileFraction, projectileCollidingFraction, dir, 10.0f, "gfx/Projectiles/BasicProjectile.png" }));
            }
        }*/

        public override void Update(GameTime gameTime, GameWorld world)
        {
            if (MathUtil.Rand.NextDouble() < 0.5)
                    weapon.Update(gameTime, world);
            //runnung away
            if(chaser == null)
                chaser = world.GetClosestEntityInRadius(this.Owner, enemyFraction, runRadius);

            else
            {

                //Console.WriteLine(chaser.UniqueId);
                moveDir = - (chaser.Collider.Position - this.Owner.Collider.Position);
                float len = Vec2f.Length(moveDir);

                if (len > runRadius)
                    chaser = null;

                else
                {
                    moveDir = Vec2f.Normalized(moveDir, len) * speed * (float)gameTime.ElapsedTime.TotalSeconds;
                    this.Owner.Collider.Move(moveDir);
                }

           
            }

            //shooting
            if (target == null || target.UniqueId == -1)
                target = world.GetClosestEntityInRadius(this.Owner, enemyFraction, runRadius);

            else
            {

                //Console.WriteLine(chaser.UniqueId);
                shootDir = (target.Collider.Position - this.Owner.Collider.Position);
                float len = Vec2f.Length(moveDir);

                if (len > shootRadius)
                    target = null;

                else
                {
                    //moveDir = Vec2f.Normalized(moveDir, len) * speed * (float)gameTime.ElapsedTime.TotalSeconds;
                    //this.Owner.Collider.Move(moveDir);
                    Owner.Collider.Direction = shootDir;
                    
                   // if (MathUtil.Rand.NextDouble() < 0.001)
                        weapon.Attack(world);
                }
                

            }
        }
    }
}
