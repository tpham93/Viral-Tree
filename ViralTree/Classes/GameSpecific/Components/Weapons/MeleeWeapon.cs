using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViralTree.Components;
using ViralTree.World;

namespace ViralTree.Weapons
{
    public class MeeleWeapon : AWeapon
    {

        Fraction fraction;
        CollidingFractions colFraction;
        float minRange;
        float maxRange;

        int maxAttacks = 0;
        int attackCount = 0;

        double seriesCurTime;
        double seriesMaxTime;

        double deltaSeriesAttack;

        double duration;

        public MeeleWeapon(TimeSpan cooldown, double duration, int ammo, Fraction fraction, CollidingFractions colFrac, float minRange, float maxRange, int attackNums, double attackSeriesTime)
            : base(cooldown, ammo)
        {
            this.duration = duration;
            this.fraction = fraction;
            this.colFraction = colFrac;

            this.minRange = minRange;
            this.maxRange = maxRange;

            this.attackCount = attackNums;
            this.maxAttacks = attackNums;

            this.seriesCurTime = -1;
            this.seriesMaxTime = attackSeriesTime;

            deltaSeriesAttack = seriesMaxTime / maxAttacks;
        }

        public override void Attack(World.GameWorld world, GameTime gameTime)
        {
            if (CoolDown.TotalSeconds <= 0)
            {
                seriesCurTime = deltaSeriesAttack;
                attackCount = maxAttacks - 1;

                CoolDown = MaxCoolDown;
                SpawnEntity(world);
            }

        }

        private void SpawnEntity(GameWorld world)
        {
            object[] additionalInfo = { this.Owner, fraction, colFraction, duration, minRange, maxRange, GameplayConstants.TANK_DAMAGE, float.PositiveInfinity };
            world.AddEntity(EntityFactory.Create(EntityType.Melee, Owner.Collider.Position, PolygonFactory.GetElipse(4, 64, 32), additionalInfo));
        }

        public override void Update(GameTime gameTime, GameWorld world)
        {
            base.Update(gameTime, world);

            if (seriesCurTime > 0)
            {
                seriesCurTime -= gameTime.ElapsedTime.TotalSeconds;

                if (seriesCurTime <= 0)
                {
                    attackCount--;
                    seriesCurTime = deltaSeriesAttack * attackCount;

                   // Console.WriteLine("bam");

                    SpawnEntity(world);
                }
                
            }

        }


    }
}
