using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViralTree.Tiled
{
    public class EntityAttribs
    {
        public Vector2f pos;
        ACollider collider;
        List<object> additionalAttribs = new List<object>();

        public EntityAttribs(ACollider collider, Vector2f pos)
        {
            this.pos = pos;
        }

        public object[] GetAttribs()
        {
            object[] objs = new object[additionalAttribs.Count];

            for (int i = 0; i < additionalAttribs.Count; i++)
                objs[i] = additionalAttribs[i];

            return objs;
        }

        public ACollider GetCollider()
        {
            return collider;
        }

        public void AddAttribute(object obj)
        {
            additionalAttribs.Add(obj);
        }
    }
}
