using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CindEngine.Levels
{
    public class Level
    {
        static List<Level> levels = new List<Level>();

        public string name;
        public int id;
        public int width;
        public int height;
        public Level()
        {
            id = levels.Count + 1;
            name = "Level " + id;
            levels.Add(this);
        }

        public Level(string name)
        {
            this.name = name;
            this.id = levels.Count + 1;
            levels.Add(this);
        }

        /// <summary>
        /// a function that is meant to be put into the Update()
        /// </summary>
        public virtual void LevelUpdate()
        {
            //
        }

        /// <summary>
        /// rendering for the level
        /// </summary>
        public virtual void LevelGUI(System.Drawing.Graphics graphics)
        {

        }

        /// <summary>
        /// run code when the level has loaded successfully
        /// </summary>
        public virtual void OnLoad()
        {
            Console.WriteLine("Loaded objects");
        }

        /// <summary>
        /// finds a level with the same name
        /// </summary>
        /// <param name="name">Level name</param>
        /// <returns>Level</returns>
        public static Level GetLevel(string name)
        {
            for (int i = 0; i < levels.Count; i++)
            {
                if (levels[i].name == name)
                {
                    return levels[i];
                }
            }

            return null;
        }

        /// <summary>
        /// loads a level
        /// </summary>
        /// <param name="level">Target level</param>
        /// <returns>level</returns>
        public static Level Load(Level level)
        {
            level.OnLoad();
            return level;
        }

        /// <summary>
        /// finds a level with the same ID
        /// </summary>
        /// <param name="id">Target level ID</param>
        /// <returns>Level</returns>
        public static Level GetLevel(int id)
        {
            for (int i = 0; i < levels.Count; i++)
            {
                if (levels[i].id == id)
                {
                    return levels[i];
                }
            }

            return null;
        }
    }
}
