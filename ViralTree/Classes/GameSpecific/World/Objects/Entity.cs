﻿using ViralTree.Components;
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
    public enum Fraction
    {
        Neutral,
        Cell,
        CellProjectile,
        Virus,
        VirusProjectile
    }

    public enum CollidingFractions
    {
        All,
        Cell,
        CellProjectile,
        Virus,
        VirusProjectile,
        None
    }

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


        private float currentLife;
        public float CurrentLife
        {
            get { return currentLife;  }
            set { currentLife = value; }
        }

        private void helpFloat(float ding)
        {
            if (ding > maxLife)
                ding = maxLife;

            currentLife = ding;
        }

        private float maxLife;
        public float MaxLife
        {
            get { return maxLife; }
            set { maxLife = value; }
        }
        public bool isDead
        {
            get { return currentLife <= 0; }
        }

        private Components.AThinker thinker;
        public Components.AThinker Thinker
        {
            get { return thinker; }
            set { value.Owner = this; value.IsActive = true; thinker = value; thinker.Initialize(); }

        }


        private Components.ACollisionResponse response;
        public Components.ACollisionResponse Response
        {
            get { return response; }
            set { value.Owner = this; value.IsActive = true; response = value; response.Initialize(); }

        }


        private Components.ADrawer drawer;
        public Components.ADrawer Drawer
        {
            get { return drawer; }
            set { if (value != null) { value.Owner = this; value.IsActive = true; drawer = value; drawer.Initialize(); } }
        }

        private Components.AActivatable activatable;
        public Components.AActivatable Activatable
        {
            get { return activatable; }
            set { value.Owner = this; value.IsActive = true; activatable = value; activatable.Initialize(); }
        }

        private Fraction fraction;
        public Fraction Fraction
        {
            get { return fraction; }
            set { fraction = value; }
        }

        private CollidingFractions collidingFraction;
        public CollidingFractions CollidingFraction
        {
            get { return collidingFraction; }
            set { collidingFraction = value; }
        }

        #endregion


        public Entity(ACollider collider, Vector2f position, float life, Fraction fraction, CollidingFractions collidingFraction)
        {
            this.Collider = collider;

            this.Collider.Position = position;

            this.MaxLife = life;

            this.CurrentLife = life;


            this.Fraction = fraction;

            this.CollidingFraction = collidingFraction;

            UniqueId = -1;

            ChunkId = new Vector2i(0, 0);

            Drawable = true;

            LeavesChunk = false;
        }

        public Entity(ACollider collider, Vector2f position, float life, Fraction fraction, CollidingFractions collidingFractions, AThinker thinker, ACollisionResponse response, AActivatable activatable, ADrawer drawing)
            : this(collider, position, life, fraction, collidingFractions)
        {
            this.Thinker = thinker;
            this.Response = response;
            this.Drawer = drawing;
            this.Activatable = activatable;
        }

        public void InitEmptyComponents()
        {

            thinker = Components.EmptyThinker.Instance;

            response = Components.EmptyResponse.Instance;

            activatable = Components.EmptyActivatable.Instance;
        }

        public void Update(GameTime gameTime, GameWorld world)
        {

            if (thinker.IsActive)
                thinker.Update(gameTime, world);

            if (response.IsActive)
                response.Update(gameTime, world);

            if (drawer != null && drawer.IsActive)
                drawer.Update(gameTime, world);

            if (activatable.IsActive)
                activatable.Update(gameTime, world);

            if (isDead)
                world.QueueRemovingEntity(this);
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
            {
                if (drawer != null)
                    drawer.Draw(target);
                else
                    Collider.Draw(target);
            }

        }

        public static bool CanCollide(Fraction fraction, Fraction otherFraction, CollidingFractions collidingFraction, CollidingFractions otherCollidingFraction)
        {
            if (collidingFraction == CollidingFractions.None || otherCollidingFraction == CollidingFractions.None)
            {
                return false;
            }
            if (collidingFraction == CollidingFractions.All || otherCollidingFraction == CollidingFractions.All || fraction == Fraction.Neutral || otherFraction == Fraction.Neutral)
            {
                return true;
            }

            switch (fraction)
            {
                case Fraction.Cell:
                    return otherCollidingFraction == CollidingFractions.Cell || otherFraction == Fraction.Virus || otherFraction == Fraction.VirusProjectile;
                case Fraction.CellProjectile:
                    return otherCollidingFraction == CollidingFractions.Cell;
                case Fraction.Virus:
                    return otherCollidingFraction == CollidingFractions.Virus || otherFraction == Fraction.Cell || otherFraction == Fraction.CellProjectile;
                case Fraction.VirusProjectile:
                    return otherCollidingFraction == CollidingFractions.Virus;
                default:
                    break;
            }
            
            switch (otherFraction)
            {
                case Fraction.Cell:
                    return otherCollidingFraction == CollidingFractions.Cell || otherFraction == Fraction.Virus || otherFraction == Fraction.VirusProjectile;
                case Fraction.CellProjectile:
                    return otherCollidingFraction == CollidingFractions.Cell;
                case Fraction.Virus:
                    return otherCollidingFraction == CollidingFractions.Virus || otherFraction == Fraction.Cell || otherFraction == Fraction.CellProjectile;
                case Fraction.VirusProjectile:
                    return otherCollidingFraction == CollidingFractions.Virus;
                default:
                    break;
            }
            return false;
        }
    }
}
