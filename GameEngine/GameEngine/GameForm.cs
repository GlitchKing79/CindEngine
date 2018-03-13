using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Input;
using CindEngine.Renderer;

namespace CindEngine
{
    public class GameForm : Form
    {
        public static GameEngine gameClass;
        public Panel canvas;
        public GameForm(GameEngine game)
        {
            gameClass = game;
            this.canvas = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // canvas
            // 
            this.canvas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.canvas.Location = new System.Drawing.Point(0, 0);
            this.canvas.Name = "canvas";
            this.canvas.Size = new System.Drawing.Size(gameClass.width, gameClass.height);
            this.canvas.TabIndex = 0;
            this.canvas.Paint += new System.Windows.Forms.PaintEventHandler(this.canvas_Paint);
            // 
            // GameForm
            // 
            this.ClientSize = new System.Drawing.Size(gameClass.width, gameClass.height);
            this.Controls.Add(this.canvas);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "GameForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.GameForm_FormClosing);
            this.Load += new System.EventHandler(this.GameForm_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.GameForm_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.GameForm_KeyUp);
            canvas.Click += new EventHandler(this.Game_MousePress);
            this.ResumeLayout(false);
        }

        /// <summary>
        /// Graphics Rendering event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void canvas_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = canvas.CreateGraphics();
            gameClass.StartGraphics(g);
        }

        /// <summary>
        /// Simple Debug function
        /// </summary>
        /// <param name="str"></param>
        void Debug(string str)
        {
            System.Diagnostics.Debug.Print(str);
        }

        /// <summary>
        /// close event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GameForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            gameClass.StopGame();
        }

        /// <summary>
        /// game start event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GameForm_Load(object sender, EventArgs e)
        {
            AllocConsole();
            gameClass.GameInit();
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAsAttribute(UnmanagedType.Bool)]
        static extern bool AllocConsole();

        /// <summary>
        /// key down event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GameForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (!Input.INPUT_KEYS.Contains((InputKeys)e.KeyValue))
            {
                Input.INPUT_KEYS.Add((InputKeys)e.KeyValue);
                Input.INPUT_KEYS_DOWN.Add((InputKeys)e.KeyValue);
                Input.INPUT_KEYS_UP.Add((InputKeys)e.KeyValue);
            }
        }

        /// <summary>
        /// key up event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GameForm_KeyUp(object sender, KeyEventArgs e)
        {
            Input.INPUT_KEYS.Remove((InputKeys)e.KeyValue);
            Input.INPUT_KEYS_DOWN.Remove((InputKeys)e.KeyValue);
            Input.INPUT_KEYS_UP.Remove((InputKeys)e.KeyValue);
        }

        /// <summary>
        /// mouse press event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Game_MousePress(object sender, EventArgs e)
        {
            
                Input.MOUSE_LEFT_GAME = true;
                Input.MOUSE_LEFT_GUI = true;
            
            
        }
    }
}
