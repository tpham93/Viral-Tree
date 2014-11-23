using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViralTree.World;

namespace ViralTree.Components
{
    public class ExitResponse : ACollisionResponse
    {
        public Entity player1;
        public Entity player2;

        public int numKeys;

        private List<KeyResponse> keys = new List<KeyResponse>();

        public ExitResponse(int numKeys, Entity player1, Entity player2)
        {
            this.player1 = player1;
            this.player2 = player2;

            this.numKeys = numKeys;
        }

        public override void Initialize()
        {
            base.Initialize();
     
        }

        public override void OnCollision(World.Entity collidedEntity, IntersectionData data, World.GameWorld world, bool firstCalled, GameTime gameTime)
        {
            if (numKeys <= 0 && (player1 != null && collidedEntity == player1 || player2 != null && collidedEntity == player2))
            {
                Owner.CurrentLife = 0.0f;
            }
        }

        public override void Update(GameTime gameTime, World.GameWorld world)
        {
            (Owner.Drawer as TextureDrawer).useRed = false;

            if (numKeys <= 0)
            {
                (Owner.Drawer as TextureDrawer).sprite.Color = Color.White;
            }

            else
            {
                (Owner.Drawer as TextureDrawer).sprite.Color = new Color(255, 255, 255, 0);
            }
        }

        public void RemoveNumKey(KeyResponse res)
        {
            if (!keys.Contains(res))
            {
                keys.Add(res);
                numKeys--;
            }

            
        }
    }
}
