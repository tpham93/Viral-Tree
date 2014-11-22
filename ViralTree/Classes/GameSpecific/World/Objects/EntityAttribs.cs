using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViralTree.World;

namespace ViralTree.Tiled
{
    public class EntityAttribs
    {
        public EntityType type;
        public Vector2f pos;
        public ACollider collider;
        public List<object> additionalAttribs = new List<object>();

        public EntityAttribs(EntityType type, ACollider collider, Vector2f pos)
        {
            this.type = type;
            this.pos = pos;
            this.collider = collider;
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

        public static EntityAttribs CreateAttrib(EntityType type)
        {
            EntityAttribs result = null;

            if (type == EntityType.Fungus)
            {
                result = new EntityAttribs(EntityType.Fungus, PolygonFactory.getRegularPolygon(5, 52), Vec2f.Zero);
            }

            if (type == EntityType.Veinball)
            {
                result = new EntityAttribs(EntityType.Veinball, new CircleCollider(48), Vec2f.Zero);
            }

            return result;
        }
    }
}
