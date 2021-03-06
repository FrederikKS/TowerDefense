﻿namespace TowerDefense
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.pnl_Main = new System.Windows.Forms.Panel();
            this.pnl_high = new System.Windows.Forms.Panel();
            this.rtb_highscore = new System.Windows.Forms.RichTextBox();
            this.btn_highHard = new System.Windows.Forms.Button();
            this.btn_highMedium = new System.Windows.Forms.Button();
            this.btn_highEasy = new System.Windows.Forms.Button();
            this.btn_exit = new System.Windows.Forms.Button();
            this.btn_high = new System.Windows.Forms.Button();
            this.pnl_dif = new System.Windows.Forms.Panel();
            this.btn_Hard = new System.Windows.Forms.Button();
            this.txt_name = new System.Windows.Forms.TextBox();
            this.lbl_name = new System.Windows.Forms.Label();
            this.btn_Medium = new System.Windows.Forms.Button();
            this.btn_easy = new System.Windows.Forms.Button();
            this.btn_dif = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.pnl_Main.SuspendLayout();
            this.pnl_high.SuspendLayout();
            this.pnl_dif.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnl_Main
            // 
            this.pnl_Main.Controls.Add(this.pnl_high);
            this.pnl_Main.Controls.Add(this.btn_exit);
            this.pnl_Main.Controls.Add(this.btn_high);
            this.pnl_Main.Controls.Add(this.pnl_dif);
            this.pnl_Main.Controls.Add(this.btn_dif);
            this.pnl_Main.Location = new System.Drawing.Point(21, 12);
            this.pnl_Main.Name = "pnl_Main";
            this.pnl_Main.Size = new System.Drawing.Size(1165, 490);
            this.pnl_Main.TabIndex = 0;
            // 
            // pnl_high
            // 
            this.pnl_high.Controls.Add(this.rtb_highscore);
            this.pnl_high.Controls.Add(this.btn_highHard);
            this.pnl_high.Controls.Add(this.btn_highMedium);
            this.pnl_high.Controls.Add(this.btn_highEasy);
            this.pnl_high.Location = new System.Drawing.Point(214, 121);
            this.pnl_high.Name = "pnl_high";
            this.pnl_high.Size = new System.Drawing.Size(574, 341);
            this.pnl_high.TabIndex = 4;
            // 
            // rtb_highscore
            // 
            this.rtb_highscore.Location = new System.Drawing.Point(5, 108);
            this.rtb_highscore.Name = "rtb_highscore";
            this.rtb_highscore.ReadOnly = true;
            this.rtb_highscore.Size = new System.Drawing.Size(560, 176);
            this.rtb_highscore.TabIndex = 3;
            this.rtb_highscore.Text = "";
            // 
            // btn_highHard
            // 
            this.btn_highHard.BackgroundImage = global::TowerDefense.Properties.Resources.menu_b;
            this.btn_highHard.Location = new System.Drawing.Point(384, 2);
            this.btn_highHard.Name = "btn_highHard";
            this.btn_highHard.Size = new System.Drawing.Size(185, 97);
            this.btn_highHard.TabIndex = 2;
            this.btn_highHard.Text = "Hard";
            this.btn_highHard.UseVisualStyleBackColor = true;
            this.btn_highHard.Click += new System.EventHandler(this.btn_highHard_Click);
            // 
            // btn_highMedium
            // 
            this.btn_highMedium.BackgroundImage = global::TowerDefense.Properties.Resources.menu_b;
            this.btn_highMedium.Location = new System.Drawing.Point(195, 2);
            this.btn_highMedium.Name = "btn_highMedium";
            this.btn_highMedium.Size = new System.Drawing.Size(185, 97);
            this.btn_highMedium.TabIndex = 1;
            this.btn_highMedium.Text = "Medium";
            this.btn_highMedium.UseVisualStyleBackColor = true;
            this.btn_highMedium.Click += new System.EventHandler(this.btn_highMedium_Click);
            // 
            // btn_highEasy
            // 
            this.btn_highEasy.BackgroundImage = global::TowerDefense.Properties.Resources.menu_b;
            this.btn_highEasy.Location = new System.Drawing.Point(5, 2);
            this.btn_highEasy.Name = "btn_highEasy";
            this.btn_highEasy.Size = new System.Drawing.Size(185, 97);
            this.btn_highEasy.TabIndex = 0;
            this.btn_highEasy.Text = "Easy";
            this.btn_highEasy.UseVisualStyleBackColor = true;
            this.btn_highEasy.Click += new System.EventHandler(this.btn_highEasy_Click);
            // 
            // btn_exit
            // 
            this.btn_exit.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btn_exit.BackgroundImage")));
            this.btn_exit.Location = new System.Drawing.Point(25, 222);
            this.btn_exit.Name = "btn_exit";
            this.btn_exit.Size = new System.Drawing.Size(184, 95);
            this.btn_exit.TabIndex = 3;
            this.btn_exit.Text = "Exit Game";
            this.btn_exit.UseVisualStyleBackColor = true;
            this.btn_exit.Click += new System.EventHandler(this.btn_exit_Click);
            // 
            // btn_high
            // 
            this.btn_high.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btn_high.BackgroundImage")));
            this.btn_high.Location = new System.Drawing.Point(24, 122);
            this.btn_high.Name = "btn_high";
            this.btn_high.Size = new System.Drawing.Size(184, 95);
            this.btn_high.TabIndex = 2;
            this.btn_high.Text = "Highscore";
            this.btn_high.UseVisualStyleBackColor = true;
            this.btn_high.Click += new System.EventHandler(this.btn_high_Click);
            // 
            // pnl_dif
            // 
            this.pnl_dif.Controls.Add(this.btn_Hard);
            this.pnl_dif.Controls.Add(this.txt_name);
            this.pnl_dif.Controls.Add(this.lbl_name);
            this.pnl_dif.Controls.Add(this.btn_Medium);
            this.pnl_dif.Controls.Add(this.btn_easy);
            this.pnl_dif.Location = new System.Drawing.Point(214, 9);
            this.pnl_dif.Name = "pnl_dif";
            this.pnl_dif.Size = new System.Drawing.Size(936, 108);
            this.pnl_dif.TabIndex = 1;
            this.pnl_dif.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // btn_Hard
            // 
            this.btn_Hard.BackgroundImage = global::TowerDefense.Properties.Resources.menu_b;
            this.btn_Hard.Location = new System.Drawing.Point(383, 7);
            this.btn_Hard.Name = "btn_Hard";
            this.btn_Hard.Size = new System.Drawing.Size(185, 97);
            this.btn_Hard.TabIndex = 2;
            this.btn_Hard.Text = "Hard";
            this.btn_Hard.UseVisualStyleBackColor = true;
            this.btn_Hard.Click += new System.EventHandler(this.btn_Hard_Click);
            // 
            // txt_name
            // 
            this.txt_name.Location = new System.Drawing.Point(577, 24);
            this.txt_name.Name = "txt_name";
            this.txt_name.Size = new System.Drawing.Size(100, 20);
            this.txt_name.TabIndex = 5;
            // 
            // lbl_name
            // 
            this.lbl_name.AutoSize = true;
            this.lbl_name.Location = new System.Drawing.Point(574, 6);
            this.lbl_name.Name = "lbl_name";
            this.lbl_name.Size = new System.Drawing.Size(41, 15);
            this.lbl_name.TabIndex = 7;
            this.lbl_name.Text = "Name";
            // 
            // btn_Medium
            // 
            this.btn_Medium.BackgroundImage = global::TowerDefense.Properties.Resources.menu_b;
            this.btn_Medium.Location = new System.Drawing.Point(193, 8);
            this.btn_Medium.Name = "btn_Medium";
            this.btn_Medium.Size = new System.Drawing.Size(185, 97);
            this.btn_Medium.TabIndex = 1;
            this.btn_Medium.Text = "Medium";
            this.btn_Medium.UseVisualStyleBackColor = true;
            this.btn_Medium.Click += new System.EventHandler(this.btn_Medium_Click);
            // 
            // btn_easy
            // 
            this.btn_easy.BackgroundImage = global::TowerDefense.Properties.Resources.menu_b;
            this.btn_easy.Location = new System.Drawing.Point(3, 7);
            this.btn_easy.Name = "btn_easy";
            this.btn_easy.Size = new System.Drawing.Size(185, 97);
            this.btn_easy.TabIndex = 0;
            this.btn_easy.Text = "Easy";
            this.btn_easy.UseVisualStyleBackColor = true;
            this.btn_easy.Click += new System.EventHandler(this.btn_easy_Click);
            // 
            // btn_dif
            // 
            this.btn_dif.BackgroundImage = global::TowerDefense.Properties.Resources.menu_b;
            this.btn_dif.Location = new System.Drawing.Point(24, 16);
            this.btn_dif.Name = "btn_dif";
            this.btn_dif.Size = new System.Drawing.Size(182, 96);
            this.btn_dif.TabIndex = 0;
            this.btn_dif.Text = "Difficulty";
            this.btn_dif.UseVisualStyleBackColor = true;
            this.btn_dif.Click += new System.EventHandler(this.btn_dif_Click);
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.BackgroundImage = global::TowerDefense.Properties.Resources.menu_bg;
            this.ClientSize = new System.Drawing.Size(1337, 742);
            this.Controls.Add(this.pnl_Main);
            this.Name = "Form1";
            this.Text = "Tower Defence";
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseDown);
            this.pnl_Main.ResumeLayout(false);
            this.pnl_high.ResumeLayout(false);
            this.pnl_dif.ResumeLayout(false);
            this.pnl_dif.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnl_Main;
        private System.Windows.Forms.Panel pnl_dif;
        private System.Windows.Forms.Button btn_Hard;
        private System.Windows.Forms.Button btn_Medium;
        private System.Windows.Forms.Button btn_easy;
        private System.Windows.Forms.Button btn_dif;
        private System.Windows.Forms.Button btn_exit;
        private System.Windows.Forms.Button btn_high;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Panel pnl_high;
        private System.Windows.Forms.Button btn_highHard;
        private System.Windows.Forms.Button btn_highMedium;
        private System.Windows.Forms.Button btn_highEasy;
        private System.Windows.Forms.RichTextBox rtb_highscore;
        private System.Windows.Forms.Label lbl_name;
        private System.Windows.Forms.TextBox txt_name;
    }
}

