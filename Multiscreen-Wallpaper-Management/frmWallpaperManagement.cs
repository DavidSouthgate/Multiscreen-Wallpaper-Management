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

        //USED TO SET WALLPAPER
        public void SetDWallpaper(string path)
        {

            //Set wallpaper to image at given path
            SystemParametersInfo(SPI_SETDESKWALLPAPER, 0, path, SPIF_UPDATEINIFILE);
        }

        public frmWallpaperManagement()
        {
            
            InitializeComponent();

            //Add event to run when display settings change
            Microsoft.Win32.SystemEvents.DisplaySettingsChanged += new EventHandler(ScreenHandler);
        }

        //RUNS WHEN DISPLAY SETTINGS CHANGE
        private void ScreenHandler(object sender, EventArgs e)
        {

            //Generates the wallpaper and applies it
            loadWallpaper();
        }

        //CALCULATE TOTAL WALLPAPER SIZE
        private void wallpaperTotalSize(ref int wallpaperTotalWidth, ref int wallpaperTotalHeight, configClass config)
        {
            foreach (var displayScreen in Screen.AllScreens)
            {
                wallpaperTotalWidth = wallpaperTotalWidth + displayScreen.Bounds.Width;

                foreach (var configScreen in config.screens)
                {
                
                    if(configScreen.name == displayScreen.DeviceName)
                    {
                        if(displayScreen.Bounds.Height + configScreen.padding_top > wallpaperTotalHeight)
                        {
                            wallpaperTotalHeight = displayScreen.Bounds.Height + configScreen.padding_top;
                        }
                    }
                }
            }
        }

        //GETS THE GREATEST COMMON DIVISOR OF A AND B
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

        //CLASS USED TO STORE SCREEN CONFIG
        public class configScreenClass
        {
            public string name { get; set; }
            public List<string> wallpaper { get; set; }
            public int padding_top { get; set; }
        }

        //CLASS USED TO STORE MISC CONFIG
        public class configMiscClass
        {
            public string name { get; set; }
            public string wallpaper { get; set; }
            public int padding_top { get; set; }
        }

        //CLASS USED TO STORE CONFIG
        public class configClass
        {
            public List<configScreenClass> screens { get; set; }
            public List<configMiscClass> misc { get; set; }
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

            //If the config.json file exists
            if (File.Exists(Application.StartupPath + @"\config.json"))
            {
                int i;                          //Declare variable used for an index
                int wallpaperTotalWidth = 0;    //Declare variable used for total wallpaper width
                int wallpaperTotalHeight = 0;   //Declare variable used for total wallpaper height
                string sJson = "";              //Declare variable used to store json from config
                var config = new configClass(); //Declare variable used to store configuration
                StreamReader streamReaderJson;  //Declare variable used to open config

                //Read config
                streamReaderJson = new StreamReader(Application.StartupPath + @"\config.json");

                //Stroe contents of config.json in variable
                sJson = streamReaderJson.ReadToEnd();

                //Attempt to parse string
                try
                {

                    //Parse json config
                    config = JsonConvert.DeserializeObject<configClass>(sJson);
                }

                //If parsing fails
                catch
                {

                    //Parse blank config
                    config = JsonConvert.DeserializeObject<configClass>("[]");
                }

                //Get total wallpaper size
                wallpaperTotalSize(ref wallpaperTotalWidth, ref wallpaperTotalHeight, config);

                var imgwallpapers = new List<Image>();      //Declare variable for storing the wallpaper images

                //Reset index counter
                i = 0;

                //Loop for every wallpaper file
                foreach (var screens in config.screens)
                {

                    Random randomNumber = new Random();                         //Declare variable used to generate random number for random wallpaper
                    int wallpaperCount = screens.wallpaper.Count;               //Declare integer and store wallpaper count in it
                    int wallpaperRandom = randomNumber.Next(0, wallpaperCount); //Generate a random number for random wallpaper

                    //If image file exists
                    if (File.Exists(screens.wallpaper[wallpaperRandom]))
                    {

                        //Store wallpaper image to variable
                        imgwallpapers.Add(Image.FromFile(screens.wallpaper[wallpaperRandom]));
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
                foreach (var configScreen in config.screens)
                {

                    //Loop for every screen
                    foreach (var displayScreen in Screen.AllScreens)
                    {

                        //If the name of the screen is equal to the X screen from config
                        if (configScreen.name == displayScreen.DeviceName)
                        {

                            //Loof for every wallpaper image
                            foreach (var wallpaper in imgwallpapers)
                            {

                                //If the aspect ration of the wallpaper is equal to the aspect ratio of the screen resolution
                                if(aspectRatio(displayScreen.Bounds.Width, displayScreen.Bounds.Height) == aspectRatio(wallpaper.Width, wallpaper.Height))
                                {
                                    //Add wallpaper image to template
                                    gWallpaper.DrawImage(wallpaper, new Rectangle(screenWidthUsed, configScreen.padding_top, displayScreen.Bounds.Width, displayScreen.Bounds.Height));

                                }
                            }

                            //Add screen width to the screen width used
                            screenWidthUsed = screenWidthUsed + displayScreen.Bounds.Width;
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
            ////////////Opacity = 0;////////////////////////////////////////////////////////////////////////////////////////

            //Generates the wallpaper and applies it
            loadWallpaper();
        }

        //FORM DISPLAYED
        private void Form_Shown(object sender, EventArgs e)
        {

            //Make form invisible
            ////////////Visible = false;////////////////////////////////////////////////////////////////////////////////////
            ////////////Opacity = 100;//////////////////////////////////////////////////////////////////////////////////////
        }
    }
}
