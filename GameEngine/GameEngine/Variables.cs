using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Input;
using CindEngine.Renderer;
namespace CindEngine
{
    public class Input
    {
        public static List<char> INPUT_KEYS = new List<char>();
        public static List<char> INPUT_KEYS_DOWN = new List<char>();
        public static List<char> INPUT_KEYS_UP = new List<char>();

        public static bool MOUSE_LEFT_GUI = false;
        public static bool MOUSE_LEFT_GAME = false;

        /// <summary>
        /// The current mous position on the window
        /// </summary>
        /// <returns>Mouse Position</returns>
        public static Vector MousePosition()
        {
            try
            {
                return new Vector(System.Windows.Forms.Cursor.Position.X - System.Windows.Forms.Form.ActiveForm.DesktopLocation.X, System.Windows.Forms.Cursor.Position.Y - System.Windows.Forms.Form.ActiveForm.DesktopLocation.Y);
            }
            catch (Exception)
            {
                return new Vector(0, 0);
            }
        }

        /// <summary>
        /// If a key has been pressed
        /// </summary>
        /// <param name="c">Target key</param>
        /// <returns>True or false</returns>
        public static bool GetKey(char c)
        {
            return INPUT_KEYS.Contains(c);
        }

        /// <summary>
        /// gets if a key has been pressed down
        /// </summary>
        /// <param name="c">Target key</param>
        /// <returns>True or false</returns>
        public static bool GetKeyDown(char c)
        {
            bool isKeyPressed = INPUT_KEYS_DOWN.Contains(c);
            INPUT_KEYS_DOWN.Remove(c);
            return isKeyPressed;
        }

        /// <summary>
        /// if a key has been pressed up
        /// </summary>
        /// <param name="c">Target key</param>
        /// <returns>True or false</returns>
        public static bool GetKeyUp(char c)
        {
            bool isKeyPressed = INPUT_KEYS_UP.Contains(c);
            INPUT_KEYS_UP.Remove(c);
            return isKeyPressed;
        }

        /// <summary>
        /// If the mouse is in the starget area
        /// </summary>
        /// <param name="bounds"></param>
        public static bool MouseEnter(Bounds bounds)
        {
            return (MousePosition().x > bounds.points[0].x && MousePosition().x < bounds.points[3].x && MousePosition().y > bounds.points[0].y && MousePosition().y < bounds.points[3].y);
        }

        /// <summary>
        /// if the mouse has clicked in a certain area
        /// </summary>
        /// <param name="bounds">Target Area</param>
        /// <returns>True or false</returns>
        public static bool MousePressedArea(Bounds bounds)
        {
            if (MouseEnter(bounds))
            {
                return MOUSE_LEFT_GAME;
            } else
            {
                return false;
            }
        }

        /// <summary>
        /// if the mouse has clicked in a certain area (Debug Version)
        /// </summary>
        /// <param name="bounds">Target area</param>
        /// <param name="g">Traget graphics</param>
        /// <returns>True or false</returns>
        public static bool MousePressedArea(Bounds bounds, System.Drawing.Graphics g)
        {
            g.DrawRectangle(new Pen(Color.Green), bounds.points[0].x, bounds.points[0].y, bounds.points[3].x, bounds.points[3].y);
            if (MouseEnter(bounds))
            {
                return MOUSE_LEFT_GUI;
            }
            else
            {
                return false;
            }
        }
    }
    public class Vector
    {
        public static Vector right = new Vector(1, 0);
        public static Vector left = new Vector(-1, 0);
        public static Vector up = new Vector(0, 1);
        public static Vector down = new Vector(0, -1); 

        public float x;
        public float y;
        public float magnitude;

        public override string ToString()
        {
            return String.Format("X={0}, Y={1}",this.x,this.y);
        }

        public static Vector operator+(Vector left, Vector right)
        {
            return new Vector(left.x + right.x, left.y + right.y);
        }

        public static Vector operator-(Vector left, Vector right)
        {
            return new Vector(left.x - right.x, left.y - right.y);
        }

        public static Vector operator*(Vector left, float f)
        {
            return new Vector(left.x * f, left.y * f);
        }

        public static Vector operator *(Vector left, Vector right)
        {
            return new Vector(left.x * right.x, left.y * right.y);
        }

        public static Vector operator/(Vector left, float f)
        {
            return new Vector(left.x / f, left.y / f);
        }

        public static Vector operator /(Vector left, Vector right)
        {
            return new Vector(left.x / right.x, left.y / right.y);
        }

        public Vector(float x, float y)
        {
            this.x = x;
            this.y = y;
            magnitude = GetMagnitude(x,y);
        }

        public Vector Rotate(float rotation)
        {
            rotation = rotation  * (float)(Math.PI/180);
            return new Vector((float)((x  * Math.Cos(rotation)) - (y * Math.Sin(rotation))), (float)((x * Math.Sin(rotation)) + (y * Math.Cos(rotation))));
        }

        public float GetRoation(Vector origin)
        {
            Vector v = this - origin;
            return (float)(Math.Atan2(v.y,v.x) * (Math.PI/180));
        }

        /// <summary>
        /// gets the maginitude of 2 points
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns>magnitude</returns>
        public static float GetMagnitude(float x, float y)
        {
            return (float)(Math.Sqrt((x * x) + (y * y)));
        }
        

        /// <summary>
        /// gives a normized vector the when x+y = 1
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public Vector GetNormal()
        {
            float Nx = (x != 0) ? x / magnitude : 0;
            float Ny = (y != 0) ? y / magnitude : 0;
            return new Vector(Nx, Ny);
        }

    }

    public class Bounds
    {
        public Vector[] points = new Vector[4];
        public static Bounds empty = new Bounds(new Vector[] { new Vector(0,0), new Vector(0, 0) , new Vector(0, 0) , new Vector(0, 0) });

        public static Bounds operator+(Bounds left, Bounds right)
        {
            Bounds b = Bounds.empty;
            for (int i = 0; i < 4; i++)
            {
                b.points[i] = left.points[i] + right.points[i];
            }
            return b;
        }

        public static Bounds operator -(Bounds left, Bounds right)
        {
            Bounds b = Bounds.empty;
            for (int i = 0; i < 4; i++)
            {
                b.points[i] = left.points[i] - right.points[i];
            }
            return b;
        }

        public static Bounds operator +(Bounds left, Vector right)
        {
            Bounds b = Bounds.empty;
            for (int i = 0; i < 4; i++)
            {
                b.points[i] = left.points[i] + right;
            }
            return b;
        }

        public static Bounds operator -(Bounds left, Vector right)
        {
            Bounds b = Bounds.empty;
            for (int i = 0; i < 4; i++)
            {
                b.points[i] = left.points[i] - right;
            }
            return b;
        }

        public Bounds(Vector[] points)
        {
                this.points = points;
        }

        public Bounds(float x1, float y1, float x2, float y2)
        {
            points[0] = new Vector(x1, y1);
            points[1] = new Vector(x2, y1);
            points[2] = new Vector(x2, y2);
            points[3] = new Vector(x1, y2);
        }

        public override string ToString()
        {
            return String.Format("{0}, {1}, {2}, {3}", points[0].ToString(), points[1].ToString(),points[2].ToString(),points[3].ToString());
        }
    }

    public enum CollisionType
    {
        left,right,up,down
    }


    public class Collision
    {
        static List<Collision> allCollisionObjects = new List<Collision>();

        /// <summary>
        /// gets all created collisions
        /// </summary>
        /// <returns>All Colliders</returns>
        public static Collision[] GetAllCollsions()
        {
            return allCollisionObjects.ToArray();
        }
        
        public Bounds collisionBounds = Bounds.empty;
        public GameObject holder;
        public bool canCollied = true;
        public Collision(Bounds boundary, GameObject holder)
        {
            this.collisionBounds = boundary;
            this.holder = holder;
            allCollisionObjects.Add(this);
        }

        /// <summary>
        /// checks if the collider has collided with another object in a selected area
        /// </summary>
        /// <param name="direction">detection direction</param>
        /// <returns>True or false</returns>
        public bool OnCollision(CollisionType direction)
        {
            if (direction == CollisionType.right)
            {
                return OnCollision()[0];
            }
            else if (direction == CollisionType.left)
            {
                return OnCollision()[1];
            }
            else if (direction == CollisionType.down)
            {
                return OnCollision()[2];
            }
            else if (direction == CollisionType.up)
            {
                return OnCollision()[3];
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// gets the collision of the collider
        /// </summary>
        /// <returns>Collider</returns>
        public Collision GetCollision()
        {
            Collision[] colliders = GetAllCollsions();
            for (int i = 0; i < colliders.Length; i++)
            {
                if (holder != colliders[i].holder && colliders[i].canCollied)
                {
                    if (collisionBounds != Bounds.empty && colliders[i].collisionBounds != Bounds.empty)
                    {
                        float rot1 = holder.rotation;
                        Vector v1 = new Vector(GameForm.gameClass.width, GameForm.gameClass.height);
                        Vector v2 = new Vector(0, 0);

                        float rot2 = colliders[i].holder.rotation;
                        Vector v3 = new Vector(GameForm.gameClass.width, GameForm.gameClass.height);
                        Vector v4 = new Vector(0, 0);

                        for (int calcPoints = 0; calcPoints < collisionBounds.points.Length; calcPoints++)
                        {
                            if (collisionBounds.points[calcPoints].Rotate(rot1).x < v1.x)
                            {
                                v1.x = collisionBounds.points[calcPoints].Rotate(rot1).x;
                            }
                            if (collisionBounds.points[calcPoints].Rotate(rot1).y < v1.y)
                            {
                                v1.y = collisionBounds.points[calcPoints].Rotate(rot1).y;
                            }

                            if (collisionBounds.points[calcPoints].Rotate(rot1).x > v2.x)
                            {
                                v2.x = collisionBounds.points[calcPoints].Rotate(rot1).x;
                            }
                            if (collisionBounds.points[calcPoints].Rotate(rot1).y > v2.y)
                            {
                                v2.y = collisionBounds.points[calcPoints].Rotate(rot1).y;
                            }
                            //
                            if (colliders[i].collisionBounds.points[calcPoints].Rotate(rot2).x < v3.x)
                            {
                                v3.x = colliders[i].collisionBounds.points[calcPoints].Rotate(rot2).x;
                            }
                            if (colliders[i].collisionBounds.points[calcPoints].Rotate(rot2).y < v3.y)
                            {
                                v3.y = colliders[i].collisionBounds.points[calcPoints].Rotate(rot2).y;
                            }

                            if (colliders[i].collisionBounds.points[calcPoints].Rotate(rot2).x > v4.x)
                            {
                                v4.x = colliders[i].collisionBounds.points[calcPoints].Rotate(rot2).x;
                            }
                            if (colliders[i].collisionBounds.points[calcPoints].Rotate(rot2).y > v4.y)
                            {
                                v4.y = colliders[i].collisionBounds.points[calcPoints].Rotate(rot2).y;
                            }
                        }

                        int x1a = (int)Math.Round(holder.position.x + v1.x);
                        int x1b = (int)Math.Round(colliders[i].holder.position.x + v3.x);
                        int x2a = (int)Math.Round(holder.position.x + v2.x);
                        int x2b = (int)Math.Round(colliders[i].holder.position.x + v4.x);

                        int y1a = (int)Math.Round(holder.position.y + v1.y);
                        int y1b = (int)Math.Round(colliders[i].holder.position.y + v3.y);
                        int y2a = (int)Math.Round(holder.position.y + v2.y);
                        int y2b = (int)Math.Round(colliders[i].holder.position.y + v4.y);
                        
                        if (x1a >= x1b && x1a <= x2b)
                        {
                            if (y1a >= y1b && y1a <= y2b)
                            {
                                return colliders[i];
                            }
                        }
                        if (x2a >= x1b && x2a <= x2b)
                        {
                            if (y2a >= y1b && y2a <= y2b)
                            {
                                return colliders[i];
                            }
                        }
                        //
                        if (x1b >= x1a && x1b <= x2a)
                        {
                            if (y1b >= y1a && y1b <= y2a)
                            {
                                return colliders[i];
                            }
                        }
                        if (x2b >= x1a && x2b <= x2a)
                        {
                            if (y2b >= y1a && y2b <= y2a)
                            {
                                return colliders[i];
                            }
                        }

                    }
                }
            }
            return null;
        }

        public int[] objectPoints;
        public int[] otherPoints;

        /// <summary>
        /// Checks the collision bounds
        /// </summary>
        /// <returns>an array of booleans of what side has collided</returns>
        bool[] OnCollision()
        {
            bool[] collided = new bool[4];
            Collision[] colliders = GetAllCollsions();
            for (int i = 0; i < colliders.Length; i++)
            {
                if (holder != colliders[i].holder)
                {
                    if (collisionBounds != Bounds.empty && colliders[i].collisionBounds != Bounds.empty)
                    {
                        float rot1 = holder.rotation;
                        Vector v1 = new Vector(GameForm.gameClass.width, GameForm.gameClass.height);
                        Vector v2 = new Vector(0, 0);

                        float rot2 = colliders[i].holder.rotation;
                        Vector v3 = new Vector(GameForm.gameClass.width, GameForm.gameClass.height);
                        Vector v4 = new Vector(0, 0);

                        for (int calcPoints = 0; calcPoints < collisionBounds.points.Length; calcPoints++)
                        {
                            if (collisionBounds.points[calcPoints].Rotate(rot1).x < v1.x)
                            {
                                v1.x = collisionBounds.points[calcPoints].Rotate(rot1).x;
                            }
                            if (collisionBounds.points[calcPoints].Rotate(rot1).y < v1.y)
                            {
                                v1.y = collisionBounds.points[calcPoints].Rotate(rot1).y;
                            }

                            if (collisionBounds.points[calcPoints].Rotate(rot1).x > v2.x)
                            {
                                v2.x = collisionBounds.points[calcPoints].Rotate(rot1).x;
                            }
                            if (collisionBounds.points[calcPoints].Rotate(rot1).y > v2.y)
                            {
                                v2.y = collisionBounds.points[calcPoints].Rotate(rot1).y;
                            }
                            //
                            if (colliders[i].collisionBounds.points[calcPoints].Rotate(rot2).x < v3.x)
                            {
                                v3.x = colliders[i].collisionBounds.points[calcPoints].Rotate(rot2).x;
                            }
                            if (colliders[i].collisionBounds.points[calcPoints].Rotate(rot2).y < v3.y)
                            {
                                v3.y = colliders[i].collisionBounds.points[calcPoints].Rotate(rot2).y;
                            }

                            if (colliders[i].collisionBounds.points[calcPoints].Rotate(rot2).x > v4.x)
                            {
                                v4.x = colliders[i].collisionBounds.points[calcPoints].Rotate(rot2).x;
                            }
                            if (colliders[i].collisionBounds.points[calcPoints].Rotate(rot2).y > v4.y)
                            {
                                v4.y = colliders[i].collisionBounds.points[calcPoints].Rotate(rot2).y;
                            }
                        }

                        int x1a = (int)Math.Round(holder.position.x + v2.x);
                        int x1b = (int)Math.Round(colliders[i].holder.position.x + v4.x);
                        int x2a = (int)Math.Round(holder.position.x + v1.x);
                        int x2b = (int)Math.Round(colliders[i].holder.position.x + v3.x);

                        int y1a = (int)Math.Round(holder.position.y + v2.y);
                        int y1b = (int)Math.Round(colliders[i].holder.position.y + v4.y);
                        int y2a = (int)Math.Round(holder.position.y + v1.y);
                        int y2b = (int)Math.Round(colliders[i].holder.position.y + v3.y);

                        objectPoints = new int[] { x2a, y2a, x1a, y1a };
                        otherPoints = new int[] { x2b, y2b, x1b, y1b };


                        if (objectPoints[2] == otherPoints[0])
                        {
                            if (objectPoints[1] >= otherPoints[1] && objectPoints[1] <= otherPoints[3])
                            {
                                Console.WriteLine("Normal Collision 1");
                                collided[0] = true;
                            } else if (objectPoints[1] + (objectPoints[3] - objectPoints[1]) <= otherPoints[1] && objectPoints[1] - (objectPoints[3] - objectPoints[1]) <= otherPoints[3])
                            {
                                Console.WriteLine("Half Collision 1");
                                collided[0] = true;
                            }
                        }
                        //
                        if (objectPoints[0] == otherPoints[2])
                        {
                            if (objectPoints[1] >= otherPoints[1] && objectPoints[1] <= otherPoints[3])
                            {
                                Console.WriteLine("Normal Collision 2");
                                collided[1] = true;
                            }
                            else if (objectPoints[1] + (objectPoints[3] - objectPoints[1])/4 <= otherPoints[1] && objectPoints[1] - (objectPoints[3] - objectPoints[1])/4 >= otherPoints[3])
                            {
                                Console.WriteLine("Half Collision 2");
                                collided[1] = true;
                            }
                        }
                    }
                }
            }
            return collided;
        }
    }

    public enum PrimitiveType
    {
        rectangle, eclipse, triangle
    }

    public class GameObject
    {
        public string name;
        public Bitmap image;
        public Color color = Color.White;
        public Vector position;
        public Collision collider;
        public string tag = "default";
        private int layer = 0;
        public float rotation = 0;

        public static int topLayer = 0;
        static List<GameObject> allObjects = new List<GameObject>();

        public override string ToString()
        {
            return String.Format("Name= {0}, Position={1}",this.name,this.position.ToString());
        }

        public GameObject(string name, Bitmap image, Vector position)
        {
            this.name = name;
            this.image = image;
            this.position = position;
            collider = new Collision(Bounds.empty, this);
            allObjects.Add(this);
        }

        public GameObject(string name, Bitmap image, Vector position, Bounds collisionBox)
        {
            this.name = name;
            this.image = image;
            this.position = position;
            collider = new Collision(collisionBox, this);
            allObjects.Add(this);
        }

        public GameObject(string name, PrimitiveType type, Vector position, Bounds collisionBox)
        {
            this.name = name;
            if (type == PrimitiveType.rectangle)
            {
                Bitmap bitmap = new Bitmap((int)collisionBox.points[2].x, (int)collisionBox.points[2].y);
                for (int pixX = 0; pixX < collisionBox.points[2].x; pixX++)
                {
                    for (int pixY = 0; pixY < collisionBox.points[2].y; pixY++)
                    {
                        bitmap.SetPixel(pixX, pixY, color);
                    }
                }
                this.image = bitmap;
            }
            
            this.position = position;
            collider = new Collision(collisionBox, this);
            allObjects.Add(this);
        }

        /// <summary>
        /// Finds the first GameObject with the same name
        /// </summary>
        /// <param name="name">Name of object</param>
        /// <returns></returns>
        public static GameObject FindGameObject(string name)
        {
            for (int i = 0; i < allObjects.Count; i++)
            {
                if (allObjects[i].name == name)
                {
                    return allObjects[i];
                }
            }
            return null;
        }

        /// <summary>
        /// Returns a array of all the created GameObjects
        /// </summary>
        /// <returns>GameObject[]</returns>
        public static GameObject[] GetAllGameObjects()
        {
            return allObjects.ToArray();
        }

        /// <summary>
        /// Destroys GameObject and runs code
        /// </summary>
        /// <param name="onDestroy">Code to run on destroy</param>
        public void Destroy(Action onDestroy)
        {
            onDestroy.Invoke();
            Destroy();
        }

        /// <summary>
        /// Destroys GameObject
        /// </summary>
        public void Destroy()
        {
            allObjects.Remove(this);
            image.Dispose();
            position = new Vector(0, 0);
            collider = new Collision(Bounds.empty, this);
            name = "";
        }

        /// <summary>
        /// set the objects layer
        /// </summary>
        /// <param name="layer">new layer</param>
        public void Setlayer(int layer)
        {
            this.layer = layer;
            if (layer > topLayer)
            {
                topLayer = layer;
            }
        }

        /// <summary>
        /// get te object layer
        /// </summary>
        /// <returns></returns>
        public int Getlayer()
        {
            return this.layer;
        }
    }
}
