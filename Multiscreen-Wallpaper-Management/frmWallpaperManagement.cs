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

        ////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////////
        configClass config;

        List<int> wallpaperRandomIndexes = new List<int>();

        //Declare variable used to generate random number for random wallpaper
        Random randomNumber = new Random();
        ////////////////////////////////////////////////////////////////////////////////////////////////

        //RETURNS CONFIG OBJECT PARSED FROM CONFIG.JSON
        private configClass loadConfig()
        {
            //Read config
            StreamReader streamReaderConfig = new StreamReader(Application.StartupPath + @"\config.json");

            //Store contents of config.json in variable
            string json = streamReaderConfig.ReadToEnd();

            //Attempt to parse string
            try
            {

                //Parse json config
                config = JsonConvert.DeserializeObject<configClass>(json);

                streamReaderConfig.Close();

                //Return config
                return config;
            }

            //If parsing fails
            catch
            {

                //Parse blank config
                config = JsonConvert.DeserializeObject<configClass>("[]");

                streamReaderConfig.Close();

                //Return config
                return config;
            }
        }

        private void loadWallpaper()
        {

            //Load config
            config = loadConfig();

            if (config.update_every > 0 && config.update_every < 2147483647)
            {
                update.Interval = config.update_every;
                update.Enabled = true;
            }
            else
            {
                update.Enabled = false;
            }

            //Clear random wallpaper indexes
            wallpaperRandomIndexes.Clear();

            //Loop for every screen in the config
            foreach (var screens in config.screens)
            {
                
                //Declare integer and store wallpaper count in it                       
                int wallpaperCount = screens.wallpaper.Count;

                //Generate a random number for random wallpaper              
                int wallpaperRandom = randomNumber.Next(0, wallpaperCount);

                //Add random index to list
                wallpaperRandomIndexes.Add(wallpaperRandom);
            }

            guiMessageOutput("Wallpaper Loaded");

            displayWallpaper();
        }

        private void displayWallpaper()
        {

            //Calculate the minimum top padding
            int minumumPaddingTop = wallpaperMinimumPaddingTop(config);

            //Declare wallpaper total width and height variables
            int wallpaperTotalWidth = 0;
            int wallpaperTotalHeight = 0;

            //Get total wallpaper size
            wallpaperTotalSize(ref wallpaperTotalWidth, ref wallpaperTotalHeight, config, minumumPaddingTop);

            //Create wallpaper template
            Image imgWallpaper = new Bitmap(wallpaperTotalWidth, wallpaperTotalHeight);

            //Convert wallpaper template to graphic
            Graphics gWallpaper = Graphics.FromImage(imgWallpaper);

            //Declare and set index counter
            int i = 0;

            int screenWidthUsed = 0;

            //Loop for every screen in the config
            foreach (var configScreen in config.screens)
            {

                //Loop for every screen
                foreach (var displayScreen in Screen.AllScreens)
                {

                    //If the name of the screen is equal to the X screen from config
                    if (configScreen.name == displayScreen.DeviceName)
                    {

                        //If image file exists
                        if (File.Exists(configScreen.wallpaper[wallpaperRandomIndexes[i]]))
                        {

                            //Store wallpaper image to variable
                            Image imgScreenWallpaper = Image.FromFile(configScreen.wallpaper[wallpaperRandomIndexes[i]]);

                            gWallpaper.DrawImage(imgScreenWallpaper, new Rectangle(screenWidthUsed, configScreen.padding_top - minumumPaddingTop, displayScreen.Bounds.Width, displayScreen.Bounds.Height));

                            imgScreenWallpaper.Dispose();

                        }

                        //Add screen width to the screen width used
                        screenWidthUsed = displayScreen.Bounds.Width + screenWidthUsed;
                    }
                }

                //Add one to index
                i++;
            }

            //Save wallpaper
            imgWallpaper.Save("wallpaper.png");

            //Set wallpaper
            SetDWallpaper(appPath + "/wallpaper.png");

            guiMessageOutput("Wallpaper Displayed");
        }






        ////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////////


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
            public int update_every { get; set; }
            public List<configMiscClass> misc { get; set; }
        }

        //USED TO SET WALLPAPER
        public void SetDWallpaper(string path)
        {

            //Set wallpaper to image at given path
            SystemParametersInfo(SPI_SETDESKWALLPAPER, 0, path, SPIF_UPDATEINIFILE);
        }

        public frmWallpaperManagement()
        {
            if (File.Exists(Application.StartupPath + @"\config.json"))
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
                    if (arg == "-gui")
                    {

                        displayForm();
                    }
                }

                //Add event to run when display settings change
                Microsoft.Win32.SystemEvents.DisplaySettingsChanged += new EventHandler(ScreenHandler);
            }

            else
            {
                MessageBox.Show("Wallpaper Management: Config Not Found");
                Environment.Exit(1);
            }
        }

        //PROCESSES WINDOWS MESSAGES
        protected override void WndProc(ref Message m)
        {
            
            //If message says to update
            if (m.Msg == NativeMethods.UPDATE)
            {

                //Output gui message
                guiMessageOutput("Message to program saying update");

                //Update the wallpaper and set ti
                loadWallpaper();
            }

            //If message says to show gui
            else if (m.Msg == NativeMethods.GUI)
            {

                //Make form visible
                displayForm();
            }
            base.WndProc(ref m);
        }

        //RUNS WHEN DISPLAY SETTINGS CHANGE
        private void ScreenHandler(object sender, EventArgs e)
        {

            //Output gui message
            guiMessageOutput("Display settings changed");

            //Generates the wallpaper and applies it
            displayWallpaper();
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
                        if (displayScreen.Bounds.Height + configScreen.padding_top > wallpaperTotalHeight)
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
                        if (minumumPaddingTopFirstSet == false)
                        {

                            //Set minimum padding to first screen top padding
                            minumumPaddingTop = configScreen.padding_top;

                            //Set flag so that boolean flag is set
                            minumumPaddingTopFirstSet = true;
                        }

                        //Otherwise, of screen top padding is smaller than the minimum top padding
                        else if (configScreen.padding_top < minumumPaddingTop)
                        {

                            //Set minimum padding to screen top padding
                            minumumPaddingTop = configScreen.padding_top;
                        }
                    }
                }
            }

            //return minumumPaddingTop;
            return 0;
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


        //GENERATES THE WALLPAPER AND APPLIES IT
        /*private void loadWallpaper_old()
        {

            //Output gui message
            guiMessageOutput("Begin loading wallpaper");

            int i;                          //Declare variable used for an index
            int wallpaperTotalWidth = 0;    //Declare variable used for total wallpaper width
            int wallpaperTotalHeight = 0;   //Declare variable used for total wallpaper height
            string sJson = "";              //Declare variable used to store json from config
            StreamReader streamReaderJson;  //Declare variable used to open config
            configClass config;

            //Read config
            streamReaderJson = new StreamReader(Application.StartupPath + @"\config.json");

            //Store contents of config.json in variable
            sJson = streamReaderJson.ReadToEnd();

            //Output gui message
            guiMessageOutput("Read contents of config.json");

            //Attempt to parse string
            try
            {

                //Parse json config
                config = JsonConvert.DeserializeObject<configClass>(sJson);

                //Output gui message
                guiMessageOutput("Successfully parsed config");
            }

            //If parsing fails
            catch
            {

                //Parse blank config
                config = JsonConvert.DeserializeObject<configClass>("[]");

                //Output gui message
                guiMessageOutput("Unsuccessfully parsed config", true);
            }

            if(config.update_every > 0 && config.update_every < 2147483647)
            {
                update.Interval = config.update_every;
                update.Enabled = true;
            }
            else
            {
                update.Enabled = false;
            }

            int minumumPaddingTop = wallpaperMinimumPaddingTop(config);

            //Get total wallpaper size
            wallpaperTotalSize(ref wallpaperTotalWidth, ref wallpaperTotalHeight, config, minumumPaddingTop);

            //Output gui message
            guiMessageOutput("Total wallpaper size: " + wallpaperTotalWidth + "x" + wallpaperTotalHeight);

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
                    //Output gui message
                    guiMessageOutput("Using " + screens.wallpaper[wallpaperRandom]);

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
                            if (aspectRatio(displayScreen.Bounds.Width, displayScreen.Bounds.Height) == aspectRatio(wallpaper.Width, wallpaper.Height))
                            {
                                //Add wallpaper image to template
                                gWallpaper.DrawImage(wallpaper, new Rectangle(screenWidthUsed, configScreen.padding_top - minumumPaddingTop, displayScreen.Bounds.Width, displayScreen.Bounds.Height));
                                
                                //Output gui message
                                guiMessageOutput("Wallpaper drawn on screen " + configScreen.name);
                            }
                        }

                        //Add screen width to the screen width used
                        screenWidthUsed = screenWidthUsed + displayScreen.Bounds.Width;
                    }
                }
            }

            //Save wallpaper
            imgWallpaper.Save("wallpaper.png");

            //Output gui message
            guiMessageOutput("Saved wallpaper.png");

            //Set wallpaper
            SetDWallpaper(appPath + "/wallpaper.png");

            //Output gui message
            guiMessageOutput("Set wallpaper.png as wallpaper");

            //Close the config file
            streamReaderJson.Close();

            //Set index to 0
            i = 0;

            //Loop for every image in imgwallpapers
            foreach (var nul in imgwallpapers)
            {

                //Dispose image stored
                imgwallpapers[i].Dispose();

                //Add one to index
                i++;
            }

            imgwallpapers.Clear();

            imgWallpaper.Dispose();

            gWallpaper.Dispose();
        }*/

        //DISPLAY THE FORM
        private void displayForm()
        {
            this.Show();
            guiMessageOutput("GUI form has been displayed");
        }

        //HIDE THE FORM
        private void hideForm()
        {
            this.Hide();
            guiMessageOutput("GUI form has been hidden");
        }


        public class guiListItem
        {
            public guiListItem(Color c, string m)
            {
                ItemColor = c;
                Message = m;
            }
            public Color ItemColor { get; set; }
            public string Message { get; set; }
        }

        private void update_Tick(object sender, EventArgs e)
        {
            loadWallpaper();
        }

        ///////////////////////////////////////////////////////////////////////////////
        //GUI CODE FOLLOWS
        ///////////////////////////////////////////////////////////////////////////////

        bool configModified;

        //RUNS WHEN THE FORM IS LOADED
        private void frmWallpaperManagement_Load(object sender, EventArgs e)
        {
            //Load config in text box
            guiLoadConfig();
        }

        //COMMAND BUTTON TO MANUALLY UPDATE WALLPAPER
        private void cmdUpdate_Click(object sender, EventArgs e)
        {
            cmdUpdate.Enabled = false;
            loadWallpaper();
            cmdUpdate.Enabled = true;
            cmdUpdate.Focus();
        }

        //COMMAND BUTTON TO MANUALLY LOAD CONFIG
        private void cmdConfigLoad_Click(object sender, EventArgs e)
        {
            guiLoadConfig();
            lblConfigStatus.ForeColor = Color.Green;
            lblConfigStatus.Text = "Config loaded";
        }

        //COMMAND BUTTON TO MANUALLY SAVE CONFIG
        private void cmdConfigSave_Click(object sender, EventArgs e)
        {
            if (configValidate(txtConfig.Text))
            {
                guiSaveConfig();
                configModified = false;
                lblConfigStatus.ForeColor = Color.Green;
                lblConfigStatus.Text = "Config saved";
            }
            else
            {
                lblConfigStatus.ForeColor = Color.Red;
                lblConfigStatus.Text = "Invalid config";
            }
        }

        //COMMAND BUTTON TO HIDE PROGRAM
        private void cmdHide_Click(object sender, EventArgs e)
        {
            this.Hide();
            guiMessageOutput("GUI Hidden");
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
                lblConfigStatus.ForeColor = Color.Green;
                lblConfigStatus.Text = "Valid config";
            }
            else
            {
                lblConfigStatus.ForeColor = Color.Red;
                lblConfigStatus.Text = "Invalid config";
            }
        }

        //RUNS WHEN TEXT BOX IS CHANGED
        private void txtConfig_TextChanged(object sender, EventArgs e)
        {
            grpConfig.Text = "Config *";
            configModified = true;
        }

        //LOAD CONFIG TO TEXT BOX
        private void guiLoadConfig()
        {

            StreamReader streamReader = new StreamReader(Application.StartupPath + @"\config.json");
            string configJson = streamReader.ReadToEnd();
            txtConfig.Text = configJson;
            streamReader.Close();

            var config_edit = new configClass();
            config_edit = JsonConvert.DeserializeObject<configClass>(configJson);
            txtAutoUpdate.Text = Convert.ToString(config_edit.update_every);

            

            //JsonFormat.FormatJson()
            grpConfig.Text = "Config";
            configModified = false;
        }

        //SAVE CONFIG IN TEXT BOX TO FILE
        private void guiSaveConfig()
        {

            var config_edit = new configClass();
            config_edit = JsonConvert.DeserializeObject<configClass>(txtConfig.Text);

            var configJson = JsonConvert.SerializeObject(config_edit, Formatting.Indented);

            StreamWriter streamWriter = new StreamWriter(Application.StartupPath + @"\config.json");
            streamWriter.Write(configJson);
            streamWriter.Close();

            guiLoadConfig();

            grpConfig.Text = "Config";
        }

        //USED TO OUTPUT GUI MESSAGES
        private void guiMessageOutput(string message, bool important = false)
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

        private void cmdAutoUpdateSave_Click(object sender, EventArgs e)
        {
            int updateEvery;
            bool updateEveryCheck = Int32.TryParse(txtAutoUpdate.Text, out updateEvery);

            if (configModified == true)
            {
                lblConfigStatus.ForeColor = Color.Red;
                lblConfigStatus.Text = "Config modified but not saved";
            }
            else if (updateEveryCheck == false)
            {
                lblConfigStatus.ForeColor = Color.Red;
                lblConfigStatus.Text = "Auto update value not integer";
            }
            else
            {
                StreamReader streamReader = new StreamReader(Application.StartupPath + @"\config.json");
                string configJson = streamReader.ReadToEnd();
                streamReader.Close();

                var config = new configClass();
                config = JsonConvert.DeserializeObject<configClass>(configJson);

                config.update_every = Convert.ToInt32(txtAutoUpdate.Text);

                configJson = JsonConvert.SerializeObject(config, Formatting.Indented);

                StreamWriter streamWriter = new StreamWriter(Application.StartupPath + @"\config.json");
                streamWriter.Write(configJson);
                streamWriter.Close();

                guiLoadConfig();
                loadWallpaper();
            }
        }
    }
}
