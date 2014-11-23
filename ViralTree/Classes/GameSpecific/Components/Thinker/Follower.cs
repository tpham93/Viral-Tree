using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViralTree.World;

namespace ViralTree.Components
{
    public class Follower : AThinker
    {
        Entity follower = null;
        Fraction enemyFraction;
        float followRadius;
        float speed;
        AWeapon weapon;
        float attackRadius;

        public Follower(float followRadius, float speed, AWeapon weapon = null, float attackRadius = 0.0f)
        {
            this.followRadius = followRadius;
            this.speed = speed;
            this.weapon = weapon;
            this.attackRadius = attackRadius;
        }

        public override void Initialize()
        {
            enemyFraction = this.Owner.Fraction == Fraction.Virus ? Fraction.Cell : Fraction.Virus;
            if(weapon != null)
            {
                weapon.Owner = Owner;
            }
        }

        public override void Update(GameTime gameTime, GameWorld world)
        {

            if (weapon != null && MathUtil.Rand.NextDouble() < 0.5)
                weapon.Update(gameTime, world);

            if(follower == null)
                follower = world.GetClosestEntityInRadius(this.Owner, enemyFraction, followRadius);

            else
            {

                //Console.WriteLine(follower.UniqueId);
                Vector2f dir = follower.Collider.Position - this.Owner.Collider.Position;
                float len = Vec2f.Length(dir);

                if (len > followRadius || follower.UniqueId == -1)
                    follower = null;

                else
                {
                    dir = Vec2f.Normalized(dir, len) * speed * (float)gameTime.ElapsedTime.TotalSeconds;
                    this.Owner.Collider.Move(dir);
                    if (weapon != null && attackRadius > len)
                    {
                        weapon.Attack(world, gameTime);
                    }
                }

           
            }
        }
    }
}
