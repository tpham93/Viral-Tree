using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViralTree.Components;

namespace ViralTree.Weapons
{
    class ScoutSpecial : AWeapon
    {

        TimeSpan decreasedShootFreq;
        TimeSpan originalShootFreq;
        TimeSpan duration;
        TimeSpan maxDuration;
        AWeapon scoutWeapon;

        public ScoutSpecial(TimeSpan coolDown, TimeSpan duration, TimeSpan increasedWeapongFreq, AWeapon scoutWeapon)
            : base(coolDown, float.PositiveInfinity)
        {
            this.decreasedShootFreq = increasedWeapongFreq;
            this.CoolDown = TimeSpan.Zero;
            this.duration = TimeSpan.Zero;
            this.maxDuration = duration;
            this.scoutWeapon = scoutWeapon;
        }

        public override void Update(GameTime gameTime, World.GameWorld world)
        {
            if (duration > TimeSpan.Zero)
            {
                duration -= gameTime.ElapsedTime;
                if (duration < TimeSpan.Zero)
                {
                    scoutWeapon.MaxCoolDown = originalShootFreq;
                    scoutWeapon.Ammo = scoutWeapon.MaxAmmo;
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
                originalShootFreq = scoutWeapon.MaxCoolDown;
                scoutWeapon.MaxCoolDown = decreasedShootFreq;
                scoutWeapon.Ammo = float.PositiveInfinity;
                duration = maxDuration;
            }
        }


    }
}
