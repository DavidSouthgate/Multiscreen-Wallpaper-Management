/* The MIT License (MIT)
 * 
 * Copyright (c) 2015 David Southgate
 * github.com/DavidSouthgate/Multiscreen-Wallpaper-Management
 * dav@davidsouthgate.co.uk
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:

 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.

 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;
using Newtonsoft.Json;

namespace MultiScreenWallpaper
{
    public partial class frmWallpaperManagement : Form
    {
        [DllImport("user32.dll")]
        private static extern bool SystemParametersInfo(uint uiAction, uint uiParam, string pvParam, uint fWinIni);
        const uint SPI_SETDESKWALLPAPER = 0x14;
        const uint SPIF_UPDATEINIFILE = 0x01;
        string appPath = Path.GetDirectoryName(Application.ExecutablePath);

        public void SetDWallpaper(string path)
        {
            SystemParametersInfo(SPI_SETDESKWALLPAPER, 0, path, SPIF_UPDATEINIFILE);
        }

        public frmWallpaperManagement()
        {
            
            InitializeComponent();
            Microsoft.Win32.SystemEvents.DisplaySettingsChanged += new EventHandler(ScreenHandler);
        }

        private void ScreenHandler(object sender, EventArgs e)
        {
            loadWallpaper();
        }

        private void wallpaperTotalSize(ref int wallpaperTotalWidth, ref int wallpaperTotalHeight)
        {
            foreach (var screen in Screen.AllScreens)
            {
                wallpaperTotalWidth = wallpaperTotalWidth + screen.Bounds.Width;

                if (screen.Bounds.Height > wallpaperTotalHeight)
                {
                    wallpaperTotalHeight = screen.Bounds.Height;
                }
            }
        }

        //Gets the greatest common divisor of a and b
        static int GCD(int a, int b)
        {
            int Remainder;
            while (b != 0)
            {
                Remainder = a % b;
                a = b;
                b = Remainder;
            }
            return a;
        }

        //CALCULATE ASPECT RATIO FROM WIDTH AND HEIGHT
        private string aspectRatio(int x,   //Width of image
                                   int y)   //Height of image
        {

            //Return aspect ratio
            return string.Format("{0}:{1}", x / GCD(x, y), y / GCD(x, y));
        }

        //GENERATES THE WALLPAPER AND APPLIES IT
        private void loadWallpaper()
        {
            int i;                          //Declare variable used for an index
            int wallpaperTotalWidth = 0;    //Declare variable used for total wallpaper width
            int wallpaperTotalHeight = 0;   //Declare variable used for total wallpaper height
            string sJson = "";              //Declare variable used to store json from config

            //Get total wallpaper size
            wallpaperTotalSize(ref wallpaperTotalWidth, ref wallpaperTotalHeight);

            //Declare list used for storing config
            List<string>[] config = new List<string>[2];

            //If the config.json file exists
            if (File.Exists(Application.StartupPath + @"\config.json"))
            {
                //Declare variable used to open config
                StreamReader streamReaderJson;

                //Read config
                streamReaderJson = new StreamReader(Application.StartupPath + @"\config.json");

                //Stroe contents of config.json in variable
                sJson = streamReaderJson.ReadToEnd();

                //Attempt to parse string
                try
                {

                    //Parse json config
                    config = JsonConvert.DeserializeObject<List<string>[]>(sJson);
                }

                //If parsing fails
                catch
                {

                    //Parse blank config
                    config = JsonConvert.DeserializeObject<List<string>[]>("[]");
                }

                var screenNames = new List<string>();       //Declare variable for storing screen names from config
                var wallpaperFiles = new List<string>();    //Declare variable for storing wallpaper file names from config
                screenNames = config[0];                    //Store wallpaper files from config
                wallpaperFiles = config[1];                 //Store screen names from config
                var imgwallpapers = new List<Image>();      //Declare variable for storing the wallpaper images

                //Reset index counter
                i = 0;

                //Loop for every wallpaper file
                foreach (var nul in wallpaperFiles)
                {

                    //If image file exists
                    if (File.Exists(wallpaperFiles[i]))
                    {

                        //Store wallpaper image to variable
                        imgwallpapers.Add(Image.FromFile(wallpaperFiles[i]));
                    }

                    //Add one to index
                    i++;
                }

                //Create wallpaper template
                Image imgWallpaper = new Bitmap(wallpaperTotalWidth, wallpaperTotalHeight);

                //Convert wallpaper template to graphic
                Graphics gWallpaper = Graphics.FromImage(imgWallpaper);

                //Declare variable used for tracking width used
                int screenWidthUsed = 0;

                //Loop for ever screen name
                foreach (var screenName in screenNames)
                {

                    //Loop for every screen
                    foreach (var screen in Screen.AllScreens)
                    {

                        //If the name of the screen is equal to the X screen from config
                        if (screenName == screen.DeviceName)
                        {

                            //Loof for every wallpaper image
                            foreach (var wallpaper in imgwallpapers)
                            {

                                //If the aspect ration of the wallpaper is equal to the aspect ratio of the screen resolution
                                if(aspectRatio(screen.Bounds.Width, screen.Bounds.Height) == aspectRatio(wallpaper.Width, wallpaper.Height))
                                {
                                    //Add wallpaper image to template
                                    gWallpaper.DrawImage(wallpaper, new Rectangle(screenWidthUsed, 0, screen.Bounds.Width, screen.Bounds.Height));

                                }
                            }

                            //Add screen width to the screen width used
                            screenWidthUsed = screenWidthUsed + screen.Bounds.Width;
                        }
                    }
                }

                //Save wallpaper
                imgWallpaper.Save("wallpaper.png");

                //Set wallpaper
                SetDWallpaper(appPath + "/wallpaper.png");

                //Close the config file
                streamReaderJson.Close();

                //Set index to 0
                i = 0;

                //Loop for every image in imgwallpapers
                foreach(var nul in imgwallpapers)
                {

                    //Dispose image stored
                    imgwallpapers[i].Dispose();

                    //Add one to index
                    i++;
                }
            }

            else
            {

                //Display error message when no config exists
                MessageBox.Show("Wallpaper Management: Config Not Found");

                //Close the program
                this.Close();
            }
        }

        //FORM LOADED
        private void Form_Load(object sender, EventArgs e)
        {

            //Make form invisible
            Opacity = 0;

            //Generates the wallpaper and applies it
            loadWallpaper();
        }

        //FORM DISPLAYED
        private void Form_Shown(object sender, EventArgs e)
        {

            //Make form invisible
            Visible = false;
            Opacity = 100;
        }
    }
}
