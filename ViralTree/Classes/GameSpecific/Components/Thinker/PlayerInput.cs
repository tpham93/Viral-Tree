using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Window;

namespace ViralTree.Components
{
    class PlayerInput
    {
        public Vector2f Movement
        {
            get { return movement; }
            set { movement = value; }
        }
        public Vector2f Direction
        {
            get { return direction; }
            set { direction = value; }
        }
        public bool Attacking
        {
            get { return attacking; }
            set { attacking = value; }
        }

        private Vector2f movement;
        private Vector2f direction;
        private bool attacking;
    }
}
