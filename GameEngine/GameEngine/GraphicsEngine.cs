using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Threading;
namespace CindEngine.Renderer
{
    public class GraphicsEngine
    {

        public GameEngine game;

        public static Graphics drawHandle;
        private Thread renderThread;
        public bool assetsLoaded = false;
        public GraphicsEngine(Graphics g, GameEngine game)
        {
            drawHandle = g;
            this.game = game;
        }

        public GraphicsEngine()
        {

        }

        /// <summary>
        /// loads all assets
        /// </summary>
        public virtual void LoadAssets()
        {
            Console.WriteLine("Loading Assets");
        }

        /// <summary>
        /// starts the graphics engine and loads assets
        /// </summary>
        public void GraphicsInit()
        {
            drawHandle.DrawString("Loading", new Font(new FontFamily(System.Drawing.Text.GenericFontFamilies.Monospace), 24), new SolidBrush(Color.Black), 0, 0);

            LoadAssets();
            assetsLoaded = true;
            renderThread = new Thread(new ThreadStart(Render));
            renderThread.Start();
        }

        /// <summary>
        /// stops the graphics engine
        /// </summary>
        public void GraphicsStop()
        {
            renderThread.Abort();
        }
        public Bitmap frame;
        public Graphics frameGraphics;

        public Color backColor = Color.GhostWhite;
        /// <summary>
        /// Render loop
        /// </summary>
        private void Render()
        {
            frame = new Bitmap(game.width, game.height);
            frameGraphics = Graphics.FromImage(frame);
            while (true)
            {
                    frameGraphics.FillRectangle(new SolidBrush(backColor), 0, 0, game.width, game.height);
                    GUI();
                    drawHandle.DrawImage(frame, 0, 0);
            }
        }

        /// <summary>
        /// code that gets run in the render loop
        /// </summary>
        public virtual void GUI()
        {
            RenderAllObjectsNormaly(frameGraphics);
        }

        /// <summary>
        /// Renders the target GameObject
        /// </summary>
        /// <param name="graphics">Target Graphics</param>
        /// <param name="gameobject">Target GameObject</param>
        public static void RenderObject(Graphics graphics, GameObject gameobject)
        {
                if (gameobject.collider.collisionBounds != Bounds.empty)
                {
                    if (gameobject.rotation > 360)
                    {
                        gameobject.rotation = 0;
                    }
                    if (gameobject.rotation < 0)
                    {
                        gameobject.rotation = 360;
                    }
                    PointF[] points = new PointF[3];
                    for (int i = 0; i < 3; i++)
                    {
                        int x;
                        if (i == 2)
                        {
                            x = 3;
                        }
                        else if (i == 3)
                        {
                            x = 1;
                        }
                        else
                        {
                            x = i;
                        }
                        Vector v = gameobject.collider.collisionBounds.points[x].Rotate(gameobject.rotation); //- gameobject.collider.collisionBounds.points[2].Rotate(gameobject.rotation) / 2;
                        points[i] = new PointF(gameobject.position.x + v.x, gameobject.position.y + v.y);

                    }
                    graphics.DrawImage(gameobject.image, points);
                }
                else
                {
                    graphics.DrawImage(gameobject.image, gameobject.position.x, gameobject.position.y);
                }
        }

        /// <summary>
        /// renders and event handels a button with text
        /// </summary>
        /// <param name="label">Message</param>
        /// <param name="graphics">Target Graphics</param>
        /// <param name="color">Button Color</param>
        /// <param name="position">Button position</param>
        /// <param name="bounds">Rectangle of the button</param>
        /// <param name="code">On Click Event</param>
        public static void RenderButton(string label,Graphics graphics, Color color, Vector position, Bounds bounds, Action code)
        {
            try
            {
                RenderButton(graphics, color, position, bounds, code);
                graphics.DrawString(label, new Font(new FontFamily(System.Drawing.Text.GenericFontFamilies.Monospace), 12), new SolidBrush(Color.Black), position.x + bounds.points[2].x / 10, position.y + bounds.points[2].y / 10);

            }
            catch {}
            
        }
        /// <summary>
        /// renders and event handels a button
        /// </summary>
        /// <param name="graphics">Target Graphics</param>
        /// <param name="color">Button Color</param>
        /// <param name="position">Button position</param>
        /// <param name="bounds">Rectangle of the button</param>
        /// <param name="code">On Click Event</param>
        public static void RenderButton(Graphics graphics,Color color,Vector position, Bounds bounds, Action code)
        {
                graphics.FillRectangle(new SolidBrush(color), position.x + bounds.points[0].x, position.y + bounds.points[0].y, bounds.points[2].x, bounds.points[2].y);
                if (Input.MousePressedArea(bounds + position, graphics))
                {
                    code.Invoke();
                }
        }

        /// <summary>
        /// Renders a box
        /// </summary>
        /// <param name="graphics">Target Graphics</param>
        /// <param name="color">Box color</param>
        /// <param name="position">Box position</param>
        /// <param name="bounds">Rectangle of the box</param>
        public static void RenderBox(Graphics graphics, Color color, Vector position, Bounds bounds)
        {
                graphics.FillRectangle(new SolidBrush(color), position.x + bounds.points[0].x, position.y, bounds.points[2].x, bounds.points[2].y);
        }

        /// <summary>
        /// Renders the collision box of a GameObject
        /// </summary>
        /// <param name="graphics">Target graphics</param>
        /// <param name="gameobject">Target GameObject</param>
        public static void RenderCollisionBox(Graphics graphics, GameObject gameobject)
        {
            Vector pos = gameobject.position;
            int[] v = gameobject.collider.objectPoints;
            if (v == null)
            {
                return;
            }
            Pen p = new Pen(Color.Green);
            PointF[] poly = new PointF[8];

            poly[0] = new PointF(v[0], v[1]);
            poly[1] = new PointF(v[2] - (v[2] - v[0]) / 2, v[1] - 10);
            poly[2] = new PointF(v[2], v[1]);
            poly[3] = new PointF(v[2] + 10, v[3] - (v[3] - v[1]) / 2);
            poly[4] = new PointF(v[2], v[3]);
            poly[5] = new PointF(v[0] + (v[2] - v[0]) / 2, v[3] + 10);
            poly[6] = new PointF(v[0], v[3]);
            poly[7] = new PointF(v[0] - 10, v[3] - (v[3] - v[1]) / 2);


            graphics.DrawPolygon(p, poly);

        }

        /// <summary>
        /// renders all objects created at their correct layer
        /// </summary>
        public static void RenderAllObjectsNormaly(Graphics graphics)
        {
            GameObject[] allObjects = GameObject.GetAllGameObjects();
            for (int i = 0; i <= GameObject.topLayer; i++)
            {
                for (int x = 0; x < allObjects.Length; x++)
                {
                    if (allObjects[x].Getlayer() == i)
                    {
                        RenderObject(graphics, allObjects[x]);
                        RenderCollisionBox(graphics, allObjects[x]);
                    }
                }
            }
        }
        
    }
}
