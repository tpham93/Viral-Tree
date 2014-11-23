using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViralTree.Classes.GameSpecific.Components.Thinker;
using ViralTree.World;

namespace ViralTree.Components
{
    public class PierceThinker : AThinker
    {

        Entity parent;

        double maxAttackTime;
        double attackTime;

        float minRange;
        float maxRange;

        float t;

        float startRota;

        Vector2f attackRandOffset;

        public PierceThinker(Entity parent, double attackTime, float minRange, float maxRange)
        {
            this.parent = parent;

            this.attackTime = attackTime;
            this.maxAttackTime = attackTime;

            this.minRange = minRange;
            this.maxRange = maxRange;

            attackRandOffset = MathUtil.Rand.NextVec2fDir(MathUtil.PI_OVER_FOUR);

            startRota = MathUtil.Rand.NextFloat(0, MathUtil.TWO_PI);


        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update(GameTime gameTime, World.GameWorld world)
        {

            t = (float)(1.0 - attackTime / maxAttackTime);

            Owner.Collider.Rotation = parent.Collider.Rotation;

            Vector2f targetPos = Vec2f.RotateTransform(attackRandOffset, Vec2f.Zero, Owner.Collider.Rotation);

            this.Owner.Collider.Rotation = Vec2f.RotationFrom(targetPos);// +startRota;

            Owner.Collider.Position = parent.Collider.Position + t * (targetPos * maxRange);

            attackTime -= gameTime.ElapsedTime.TotalSeconds;

            if (attackTime <= 0)
                this.Owner.CurrentLife = 0.0f;

        }

    }
}
