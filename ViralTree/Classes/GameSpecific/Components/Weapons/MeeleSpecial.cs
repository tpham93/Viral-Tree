using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViralTree.Components;

namespace ViralTree.Weapons
{
    class MeleeSpecial : AWeapon
    {

        TimeSpan decreasedShootFreq;
        TimeSpan originalShootFreq;
        TimeSpan duration;
        TimeSpan maxDuration;
        AWeapon meleeWeapon;

        public MeleeSpecial(TimeSpan coolDown, TimeSpan duration, TimeSpan increasedWeapongFreq, AWeapon scoutWeapon)
            : base(coolDown, float.PositiveInfinity)
        {
            this.decreasedShootFreq = increasedWeapongFreq;
            this.CoolDown = TimeSpan.Zero;
            this.duration = TimeSpan.Zero;
            this.maxDuration = duration;
            this.meleeWeapon = scoutWeapon;
        }

        public override void Update(GameTime gameTime, World.GameWorld world)
        {
            if (duration > TimeSpan.Zero)
            {
                duration -= gameTime.ElapsedTime;
                if (duration < TimeSpan.Zero)
                {
                    meleeWeapon.MaxCoolDown = originalShootFreq;
                    meleeWeapon.Ammo = meleeWeapon.MaxAmmo;
                    Owner.CurrentLife = Owner.MaxLife;
                }
            }
            else
            {
                base.Update(gameTime, world);
            }
        }

        public override void Attack(World.GameWorld world, GameTime gameTime)
        {
            if (CoolDown <= TimeSpan.Zero)
            {
                CoolDown = MaxCoolDown;
                originalShootFreq = meleeWeapon.MaxCoolDown;
                meleeWeapon.MaxCoolDown = decreasedShootFreq;
                meleeWeapon.Ammo = float.PositiveInfinity;
                duration = maxDuration;
                Owner.CurrentLife = float.PositiveInfinity;
            }
        }


    }
}
