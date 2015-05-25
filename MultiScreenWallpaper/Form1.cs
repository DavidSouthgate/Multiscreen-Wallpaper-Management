using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MultiScreenWallpaper
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Image img = new Bitmap(300, 100);
            Graphics g = Graphics.FromImage(img);

            Image imgLetterA = Image.FromFile("a.jpg");
            Image imgLetterB = Image.FromFile("b.jpg");
            Image imgLetterC = Image.FromFile("c.jpg");

            // Place a.gif
            g.DrawImage(imgLetterA, new Point(0, 0));

            // Place b.jpg
            g.DrawImage(imgLetterB, new Point(100, 0));

            // Place c.jpg
            g.DrawImage(imgLetterC, new Point(200, 0));

            img.Save("output.jpg");
        }
    }
}
