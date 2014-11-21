using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Represents keyboard. Just call update() every tick once (at the beginning).
/// </summary>

namespace ViralTree
{
    public static class KInput
    {
        private static Keyboard.Key[] usedKeys;

        private static bool[] oldKeys;
        private static bool[] currentKeys;

        public static void Init(Keyboard.Key[] keys)
        {
            if (keys == null)
            {
                Keyboard.Key[] realKeys = new Keyboard.Key[(int)Keyboard.Key.KeyCount];

                for (int i = 0; i < realKeys.Length; i++)
                    realKeys[i] = (Keyboard.Key)i;

                usedKeys = realKeys;
            }

            else
                usedKeys = keys;

            oldKeys = new bool[(int)Keyboard.Key.KeyCount];
            currentKeys = new bool[(int)Keyboard.Key.KeyCount];
        }

        /// <summary>
        /// Checks every mouseButton and every used key (that were given within the constructor) if pressed.
        /// </summary>
        public static void Update()
        {
            foreach (Keyboard.Key key in usedKeys)
            {
                oldKeys[(int)key] = currentKeys[(int)key];
                currentKeys[(int)key] = Keyboard.IsKeyPressed(key);
            }
        }

        /// <summary>
        /// If the given key is clicked (pressed right NOW and NOT pressed last tick).
        /// </summary>
        /// <param name="key">Which key to be checked.</param>
        /// <returns>True if key is clicked, false otherwise.</returns>
        public static bool IsClicked(Keyboard.Key key)
        {
            return currentKeys[(int)key] && !oldKeys[(int)key];
        }

        /// <summary>
        /// If the given key is pressed right NOW.
        /// </summary>
        /// <param name="key">Which key to be checked.</param>
        /// <returns>True if the given key is pressed, false otherwise.</returns>
        public static bool IsPressed(Keyboard.Key key)
        {
            return Keyboard.IsKeyPressed(key);
        }

        /// <summary>
        /// Checks if the given key is released. (Not pressed right now and pressed last tick).
        /// </summary>
        /// <param name="key">Which key to be checked.</param>
        /// <returns></returns>
        public static bool IsReleased(Keyboard.Key key)
        {
            return oldKeys[(int)key] && !Keyboard.IsKeyPressed(key);
        }
    }

}