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
namespace MultiScreenWallpaper
{
    partial class frmWallpaperManagement
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.grpOutput = new System.Windows.Forms.GroupBox();
            this.listOutput = new System.Windows.Forms.ListBox();
            this.grpConfig = new System.Windows.Forms.GroupBox();
            this.lblConfigStatus = new System.Windows.Forms.Label();
            this.cmdConfigValidate = new System.Windows.Forms.Button();
            this.cmdConfigLoad = new System.Windows.Forms.Button();
            this.cmdConfigSave = new System.Windows.Forms.Button();
            this.txtConfig = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cmdExit = new System.Windows.Forms.Button();
            this.cmdUpdate = new System.Windows.Forms.Button();
            this.update = new System.Windows.Forms.Timer(this.components);
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtAutoUpdate = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cmdAutoUpdateSave = new System.Windows.Forms.Button();
            this.cmdHide = new System.Windows.Forms.Button();
            this.grpOutput.SuspendLayout();
            this.grpConfig.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpOutput
            // 
            this.grpOutput.Controls.Add(this.listOutput);
            this.grpOutput.Location = new System.Drawing.Point(12, 12);
            this.grpOutput.Name = "grpOutput";
            this.grpOutput.Size = new System.Drawing.Size(361, 475);
            this.grpOutput.TabIndex = 5;
            this.grpOutput.TabStop = false;
            this.grpOutput.Text = "Output";
            // 
            // listOutput
            // 
            this.listOutput.FormattingEnabled = true;
            this.listOutput.Location = new System.Drawing.Point(6, 19);
            this.listOutput.Name = "listOutput";
            this.listOutput.Size = new System.Drawing.Size(350, 446);
            this.listOutput.TabIndex = 0;
            // 
            // grpConfig
            // 
            this.grpConfig.Controls.Add(this.lblConfigStatus);
            this.grpConfig.Controls.Add(this.cmdConfigValidate);
            this.grpConfig.Controls.Add(this.cmdConfigLoad);
            this.grpConfig.Controls.Add(this.cmdConfigSave);
            this.grpConfig.Controls.Add(this.txtConfig);
            this.grpConfig.Location = new System.Drawing.Point(379, 12);
            this.grpConfig.Name = "grpConfig";
            this.grpConfig.Size = new System.Drawing.Size(689, 475);
            this.grpConfig.TabIndex = 6;
            this.grpConfig.TabStop = false;
            this.grpConfig.Text = "Config";
            // 
            // lblConfigStatus
            // 
            this.lblConfigStatus.Location = new System.Drawing.Point(249, 447);
            this.lblConfigStatus.Name = "lblConfigStatus";
            this.lblConfigStatus.Size = new System.Drawing.Size(434, 18);
            this.lblConfigStatus.TabIndex = 4;
            this.lblConfigStatus.Text = " ";
            // 
            // cmdConfigValidate
            // 
            this.cmdConfigValidate.Location = new System.Drawing.Point(168, 445);
            this.cmdConfigValidate.Name = "cmdConfigValidate";
            this.cmdConfigValidate.Size = new System.Drawing.Size(75, 23);
            this.cmdConfigValidate.TabIndex = 3;
            this.cmdConfigValidate.Text = "Validate";
            this.cmdConfigValidate.UseVisualStyleBackColor = true;
            this.cmdConfigValidate.Click += new System.EventHandler(this.cmdConfigValidate_Click);
            // 
            // cmdConfigLoad
            // 
            this.cmdConfigLoad.Location = new System.Drawing.Point(6, 445);
            this.cmdConfigLoad.Name = "cmdConfigLoad";
            this.cmdConfigLoad.Size = new System.Drawing.Size(75, 23);
            this.cmdConfigLoad.TabIndex = 2;
            this.cmdConfigLoad.Text = "Load";
            this.cmdConfigLoad.UseVisualStyleBackColor = true;
            this.cmdConfigLoad.Click += new System.EventHandler(this.cmdConfigLoad_Click);
            // 
            // cmdConfigSave
            // 
            this.cmdConfigSave.Location = new System.Drawing.Point(87, 445);
            this.cmdConfigSave.Name = "cmdConfigSave";
            this.cmdConfigSave.Size = new System.Drawing.Size(75, 23);
            this.cmdConfigSave.TabIndex = 1;
            this.cmdConfigSave.Text = "Save";
            this.cmdConfigSave.UseVisualStyleBackColor = true;
            this.cmdConfigSave.Click += new System.EventHandler(this.cmdConfigSave_Click);
            // 
            // txtConfig
            // 
            this.txtConfig.Location = new System.Drawing.Point(6, 19);
            this.txtConfig.Multiline = true;
            this.txtConfig.Name = "txtConfig";
            this.txtConfig.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtConfig.Size = new System.Drawing.Size(677, 420);
            this.txtConfig.TabIndex = 0;
            this.txtConfig.TextChanged += new System.EventHandler(this.txtConfig_TextChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cmdHide);
            this.groupBox1.Controls.Add(this.cmdExit);
            this.groupBox1.Controls.Add(this.cmdUpdate);
            this.groupBox1.Location = new System.Drawing.Point(12, 493);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(525, 48);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Operations";
            // 
            // cmdExit
            // 
            this.cmdExit.Location = new System.Drawing.Point(168, 19);
            this.cmdExit.Name = "cmdExit";
            this.cmdExit.Size = new System.Drawing.Size(75, 23);
            this.cmdExit.TabIndex = 2;
            this.cmdExit.Text = "Exit";
            this.cmdExit.UseVisualStyleBackColor = true;
            this.cmdExit.Click += new System.EventHandler(this.cmdExit_Click);
            // 
            // cmdUpdate
            // 
            this.cmdUpdate.Location = new System.Drawing.Point(6, 19);
            this.cmdUpdate.Name = "cmdUpdate";
            this.cmdUpdate.Size = new System.Drawing.Size(75, 23);
            this.cmdUpdate.TabIndex = 0;
            this.cmdUpdate.Text = "Update";
            this.cmdUpdate.UseVisualStyleBackColor = true;
            this.cmdUpdate.Click += new System.EventHandler(this.cmdUpdate_Click);
            // 
            // update
            // 
            this.update.Tick += new System.EventHandler(this.update_Tick);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txtAutoUpdate);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.cmdAutoUpdateSave);
            this.groupBox2.Location = new System.Drawing.Point(543, 493);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(525, 48);
            this.groupBox2.TabIndex = 8;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Auto Update";
            // 
            // txtAutoUpdate
            // 
            this.txtAutoUpdate.Location = new System.Drawing.Point(153, 21);
            this.txtAutoUpdate.Name = "txtAutoUpdate";
            this.txtAutoUpdate.Size = new System.Drawing.Size(100, 20);
            this.txtAutoUpdate.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(141, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Update Every (Milliseconds):";
            // 
            // cmdAutoUpdateSave
            // 
            this.cmdAutoUpdateSave.Location = new System.Drawing.Point(259, 19);
            this.cmdAutoUpdateSave.Name = "cmdAutoUpdateSave";
            this.cmdAutoUpdateSave.Size = new System.Drawing.Size(75, 23);
            this.cmdAutoUpdateSave.TabIndex = 0;
            this.cmdAutoUpdateSave.Text = "Save";
            this.cmdAutoUpdateSave.UseVisualStyleBackColor = true;
            this.cmdAutoUpdateSave.Click += new System.EventHandler(this.cmdAutoUpdateSave_Click);
            // 
            // cmdHide
            // 
            this.cmdHide.Location = new System.Drawing.Point(87, 19);
            this.cmdHide.Name = "cmdHide";
            this.cmdHide.Size = new System.Drawing.Size(75, 23);
            this.cmdHide.TabIndex = 3;
            this.cmdHide.Text = "Hide";
            this.cmdHide.UseVisualStyleBackColor = true;
            this.cmdHide.Click += new System.EventHandler(this.cmdHide_Click);
            // 
            // frmWallpaperManagement
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1080, 550);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.grpConfig);
            this.Controls.Add(this.grpOutput);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "frmWallpaperManagement";
            this.ShowIcon = false;
            this.Text = "Wallpaper Management";
            this.Load += new System.EventHandler(this.frmWallpaperManagement_Load);
            this.grpOutput.ResumeLayout(false);
            this.grpConfig.ResumeLayout(false);
            this.grpConfig.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.GroupBox grpOutput;
        private System.Windows.Forms.GroupBox grpConfig;
        private System.Windows.Forms.Button cmdConfigSave;
        private System.Windows.Forms.TextBox txtConfig;
        private System.Windows.Forms.ListBox listOutput;
        private System.Windows.Forms.Button cmdConfigLoad;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button cmdUpdate;
        private System.Windows.Forms.Button cmdConfigValidate;
        private System.Windows.Forms.Button cmdExit;
        private System.Windows.Forms.Timer update;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox txtAutoUpdate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button cmdAutoUpdateSave;
        private System.Windows.Forms.Label lblConfigStatus;
        private System.Windows.Forms.Button cmdHide;
    }
}

