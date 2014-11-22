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

        private float spawnOffsetDistance = 10f;
        private TimeSpan maxCoolDown;
        private TimeSpan coolDown;
        private ACollider colliderPrototype;
        private float ammo;

        public Shooter(float runRadius, float speed, float shootRadius)
        {
            this.runRadius      = runRadius;
            this.speed          = speed;
            this.shootRadius    = shootRadius;
        }

        public override void Initialize()
        {
            enemyFraction = this.Owner.Fraction == Fraction.Virus ? Fraction.Cell : Fraction.Virus;
        }

        private void SpawnProjectile(World.GameWorld world, Vector2f dir)
        {
            if (target != null)
            {
                Vector2f position = Owner.Collider.Position + Owner.Collider.Direction *  spawnOffsetDistance;
                world.AddEntity(World.EntityFactory.Create(EntityType.Projectile, position, Owner.Collider.Copy(), new object[] { Owner.Fraction, Owner.Fraction == Fraction.Cell ? CollidingFractions.CellProjectile : CollidingFractions.VirusProjectile, dir, 10.0f, "gfx/Projectiles/BasicProjectile.png" }));
            }
        }

        public override void Update(GameTime gameTime, GameWorld world)
        {        
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
            if (target == null)
                target = world.GetClosestEntityInRadius(this.Owner, enemyFraction, runRadius);

            else
            {

                //Console.WriteLine(chaser.UniqueId);
                shootDir = -(target.Collider.Position - this.Owner.Collider.Position);
                float len = Vec2f.Length(moveDir);

                if (len > shootRadius)
                    target = null;

                else
                {
                    //moveDir = Vec2f.Normalized(moveDir, len) * speed * (float)gameTime.ElapsedTime.TotalSeconds;
                    //this.Owner.Collider.Move(moveDir);
                    SpawnProjectile(world, shootDir);
                }


            }
        }
    }
}
