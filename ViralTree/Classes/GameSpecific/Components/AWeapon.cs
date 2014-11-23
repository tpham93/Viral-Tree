using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ViralTree.World;

namespace ViralTree.Components
{
    public abstract class AWeapon
    {
        Entity owner;
        public Entity Owner
        {
            get { return owner; }
            set { owner = value; }
        }

        private TimeSpan maxCoolDown;
        public virtual float nextAttack()
        {
            return 1;
        }

        public TimeSpan MaxCoolDown
        {
            get { return maxCoolDown; }
            set { maxCoolDown = value; }
        }
        private TimeSpan coolDown;

        public TimeSpan CoolDown
        {
            get { return coolDown; }
            set { coolDown = value; }
        }

        private float ammo;
        public float Ammo
        {
            get { return ammo; }
            set { ammo = value; }
        }

        private float maxAmmo;
        public float MaxAmmo
        {
            get { return maxAmmo; }
            set { maxAmmo = value; }
        }

        public AWeapon(TimeSpan coolDown, float ammo)
        {
            this.maxCoolDown = coolDown;
            this.coolDown = coolDown;
            this.ammo = ammo;
            this.maxAmmo = ammo;
        }
        public virtual void Update(GameTime gameTime, World.GameWorld world)
        {
            if (CoolDown > TimeSpan.Zero)
            {
                CoolDown -= gameTime.ElapsedTime;
            }
        }

        public abstract void Attack(World.GameWorld world);
    }
}
