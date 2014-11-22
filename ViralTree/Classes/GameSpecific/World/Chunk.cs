using SFML.Graphics;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViralTree.World
{
    /// <summary>
    /// A subdivision of the world, stores all entities inside of it.
    /// </summary>
    public class Chunk
    {
        #region Member

        //reference to the world, the chunk belongs to.
        private GameWorld world;

        //the id of this chunk (x and y)
        public Vector2i Id { get; private set; }

        //all entities that are in this chunk
        public MyList<Entity> chunkEntities;
       
        #endregion

        public Chunk(int idX, int idY, GameWorld world)
        {
            this.Id = new Vector2i(idX, idY);

            chunkEntities = new MyList<Entity>();

            this.world = world;
        }


        /// <summary>
        /// Updates all Entities inside of it and checks if any Entity is leaving the Chunk and notifies the GameWorld it this happens.
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            for (int i = 0; i < chunkEntities.Count; i++)
            {
                //update one specific Entity
                chunkEntities[i].Update(gameTime, world);

                //notifies the gameworld with which chunks this entity could intersect
                CheckEntityBoundingsFor(chunkEntities[i]);

                //notify that this entity could have left:
                chunkEntities[i].TestForLeavingChunk(world);
            }

        }

        //TODO: make more efficient:
        /// <summary>
        /// Stores all chunks to the gameworld with which chunks the given entity could intersect with:
        /// </summary>
        /// <param name="e"></param>
        private void CheckEntityBoundingsFor(Entity e)
        {
            Vector2i chunkUpperLeft = new Vector2i((int)(e.Collider.BoundingRectangle.Left / world.ChunkWidth),
                                                    (int)(e.Collider.BoundingRectangle.Top / world.ChunkHeight));

            Vector2i chunkUpperRight = new Vector2i((int)((e.Collider.BoundingRectangle.Left + e.Collider.BoundingRectangle.Width) / world.ChunkWidth),
                                                    (int)(e.Collider.BoundingRectangle.Top / world.ChunkHeight));

            Vector2i chunkLowerLeft = new Vector2i((int)(e.Collider.BoundingRectangle.Left / world.ChunkWidth),
                                                    (int)((e.Collider.BoundingRectangle.Top + e.Collider.BoundingRectangle.Height) / world.ChunkHeight));

            Vector2i chunkLowerRight = new Vector2i((int)((e.Collider.BoundingRectangle.Left + e.Collider.BoundingRectangle.Width) / world.ChunkWidth),
                                                    (int)((e.Collider.BoundingRectangle.Top + e.Collider.BoundingRectangle.Height) / world.ChunkHeight));


            Console.WriteLine(e.Collider.BoundingRectangle);

            world.collidableEntities.Enqueue(new EntityChunkLookup(e.UniqueId, chunkUpperLeft));

         //   if (!MathUtil.IsEqual(chunkUpperLeft, chunkUpperRight))
                world.collidableEntities.Enqueue(new EntityChunkLookup(e.UniqueId, chunkUpperRight));
            
            /*
            if (!MathUtil.IsEqual(chunkUpperLeft, chunkLowerLeft) &&
                !MathUtil.IsEqual(chunkUpperRight, chunkLowerLeft))
                */
                world.collidableEntities.Enqueue(new EntityChunkLookup(e.UniqueId, chunkLowerLeft));
/*
            if (!MathUtil.IsEqual(chunkUpperLeft, chunkLowerRight) &&
                !MathUtil.IsEqual(chunkUpperRight, chunkLowerRight) &&
                !MathUtil.IsEqual(chunkLowerLeft, chunkLowerRight))
 * */
                world.collidableEntities.Enqueue(new EntityChunkLookup(e.UniqueId, chunkLowerRight));
        }



        public void Draw(GameTime gameTime, RenderTarget target)
        {
            for (int i = 0; i < chunkEntities.Count; i++)
                chunkEntities[i].Draw(gameTime, target);
        }


        //TODO: maybe notify e for entering a new chunk
        public void AddEntity(Entity e)
        {
            chunkEntities.pushBack(e);
        }

        //TODO: maybe notify e for leaving a new chunk
        public void RemoveEntity(Entity e)
        {
            chunkEntities.remove(e);
        }


        public void DrawBoundings(RenderTarget target)
        {
            VertexArray array = new VertexArray(PrimitiveType.LinesStrip, 5);

            array[0] = new Vertex(new Vector2f(this.Id.X * world.ChunkWidth, this.Id.Y * world.ChunkHeight), Color.Magenta);
            array[1] = new Vertex(new Vector2f(this.Id.X * world.ChunkWidth, this.Id.Y * world.ChunkHeight + world.ChunkHeight), Color.Magenta);
            array[2] = new Vertex(new Vector2f(this.Id.X * world.ChunkWidth + world.ChunkWidth, this.Id.Y * world.ChunkHeight + world.ChunkHeight), Color.Magenta);
            array[3] = new Vertex(new Vector2f(this.Id.X * world.ChunkWidth + world.ChunkWidth, this.Id.Y * world.ChunkHeight), Color.Magenta);
            array[4] = new Vertex(new Vector2f(this.Id.X * world.ChunkWidth, this.Id.Y * world.ChunkHeight), Color.Magenta);

            target.Draw(array);
        }
    }

    public struct EntityChunkLookup
    {
        public int entityId;

        public Vector2i chunkId;


        public EntityChunkLookup(int id, int cIdx, int cIdy)
        {
            entityId = id;
            chunkId.X = cIdx;
            chunkId.Y = cIdy;
        }

        public EntityChunkLookup(int id, Vector2i chunkId)
        {
            entityId = id;
            this.chunkId = chunkId;
        }

    }
}
