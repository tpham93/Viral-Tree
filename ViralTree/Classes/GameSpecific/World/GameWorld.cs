using SFML.Graphics;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViralTree.Tiled;

namespace ViralTree.World
{
    public sealed class GameWorld
    {
        #region WorldAttribs

        public int WorldWidth { get; private set; }
        public int WorldHeight { get; private set; }

        #endregion

        #region Entities

        private Queue<Entity> removeMe = new Queue<Entity>();

        private UniqueList<Entity> entities;

        public void AddEntity(Entity e)
        {
            Debug.Assert(e.UniqueId == -1);

            entities.AddLast(e);

            Vector2i id = new Vector2i((int)(e.Collider.Position.X / ChunkWidth),
                                        (int)(e.Collider.Position.Y / ChunkHeight));

            e.ChunkId = id;

            chunks[id.X, id.Y].AddEntity(e);
        }

        private void RemoveEntity(Entity e)
        {
            Debug.Assert(e.UniqueId != -1);

            entities.Remove(e);
            chunks[e.ChunkId.X, e.ChunkId.Y].RemoveEntity(e);
        }

        public void QueueRemovingEntity(Entity e)
        {
            removeMe.Enqueue(e);
        }
        
        #endregion

        #region ChunkStuff

        /// <summary> The subdivion of the world in chunks. </summary>
        private Chunk[,] chunks;

        public int ChunkWidth { get; private set; }
        public int ChunkHeight { get; private set; }

        public int NumChunksX { get; private set; }
        public int NumChunksY { get; private set; }

        public Chunk GetChunkAt(float worldX, float worldY)
        {
            return chunks[(int)(worldX / ChunkWidth), (int)(worldY / ChunkHeight)];
        }
        public Chunk GetChunkAt(Vector2f worldPos)
        {
            return chunks[(int)(worldPos.X / ChunkWidth), (int)(worldPos.Y / ChunkHeight)];
        }
        public Chunk GetChunkAt(int idX, int idY)
        {
            return chunks[idX, idY];
        }
        public Chunk GetChunkAt(Vector2i index)
        {
            return chunks[index.X, index.Y];
        }


        /// <summary> All chunks that have currently at least one unit that leaves its Chunk. </summary>
        private Queue<EntityChunkLookup> chunkQueue;

        /// <summary>
        /// Notifies the GameWorld that there is a leaving Entity at the given chunk.
        /// </summary>
        /// <param name="chunkId">Where the Entity e was before leaving its current Chunk.</param>
        /// <param name="e">Which Entity left</param>
        public void NotifyLeaving(Vector2i chunkId, Entity e)
        {
            chunkQueue.Enqueue(new EntityChunkLookup(e.UniqueId, chunkId));
        }

        #endregion

        public Camera Cam
        {
            get;
            private set;
        }

        public Queue<EntityChunkLookup> collidableEntities;

        public void initCam(RenderTarget target)
        {
            Cam = new Camera(this, entities[0], target);
        }


        public GameWorld(String levelName)
        {
            TiledReader reader = new TiledReader();
            reader.Load("Content/other/level/" + levelName + ".tmx");
            Texture tileSetTexture = Game.content.Load<Texture>("other/level/"+reader.tileSetName);

            int numChunksX = (reader.numTilesX * reader.tileSizeX) / reader.spatialSizeX;
            int numChunksY = (reader.numTilesY * reader.tileSizeY) / reader.spatialSizeY;

            int chunkSizeX = reader.spatialSizeX;
            int chunkSizeY = reader.spatialSizeY;

            chunkQueue = new Queue<EntityChunkLookup>();

            entities = new UniqueList<Entity>();

            collidableEntities = new Queue<EntityChunkLookup>();

            //Compute how big the world needs to be:
            WorldWidth = numChunksX * chunkSizeX;
            WorldHeight = numChunksY * chunkSizeY;

            //The dimension of the each Chunk:
            ChunkWidth = chunkSizeX;
            ChunkHeight = chunkSizeY;

            //number of chunks:
            NumChunksX = numChunksX;
            NumChunksY = numChunksY;

            chunks = new Chunk[NumChunksX, NumChunksY];

            //create empty chunks:
            for (int i = 0; i < chunks.GetLength(0); i++)
                for (int j = 0; j < chunks.GetLength(1); j++)
                    chunks[i, j] = new Chunk(i, j, this);

            Joystick.Update();
            List<uint> connectedGamepads = GInput.getConnectedGamepads();


            ACollider tankCollider = PolygonFactory.getRegularPolygon(5, 55);
            ACollider scoutCollider = new CircleCollider(64);
            AddEntity(EntityFactory.Create(EntityType.Tank, new Vector2f(256, 256), tankCollider, new Object[] { (connectedGamepads.Count > 0 ? new GInput(connectedGamepads[0]) : null) }));


            //////________________________________________ADD ENTITIES ____________________________
            for (int i = 0; i < reader.entityAttributs.Count; i++)
            {
                EntityAttribs tmpAttrib = reader.entityAttributs[i];
                AddEntity(EntityFactory.Create(tmpAttrib.type, tmpAttrib.pos, tmpAttrib.collider, MathUtil.ToArray<object>(tmpAttrib.additionalAttribs)));
            }
      

            //////________________________________________ADD SPRITES ____________________________
            int numTilesXOnTileSet =  reader.tileSetSizeX / reader.tileSizeX;
            int numTilesYOnTileSet =  reader.tileSetSizeY / reader.tileSizeY;

            //tileSetTexture.Smooth = true;
            for (int i = 0; i < reader.tileIds.Count; i++)
            {
                Vector2i sourceRectId = new Vector2i(reader.tileIds[i].z % numTilesXOnTileSet, reader.tileIds[i].z / numTilesYOnTileSet);
                Sprite tmp = new Sprite(tileSetTexture);
                //tmp.Scale = new Vector2f(2.0f, 2.0f);
                tmp.Position = new Vector2f((reader.tileIds[i].x * reader.tileSizeX), (reader.tileIds[i].y * reader.tileSizeY));
                tmp.TextureRect = new IntRect(sourceRectId.X * reader.tileSizeX, sourceRectId.Y * reader.tileSizeY, reader.tileSizeX, reader.tileSizeY);
              //  Console.WriteLine(tmp.Position);
             //   Console.WriteLine(tmp.TextureRect);
                AddSprite(tmp);
            }

        }




        public void Update(GameTime gameTime, RenderTarget target)
        {
            //update everything relevant (points close to the cam center)
            for (int i = -Cam.updateOffset.X; i <= Cam.updateOffset.X; i++)
            {
                for (int j = -Cam.updateOffset.Y; j <= Cam.updateOffset.Y; j++)
                {
                    int tmpX = Cam.chunkIdCam.X + i;
                    int tmpY = Cam.chunkIdCam.Y + j;

                    if (tmpX < 0 || tmpX >= NumChunksX || tmpY < 0 || tmpY >= NumChunksY)
                        continue;

                    else
                        chunks[tmpX, tmpY].Update(gameTime);
                }
            }

            //Test for colliding Entities:
            while (collidableEntities.Count > 0)
            {     
                EntityChunkLookup tmpLookup = collidableEntities.Dequeue();

                Entity tmpEntity = entities.GetByUniqueId(tmpLookup.entityId);

                IntersectionData totalStaticData = new IntersectionData(false);
    
              
                for (int i = 0; i < chunks[tmpLookup.chunkId.X,tmpLookup.chunkId.Y].chunkEntities.Count; i++)
                {
                    Entity tmpOtherEntity = chunks[tmpLookup.chunkId.X, tmpLookup.chunkId.Y].chunkEntities[i];

                    if (tmpOtherEntity == tmpEntity)// && !Entity.CanCollide(tmpEntity.Fraction,tmpOtherEntity.Fraction, tmpEntity.CollidingFraction, tmpOtherEntity.CollidingFraction))
                        continue;

                    else
                    {
                        IntersectionData tmpData = tmpEntity.Collider.Intersects(tmpOtherEntity.Collider);

                        if (tmpData.Intersects)
                        {
                            tmpEntity.Response.OnCollision(tmpOtherEntity, tmpData, this, true, gameTime);

                            tmpOtherEntity.Response.OnCollision(tmpEntity, tmpData, this, false, gameTime);
                        }
                    }
               
                }

            } 

            

            //handle leaving Entities:
            //as long as there are chunks that have leaving entities:
            while(chunkQueue.Count > 0)
            {
                EntityChunkLookup currentLookUp = chunkQueue.Dequeue();

                Entity e = entities.GetByUniqueId(currentLookUp.entityId);

                if (e.LeavesChunk)
                {
                    e.LeavesChunk = false;

                    chunks[currentLookUp.chunkId.X, currentLookUp.chunkId.Y].chunkEntities.remove(e);

                    chunks[e.ChunkId.X, e.ChunkId.Y].AddEntity(e);
                }
                
            }

            while (removeMe.Count > 0)
            {
                RemoveEntity(removeMe.Dequeue());
            }
            
            Cam.Update(gameTime, target);
        }


        /// <summary>
        /// Activates the nearest Entity within its individual ActivatableDistance, that is within 3x3 Chunks from the caller.
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="caller"></param>
        public void ActivateEntity(GameTime gameTime, Entity caller)
        {
            float minSoFar = float.PositiveInfinity;
            Entity minEntity = null;

            //for each chunk in x direction:
            for (int i = -1; i <= 1; i++)
            {
                //each chunk in y direction:
                for (int j = -1; j <= 1; j++)
                {
                    //the ChunkId:
                    Vector2i tmpId = new Vector2i(caller.ChunkId.X + i, caller.ChunkId.Y + j);

                    //only check if the ChunkId is avaible (inside the world):
                    if (tmpId.X >= 0 && tmpId.X < NumChunksX && tmpId.Y >= 0 && tmpId.Y < NumChunksY)
                    {
                        //test for every entity inside the current Chunk:
                        for (int k = 0; k < chunks[tmpId.X, tmpId.Y].chunkEntities.Count; k++)
                        {
                            Entity tmpEntity = chunks[tmpId.X, tmpId.Y].chunkEntities[k];

                            if (caller != tmpEntity)
                            {
                                float tmpDistance = Vec2f.EuclidianDistanceSq(caller.Collider.Position, tmpEntity.Collider.Position);

                                if (tmpDistance < minSoFar && tmpDistance <= tmpEntity.Activatable.ActivateDistance * tmpEntity.Activatable.ActivateDistance)
                                {
                                    minSoFar = tmpDistance;
                                    minEntity = tmpEntity;
                                }
                            }
                        }                           
                    }
                }
            }

            if (minEntity != null)
                minEntity.Activatable.OnActivation(gameTime, this, caller);
        }


        public void Draw(GameTime gameTime, RenderTarget target)
        {
            //draw everything on screen:

            //first pass: draw everything beyond the entities, then on second pass, draw entities
            for (int k = 0; k < 2; k++)
            {

                for (int i = -Cam.drawOffset.X; i <= Cam.drawOffset.X; i++)
                {
                    for (int j = -Cam.drawOffset.Y; j <= Cam.drawOffset.Y; j++)
                    {
                        int tmpX = Cam.chunkIdCam.X + i;
                        int tmpY = Cam.chunkIdCam.Y + j;

                        if (tmpX < 0 || tmpX >= NumChunksX || tmpY < 0 || tmpY >= NumChunksY)
                            continue;

                        else
                        {
                            if (k == 0)
                                chunks[tmpX, tmpY].DrawSprites(gameTime, target);
                            else
                                chunks[tmpX, tmpY].DrawEntities(gameTime, target);
                        }


                    }
                }
            }

            //TODO: currently for debug purposes:
        //    DrawBoundingsOfChunks(gameTime, target);

        }


        /// <summary>
        /// Draws the boundings of a Chunks, use for debug purposes only.
        /// </summary>
        /// <param name="target"></param>
        private void DrawBoundingsOfChunks(GameTime gameTime, RenderTarget target)
        {
            for (int i = -Cam.drawOffset.X; i <= Cam.drawOffset.X; i++)
            {
                for (int j = -Cam.drawOffset.Y; j <= Cam.drawOffset.Y; j++)
                {
                    int tmpX = Cam.chunkIdCam.X + i;
                    int tmpY = Cam.chunkIdCam.Y + j;

                    if (tmpX < 0 || tmpX >= NumChunksX || tmpY < 0 || tmpY >= NumChunksY)
                        continue;

                    else
                        chunks[tmpX, tmpY].DrawBoundings(target);
                }
            }
        }

        private void AddSprite(Sprite s)
        {
            Vector2i id = new Vector2i((int)(s.Position.X / this.ChunkWidth), (int)(s.Position.Y / this.ChunkHeight));

            if(IsValidId(id))
            chunks[id.X, id.Y].sprites.Add(s);
        }


        public bool IsValidId(Vector2i id)
        {
            return IsValidId(id.X, id.Y);
        }

        public bool IsValidId(int x, int y)
        {
            return x >= 0 && x < NumChunksX && y >= 0 && y < NumChunksY;
        }

        public Entity GetClosestEntityInRadius(Entity refEntity, Fraction desiredFraction, float radius)
        {
            float minDist = float.PositiveInfinity;
            Entity closest = null;

            Vector2i idOffset = new Vector2i((int)(radius / ChunkWidth) + 2, (int)(radius / ChunkHeight) + 2);

          //  Console.WriteLine(idOffset);

            for (int i = -idOffset.X; i < idOffset.X; i++)
            {
                for (int j = -idOffset.Y; j < idOffset.Y; j++)
                {
                    Vector2i trueId = new Vector2i(i, j) + refEntity.ChunkId;

                    if (IsValidId(trueId))
                    {
                        for (int k = 0; k < chunks[trueId.X, trueId.Y].chunkEntities.Count; k++)
                        {
                            Entity tmp = chunks[trueId.X, trueId.Y].chunkEntities[k];

                            if (tmp.Fraction == desiredFraction && refEntity != tmp)
                            {
                                float tmpDist = Vec2f.EuclidianDistanceSq(tmp.Collider.Position, refEntity.Collider.Position);

                                if (tmpDist < minDist)
                                {
                                    minDist = tmpDist;
                                    closest = tmp;
                                }
                            }
                        }

                    }
              
                }
            }

            return closest;
        }

        public Entity GetEntity(int uniqueId)
        {
            return entities.GetByUniqueId(uniqueId);
        }

    }
}
