using ViralTree.Components;
using ViralTree.World;
using SFML.Graphics;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViralTree.World
{
    public class Entity : IUnique
    {
        #region Member

        /// <summary> UniqueId for the whole GameWorld </summary>
        public int UniqueId { get; set; }

        /// <summary> The indices in which Chunk this entity is.</summary>
        public Vector2i ChunkId { get; set; }

        /// <summary> The type of ACollider this entity owns and that is used for collision detection. </summary>
        public ACollider Collider { get; private set; }

        /// <summary> If this entity is supposed to be drawn right now.</summary>
        public bool Drawable { get; set; }

        /// <summary> If this entity is about to leave its current chunk.</summary>
        public bool LeavesChunk { get; set; }


        private Components.AThinker thinker;
        public Components.AThinker Thinker
        {
            get { return thinker; }
            set { value.Owner = this; value.IsActive = true; thinker = value; }
           
        }


        private Components.ACollisionResponse response;
        public Components.ACollisionResponse Response
        {
            get { return response; }
            set { value.Owner = this; value.IsActive = true; response = value; }

        }


        private Components.AActivator activator;
        public Components.AActivator Activator
        {
            get { return activator; }
            set { value.Owner = this; value.IsActive = true; activator = value; }
        }


        private Components.AActivatable activatable;
        public Components.AActivatable Activatable
        {
            get { return activatable; }
            set { value.Owner = this; value.IsActive = true; activatable = value; }
        }

        #endregion


        public Entity(ACollider collider, Vector2f position)
        {
            this.Collider = collider;

            this.Collider.Position = position;

            UniqueId = -1;

            ChunkId = new Vector2i(0, 0);

            Drawable = true;

            LeavesChunk = false;
        }

        public Entity(ACollider collider, Vector2f position, AThinker thinker, ACollisionResponse response, AActivator activator, AActivatable activatable)
            : this(collider, position)
        {
            this.Thinker = thinker;
            this.Response = response;
            this.Activator = activator;
            this.Activatable = activatable;
        }

        public void InitEmptyComponents()
        {

            thinker = Components.EmptyThinker.Instance;

            response = Components.EmptyResponse.Instance;

            activator = Components.EmptyActivator.Instance;

            activatable = Components.EmptyActivatable.Instance;
        }


        public void Update(GameTime gameTime, GameWorld world)
        {

            if(thinker.IsActive)
                thinker.Update(gameTime, world);

            if(response.IsActive)
                response.Update(gameTime, world);

            if(activator.IsActive)
                activator.Update(gameTime, world);

            if(activatable.IsActive)
                activatable.Update(gameTime, world);
            
        }

        /// <summary>
        /// Tests if this Entity left its current chunk and handles the leaving aswell.
        /// </summary>
        /// <param name="world"></param>
        public void TestForLeavingChunk(GameWorld world)
        {
            //the new chunk:
            Vector2i maybeNewChunkId = new Vector2i((int)(Collider.Position.X / world.ChunkWidth), (int)(Collider.Position.Y / world.ChunkHeight));

            if (!MathUtil.IsEqual(maybeNewChunkId, ChunkId))
            {
                //notify that the entity on the old chunk, wants to leave:
                world.NotifyLeaving(ChunkId, this);

                ChunkId = maybeNewChunkId;

                LeavesChunk = true;
            }
        }


        public void Draw(GameTime gameTime, RenderTarget target)
        {
            if (Drawable)
                Collider.Draw(target);
        }

    }
}
