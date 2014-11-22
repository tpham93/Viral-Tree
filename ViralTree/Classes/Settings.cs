using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViralTree
{
    public static class Settings
    {
        public delegate void WindowResized();

        public delegate void FrameRateChanged();

        public delegate void StylesChanged();

        /// <summary> The Method that is called, when the window changes it size.. You can hang your own functions here aswell </summary>
        public static WindowResized ResizedEvent;

        public static FrameRateChanged FrameRateEvent;

        public static StylesChanged StyleEvent;

        public static IFormatProvider cultureProvide = System.Globalization.CultureInfo.CreateSpecificCulture("en-us");


        private static Vector2i windowSize = new Vector2i(800, 600);
        public static Vector2i WindowSize
        {
            get { return windowSize; }
            set 
            { 
                windowSize = value; 
                ResizedEvent(); 
            }
        }
        
        private static uint maxFps = 60;
        public static uint Fps
        {
            get { return maxFps; }
            set 
            { 
                maxFps = value; 
                FrameRateEvent(); 
            }
        }



        private static Styles styles = Styles.Titlebar | Styles.Close;
        public static Styles WindowStyles
        {
            get { return styles; }
            set 
            { 
                styles = value;
                StyleEvent();
            }
        }

        public static bool DrawFog = true;


        public static class Constants
        {
            //TODO: maybe make non const, for different gamestate e.g.
            public const String WINDOW_TITLE = "Title";

            public const String LOGGER_CONDITIONAL_STRING = "DEBUG";

            public const String TEST_CLASS_CONDITIONAL_STRING = "DEBUG";

            public const String DEBUG_CONDITIONAL_STRING = "DEBUG";
        }
    }
}
