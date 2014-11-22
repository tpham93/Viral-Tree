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
    //TODO: add possibility to follow an Entity OR follow a camera path or the like
    public class Camera
    {
        private GameWorld world;

        /// <summary> The Entity that is focused (followed) by the camera.</summary>
        public Entity entityOnCameraFocus;

        /// <summary> The current view (camera) in the world.</summary>
        public View currentView;

        public Vector2i chunkIdCam;

        /// <summary> Chunk offset from the center of the camera that will being updated</summary>
        public Vector2i updateOffset;

        /// <summary> Chunk offset from the center of the camera that will being drawn</summary>
        public Vector2i drawOffset;

        public float allowedDist;

        public Vector2f Position
        {
            get { return currentView.Center; }
            set { currentView.Center = value; }
        }
       
        public Camera(GameWorld world, Entity startEntity, RenderTarget target)
        {
            this.world = world;

            this.entityOnCameraFocus = startEntity;

            this.currentView = target.GetView();

            Settings.ResizedEvent += ComputeChunkOffsets;
            ComputeChunkOffsets();

            allowedDist = 10.0f;

            if (startEntity != null)
            {
                chunkIdCam = startEntity.ChunkId;

                currentView.Center = startEntity.Collider.Position;

                target.SetView(currentView);
            }
               
        }

        public void ComputeChunkOffsets()
        {

            //everything on screen + 10 chunks
            updateOffset = new Vector2i(Settings.WindowSize.X / (world.ChunkWidth * 2) + 10,
                                      Settings.WindowSize.Y / (world.ChunkWidth * 2) + 10);


            ///Add 1 just to be sure everythin on screen will be drawn:
            //everything on screen + 1 chunk
            drawOffset = new Vector2i(Settings.WindowSize.X / (world.ChunkWidth * 2) + 50,
                                      Settings.WindowSize.Y / (world.ChunkWidth * 2) + 50);
        }


        //TODO: update only if necessary:
        public void Update(GameTime gameTime, RenderTarget target)
        {

            bool changed = false;

            if (entityOnCameraFocus != null)
                changed = FollowEntity(gameTime);


            //TODO: remove, only for debug purpose:
            if (MInput.MouseWheelUp())
            {
                currentView.Zoom(1.075f);
                changed = true;
            }

            else if (MInput.MouseWheelDown())
            {
                currentView.Zoom(0.925f);
                changed = true;
            }


            //update the view only if there were any changes
            if(changed)
                target.SetView(currentView);

        }


        private bool FollowEntity(GameTime gameTime)
        {
            Vector2f direction = entityOnCameraFocus.Collider.Position - currentView.Center;

            if (Vec2f.Length(direction) > allowedDist)
            {
                currentView.Move(direction / 25.0f);

                chunkIdCam = new Vector2i((int)(currentView.Center.X / world.ChunkWidth), (int)(currentView.Center.Y / world.ChunkHeight));
                return true;
            }

            return false;
        }
    }
}
