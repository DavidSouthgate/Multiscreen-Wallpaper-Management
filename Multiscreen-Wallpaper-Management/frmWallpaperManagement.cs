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

            ///////////////////////////////////////////////////////////////////
            //Hacked in at the momment. Without this SendMessages don't arrive
            Opacity = 0;
            this.Show();
            this.Hide();
            Opacity = 100;
            ///////////////////////////////////////////////////////////////////

            //Generates the wallpaper and applies it
            loadWallpaper();

            string[] args = Environment.GetCommandLineArgs();

            foreach (string arg in args)
            {
                if (arg == "-debug")
                {

                    displayForm();
                }
            }

            //Add event to run when display settings change
            Microsoft.Win32.SystemEvents.DisplaySettingsChanged += new EventHandler(ScreenHandler);
        }

        //PROCESSES WINDOWS MESSAGES
        protected override void WndProc(ref Message m)
        {
            
            //If message says to update
            if (m.Msg == NativeMethods.UPDATE)
            {

                //Output debug message
                debugMessageOutput("Message to program saying update");

                //Update the wallpaper and set ti
                loadWallpaper();
            }

            //If message says to show debug
            else if (m.Msg == NativeMethods.DEBUG)
            {

                //Make form visible
                displayForm();
            }
            base.WndProc(ref m);
        }

        //RUNS WHEN DISPLAY SETTINGS CHANGE
        private void ScreenHandler(object sender, EventArgs e)
        {

            //Output debug message
            debugMessageOutput("Display settings changed");

            //Generates the wallpaper and applies it
            loadWallpaper();
        }

        //CALCULATE TOTAL WALLPAPER SIZE
        private void wallpaperTotalSize(ref int wallpaperTotalWidth, ref int wallpaperTotalHeight, configClass config, int minumumPaddingTop)
        {
            //For every screen on the system
            foreach (var displayScreen in Screen.AllScreens)
            {

                //Add width of screens together
                wallpaperTotalWidth = wallpaperTotalWidth + displayScreen.Bounds.Width;

                //For each screen in config
                foreach (var configScreen in config.screens)
                {
                
                    //If the name of the current system screen is the config screen name
                    if(configScreen.name == displayScreen.DeviceName)
                    {

                        //If the total wallpaper height is smaller than the current screen height
                        if(displayScreen.Bounds.Height + (configScreen.padding_top - minumumPaddingTop) > wallpaperTotalHeight)
                        {

                            //Set total wallpaper height
                            wallpaperTotalHeight = displayScreen.Bounds.Height + (configScreen.padding_top - minumumPaddingTop);
                        }
                    }
                }
            }
        }

        //CALCULATE SMALLEST PADDING TOP
        private int wallpaperMinimumPaddingTop(configClass config)
        {
            int minumumPaddingTop = 0;
            bool minumumPaddingTopFirstSet = false;

            //For every screen on the system
            foreach (var displayScreen in Screen.AllScreens)
            {

                //For each screen in config
                foreach (var configScreen in config.screens)
                {

                    //If the name of the current system screen is the config screen name
                    if (configScreen.name == displayScreen.DeviceName)
                    {

                        //If no minimum padding has been set
                        if(minumumPaddingTopFirstSet == false)
                        {

                            //Set minimum padding to first screen top padding
                            minumumPaddingTop = configScreen.padding_top;
                        }

                        //Otherwise, of screen top padding is smaller than the minimum top padding
                        else if(configScreen.padding_top < minumumPaddingTop)
                        {

                            //Set minimum padding to screen top padding
                            minumumPaddingTop = configScreen.padding_top;
                        }
                    }
                }
            }

            return minumumPaddingTop;
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

        //CALCULATE ASPECT RATIO FROM WIDTH AND HEIGHT
        private string aspectRatio(int x,   //Width of image
                                   int y)   //Height of image
        {

            //Return aspect ratio
            return string.Format("{0}:{1}", x / GCD(x, y), y / GCD(x, y));
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

        //GENERATES THE WALLPAPER AND APPLIES IT
        private void loadWallpaper()
        {

            //Output debug message
            debugMessageOutput("Begin loading wallpaper");

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

                //Output debug message
                debugMessageOutput("Read contents of config.json");

                //Attempt to parse string
                try
                {

                    //Parse json config
                    config = JsonConvert.DeserializeObject<configClass>(sJson);

                    //Output debug message
                    debugMessageOutput("Successfully parsed config");
                }

                //If parsing fails
                catch
                {

                    //Parse blank config
                    config = JsonConvert.DeserializeObject<configClass>("[]");

                    //Output debug message
                    debugMessageOutput("Unsuccessfully parsed config", true);
                }

                int minumumPaddingTop = wallpaperMinimumPaddingTop(config);

                //Get total wallpaper size
                wallpaperTotalSize(ref wallpaperTotalWidth, ref wallpaperTotalHeight, config, minumumPaddingTop);

                //Output debug message
                debugMessageOutput("Total wallpaper size: " + wallpaperTotalWidth + "x" + wallpaperTotalHeight);

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

                        //Output debug message
                        debugMessageOutput("Using " + screens.wallpaper[wallpaperRandom]);

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
                                    gWallpaper.DrawImage(wallpaper, new Rectangle(screenWidthUsed, configScreen.padding_top - minumumPaddingTop, displayScreen.Bounds.Width, displayScreen.Bounds.Height));

                                    //Output debug message
                                    debugMessageOutput("Wallpaper drawn on screen " + configScreen.name);
                                }
                            }

                            //Add screen width to the screen width used
                            screenWidthUsed = screenWidthUsed + displayScreen.Bounds.Width;
                        }
                    }
                }
                
                //Save wallpaper
                imgWallpaper.Save("wallpaper.png");

                //Output debug message
                debugMessageOutput("Saved wallpaper.png");

                //Set wallpaper
                SetDWallpaper(appPath + "/wallpaper.png");

                //Output debug message
                debugMessageOutput("Set wallpaper.png as wallpaper");

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

                //Output debug message
                debugMessageOutput("Config not found");

                //Display error message when no config exists
                MessageBox.Show("Wallpaper Management: Config Not Found");

                //Close the program
                this.Close();
            }
        }

        //DISPLAY THE FORM
        private void displayForm()
        {
            this.Show();
            debugMessageOutput("Debug form has been displayed");
        }

        //HIDE THE FORM
        private void hideForm()
        {
            this.Hide();
            debugMessageOutput("Debug form has been hidden");
        }


        public class debugListItem
        {
            public debugListItem(Color c, string m)
            {
                ItemColor = c;
                Message = m;
            }
            public Color ItemColor { get; set; }
            public string Message { get; set; }
        }

        ///////////////////////////////////////////////////////////////////////////////
        //GUI CODE FOLLOWS
        ///////////////////////////////////////////////////////////////////////////////

        //RUNS WHEN THE FORM IS LOADED
        private void frmWallpaperManagement_Load(object sender, EventArgs e)
        {
            //Load config in text box
            guiLoadConfig();
        }

        //COMMAND BUTTON TO MANUALLY UPDATE WALLPAPER
        private void cmdUpdate_Click(object sender, EventArgs e)
        {
            loadWallpaper();
        }

        //COMMAND BUTTON TO MANUALLY LOAD CONFIG
        private void cmdConfigLoad_Click(object sender, EventArgs e)
        {
            guiLoadConfig();
            MessageBox.Show("Config loaded");
        }

        //COMMAND BUTTON TO MANUALLY SAVE CONFIG
        private void cmdConfigSave_Click(object sender, EventArgs e)
        {
            if (configValidate(txtConfig.Text))
            {
                guiSaveConfig();
            }
            else
            {
                MessageBox.Show("Invalid Config");
            }
        }

        //COMMAND BUTTON TO EXIT PROGRAM
        private void cmdExit_Click(object sender, EventArgs e)
        {
            DialogResult d = MessageBox.Show("Are you sure you want to exit?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (d == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        //COMMAND BUTTON TO VALIDATE CONFIG
        private void cmdConfigValidate_Click(object sender, EventArgs e)
        {
            if (configValidate(txtConfig.Text))
            {
                MessageBox.Show("Valid Config");
            }
            else
            {
                MessageBox.Show("Invalid Config");
            }
        }

        //RUNS WHEN TEXT BOX IS CHANGED
        private void txtConfig_TextChanged(object sender, EventArgs e)
        {
            grpConfig.Text = "Config *";
        }

        //LOAD CONFIG TO TEXT BOX
        private void guiLoadConfig()
        {
            StreamReader streamReader = new StreamReader(Application.StartupPath + @"\config.json");
            string sConfig = streamReader.ReadToEnd();
            txtConfig.Text = sConfig;
            streamReader.Close();

            grpConfig.Text = "Config";
        }

        //SAVE CONFIG IN TEXT BOX TO FILE
        private void guiSaveConfig()
        {
            StreamWriter streamWriter = new StreamWriter(Application.StartupPath + @"\config.json");
            streamWriter.Write(txtConfig.Text);
            streamWriter.Close();

            grpConfig.Text = "Config";
        }

        //USED TO OUTPUT DEBUG MESSAGES
        private void debugMessageOutput(string message, bool important = false)
        {
            if (important == true)
            {
                message = "IMPORTANT: " + message;
            }

            //Add output message to list box
            listOutput.Items.Add(message);

            //Add output message to console
            Console.WriteLine(message);

            //Auto scroll list box
            listOutput.SelectedIndex = listOutput.Items.Count - 1;
            listOutput.SelectedIndex = -1;
        }

        //VALIDATES CONFIG JSON STRING - RETURNS BOOLEAN
        private bool configValidate(string json)
        {
            var configValidate = new configClass();
            try
            {
                configValidate = JsonConvert.DeserializeObject<configClass>(json);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
