using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViralTree.Components;
using ViralTree.World;

namespace ViralTree.Weapons
{
    class MeleeSpecial : AWeapon
    {

        TimeSpan decreasedShootFreq;
        TimeSpan duration;
        TimeSpan maxDuration;
        AWeapon meleeWeapon;
        float shieldDist;

        public MeleeSpecial(TimeSpan coolDown, TimeSpan duration, float shieldDist)
            : base(coolDown, float.PositiveInfinity)
        {
            this.duration = TimeSpan.Zero;
            this.maxDuration = duration;
            this.shieldDist = shieldDist;
        }

        private void spawnShield(World.GameWorld world)
        {
            ACollider collider = PolygonFactory.GetHalfElipse(5, 65, 100);
            collider.Rotation = Owner.Collider.Rotation;
            AThinker thinker = new ShieldThinker(Owner, shieldDist, maxDuration);
            Entity e = new Entity(collider, Owner.Collider.Position + Owner.Collider.Direction * shieldDist, float.PositiveInfinity, Fraction.CellProjectile, CollidingFractions.VirusProjectile, thinker, new ProjectileResponse(0.0f, true), EmptyActivatable.Instance, new TextureDrawer("gfx/Projectiles/tankShield.png"));
            world.AddEntity(e);
        }

        public override void Attack(World.GameWorld world, GameTime gameTime)
        {
            if (CoolDown <= TimeSpan.Zero)
            {
                CoolDown = MaxCoolDown;
                spawnShield(world);
            }
        }


    }
}
