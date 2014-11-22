using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViralTree
{
    public static class GameplayConstants
    {
        //____________________________Player__________________________

        public const float PLAYER_START_LIFE        = 100.0f;   //in use
        public const float PLAYER_MAX_SPEED         = 400.0f;   //in use
        public const float PLAYER_SCALE             = 1.0f;     //in use

        //____________________________ANORISM__________________________
         
        public const float ANORISM_LIFE             = 100.0f;   //inuse
        public const float ANORISM_SPEED            = 55.0f;    //inuse
        public const float ANORISM_SCALE            = 1.0f;     //not in use
        public const float ANORISM_FOLLOW_RADIUS    = 256.0f;   //in use
        public const float ANORISM_TOUCH_DAMAGE     = 32.0f;   //in use

        //____________________________VEINBALL__________________________

        public const float VEINBALL_LIFE            = 100.0f;   //in use
        public const float VEINBALL_SPEED           = 180.0f;   //in use
        public const float VEINBALL_SCALE           = 1.0f;     //in use
        public const float VEINBALL_CHASE_RADIUS    = 512.0f;   //in use
        public const float VEINBALL_SHOOT_RADIUS    = 1024.0f;  //in use

        //____________________________FUNGUS__________________________

        public const float FUNGUS_LIFE              = 100.0f;   //in use
        public const float FUNGUS_SPEED             = 55.0f;    //not in use
        public const float FUNGUS_SCALE             = 1.0f;     //in use




    }
}
