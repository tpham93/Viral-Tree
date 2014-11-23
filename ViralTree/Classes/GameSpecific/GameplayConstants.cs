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

        public const float PLAYER_SCALE             = 1.0f;     //in use

        public const float SCOUT_START_LIFE         = 100.0f;   //in use
        public const float SCOUT_SHOOTER_DAMAGE     = 10.0f;    //in use
        public const float SCOUT_SHOOTER_FREQ       = 250.0f;   //in use
        public const float SCOUT_SHOOTER_SPEED      = 2000.0f;  //in use
        public const float SCOUT_SPECIAL_FREQ       = 60000.0f; //in use
        public const float SCOUT_SPECIAL_DURATION   = 5000.0f;  //in use
        public const float SCOUT_DECREASED_FREQ     = 100.0f;   //in use
        public const float SCOUT_MAX_SPEED          = 400.0f;   //in use


        public const float  TANK_START_LIFE             = 200.0f;   //in use
        public const double TANK_ATTACK_COOLDOWN        = 1.0;
        public const double TANK_ATTACK_DURATION        = 0.5;
        public const double TANK_ATTACK_SERIES_DURATION = 0.15f;
        public const float  TANK_DAMAGE                 = 0.7f;
        public const float  TANK_MAX_RANGE              = 128.0f;
        public const float  TANK_MIN_RANGE              = 64.0f;
        public const int    TANK_NUM_ATTACKS            = 3;
        public const float  TANK_SPECIAL_FREQ           = 30000.0f; //in use
        public const float  TANK_SPECIAL_DURATION       = 10000.0f;  //in use
        public const float  TANK_MAX_SPEED              = 600.0f;   //in use


        //____________________________ANORISM__________________________
         
        public const float ANORISM_LIFE             = 100.0f;   //inuse
        public const float ANORISM_SPEED            = 300.0f;    //inuse
        public const float ANORISM_SCALE            = 1.0f;     //not in use
        public const float ANORISM_TOUCH_DAMAGE     = 30.0f;   //in use
        public const float ANORISM_FOLLOW_RADIUS    = 512.0f;   //in use


        //____________________________VEINBALL__________________________

        public const float VEINBALL_LIFE            = 100.0f;   //in use
        public const float VEINBALL_SPEED           = 180.0f;   //in use
        public const float VEINBALL_SCALE           = 1.0f;     //in use
        public const float VEINBALL_CHASE_RADIUS    = 512.0f;   //in use
        public const float VEINBALL_SHOOT_RADIUS    = 1024.0f;  //in use
        public const float VEINBALL_SHOOTER_DAMAGE  = 2.0f;     //in use
        public const float VEINBALL_SHOOTER_FREQ    = 1000.0f;  //in use
        public const float VEINBALL_SHOOTER_SPEED   = 1000.0f;  //in use




        //____________________________FUNGUS__________________________

        public const float FUNGUS_LIFE              = 500.0f;   //in use
        public const float FUNGUS_SPEED             = 100.0f;    //not in use
        public const float FUNGUS_SCALE             = 1.0f;     //in use
        public const float FUNGUS_AOE_FREQ          = 3000.0f;  //in use
        public const float FUNGUS_AOE_DAMAGE        = 10.0f;     //in use
        public const float FUNGUS_AOE_DURATION      = 1000.0f;  //in use
        public const float FUNGUS_AOE_MIN           = 64.0f;   //in use
        public const float FUNGUS_AOE_MAX           = 256.0f;   //in use
        public const float FUNGUS_AOE_ATTACK_RADIUS = 256.0f;   //in use
        public const float FUNGUS_CHASE_RADIUS      = 512.0f;   //in use











    }
}
