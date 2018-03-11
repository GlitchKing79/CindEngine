using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
namespace CindEngine
{
    public class Asset
    {
        /// <summary>
        /// loads an image from a file location
        /// </summary>
        /// <param name="location">File Location</param>
        /// <returns></returns>
        public static Bitmap LoadImage(string location)
        {
            return new Bitmap(location);
        }

        public static string LocalDataPath()
        {
            return System.Windows.Forms.Application.StartupPath;
        }

        public static Image LoadAnimation(string location)
        {
            return Image.FromFile(location);
        }
    }
}
