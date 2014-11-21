using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViralTree.World
{
    public static class EntityFactory
    {
        public static Entity CreateNewPlayer(ACollider collider, Vector2f position)
        {
            return new Entity(collider, position, new Components.PlayerThinker(), new Components.BasicPushResponse(true), new Components.BasicActivator(), Components.EmptyActivatable.Instance);
        }
    }
}
