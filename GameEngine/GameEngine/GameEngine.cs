using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using CindEngine.Renderer;
using System.Drawing;

namespace CindEngine
{
    public class GameEngine : GraphicsEngine
    {
        public int width;
        public int height;

        

        Thread MainThread;


        public GraphicsEngine graphicsEngine;

        /// <summary>
        /// starts the graphics renderer
        /// </summary>
        /// <param name="graphics">Target graphics</param>
        public void StartGraphics(Graphics graphics)
        {
            drawHandle = graphics;
            graphicsEngine = this;

            graphicsEngine.GraphicsInit();
        }

        /// <summary>
        /// Get the graphics engine to use
        /// </summary>
        /// <param name="graphics"></param>
        /// <returns></returns>
        public virtual GraphicsEngine GetGraphicsEngnine(Graphics graphics, GameEngine game)
        {
            return new GraphicsEngine(graphics, this);
        }

        /// <summary>
        /// Initialises the games main loop
        /// </summary>
        public void GameInit()
        {
            MainThread = new Thread(new ThreadStart(UpdateThread));
            MainThread.Start();
        }

        /// <summary>
        /// The Main Update Loop
        /// </summary>
        public void UpdateThread()
        {
            long startTime = Environment.TickCount;
            while (true)
            {
                if (Environment.TickCount >= startTime + 1)
                {
                    if (assetsLoaded)
                    {
                        Update();
                    }
                    startTime = Environment.TickCount;
                }
            }
        }

        /// <summary>
        /// Runs code every 1 millisecond
        /// </summary>
        public virtual void Update()
        {
            Console.WriteLine("Empty Update");
        }

        /// <summary>
        /// Stops main update thread
        /// </summary>
        void GameStop()
        {
            MainThread.Abort();
        }

        /// <summary>
        /// stops the game
        /// </summary>
        public void StopGame()
        {
            graphicsEngine.GraphicsStop();
            GameStop();
        }

        public void Exit()
        {
            Environment.Exit(0);
            StopGame();
        }


    }
}
