using SFML.Graphics;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViralTree.Template
{
    /// <summary>
    /// Abstract class that runs the basic game, such as setting up a window, event polling, game looping...
    /// </summary>
    public abstract class AbstractGame
    {
        public RenderWindow window;
        public GameTime gameTime;

        public double frameRate;

        protected int wheelDelta;

        /// <summary>
        /// Creating a window with the given parameter. Note: you can change everything in derived class aswell.
        /// </summary>
        /// <param name="width">Width of the window.</param>
        /// <param name="height">Height of the window.</param>
        /// <param name="title">Title of the window.</param>
        /// <param name="style">Window style, e.g. fullscreen, default, resizable, not resizable..</param>
        protected AbstractGame()
        {
            CreateWindow();

            Settings.ResizedEvent += ResizeHandler;
            Settings.FrameRateEvent += FrameRateHandler;
            Settings.StyleEvent += StyleHandler;

            gameTime = new GameTime();
        }

        private void CreateWindow()
        {
            window = new RenderWindow(
                            new VideoMode((uint)Settings.WindowSize.X, (uint)Settings.WindowSize.Y),
                            Settings.Constants.WINDOW_TITLE,
                            Settings.WindowStyles);

            //window.SetVerticalSyncEnabled(false);
            //window.SetMouseCursorVisible(true);
            window.Closed += CloseHandler;
            window.MouseWheelMoved += MouseWheelHandler;

            window.SetFramerateLimit(Settings.Fps);
        }

        private void StyleHandler()
        {
            if (window != null && window.IsOpen())
                window.Close();
            
            CreateWindow();
        }

        private void FrameRateHandler()
        {
            window.SetFramerateLimit(Settings.Fps);
        }

        private void ResizeHandler()
        {
            this.window.Size = new Vector2u((uint)Settings.WindowSize.X, (uint)Settings.WindowSize.Y);
        }


        /// <summary>
        /// Handler for the MouseWheelEvent, just reading the mouse wheel delta.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MouseWheelHandler(object sender, MouseWheelEventArgs e)
        {
            wheelDelta = e.Delta;
        }

        /// <summary>
        /// Event handler for closing the window (aka clicking the "x").
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseHandler(object sender, EventArgs e)
        {
            window.Close();
        }

        /// <summary>
        /// Start your game with this method. 
        /// Starts the gametime, keeps the window open, dispatches the window events.
        /// Updates the gametime and the derived class aswell. Calls the draw method of the derived class and displays the window.
        /// </summary>
        public void Run()
        {
            gameTime.Start();

            while (window.IsOpen())
            {
                window.DispatchEvents();
                gameTime.Update();

                Update(gameTime);
                Draw(gameTime, window);

                wheelDelta = 0;
                window.Display();

                frameRate = 1.0 / gameTime.ElapsedTime.TotalSeconds;
            }
        }

        /// <summary>
        /// Called every frame once, put your game logic here.
        /// </summary>
        /// <param name="gameTime">The gametime (e.g. elapsed time).</param>
        protected abstract void Update(GameTime gameTime);

        /// <summary>
        /// Called every frame once. Put your draw calls here.
        /// </summary>
        /// <param name="gameTime">The gametime (e.g. elapsed time).</param>
        /// <param name="window">The window where to be drawn to.</param>
        protected abstract void Draw(GameTime gameTime, RenderWindow window);
    }
}
