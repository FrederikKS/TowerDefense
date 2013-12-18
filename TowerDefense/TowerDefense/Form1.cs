using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace TowerDefense
{
    public partial class Form1 : Form
    {
        int difc = 0;
        int highscoreC = 0;
        GameWorld gw;

        public Form1()
        {
            InitializeComponent();
            timer1.Enabled = true;
            timer1.Interval = 40;
            pnl_Main.Visible = true;
            pnl_dif.Visible = false;
            pnl_high.Visible = false;
            pnl_submit.Visible = false;
            difc = 0;
        }
        #region Difficulty
        private void btn_dif_Click(object sender, EventArgs e)
        {
            pnl_dif.Visible = true;
            pnl_high.Visible = false;
        }

        private void btn_easy_Click(object sender, EventArgs e)
        {
            difc = 1;
            pnl_Main.Visible = false;
        }

        private void btn_Medium_Click(object sender, EventArgs e)
        {
            difc = 2;
            pnl_Main.Visible = false;
        }

        private void btn_Hard_Click(object sender, EventArgs e)
        {
            difc = 3;
            pnl_Main.Visible = false;
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
        #endregion

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (difc > 0)
            {
                if (gw == null)
                {
                    gw = new GameWorld(CreateGraphics(), this.DisplayRectangle, 20, 20, difc);
                }

                gw.GameLoop();
            }

        }
        #region Highscore
        private void btn_high_Click(object sender, EventArgs e)
        {
            pnl_high.Visible = true;
            pnl_dif.Visible = false;
        }

        private void btn_highEasy_Click(object sender, EventArgs e)
        {
            //TextReader tr = new StreamReader(@"Highscore/Easy.txt");
            //rtb_highscore.Text = tr.ReadLine();
            //tr.Close();

            highscoreC = 1;
            string path = @"C:\Users\Mikkel\Documents\GitHub\TowerDefense\TowerDefense\TowerDefense\Highscore\Easy.txt";
            string readText = File.ReadAllText(path);
            rtb_highscore.Text = readText.ToString();
            pnl_submit.Visible = true;
        }

        private void btn_highMedium_Click(object sender, EventArgs e)
        {
            highscoreC = 2;
            string path = @"C:\Users\Mikkel\Documents\GitHub\TowerDefense\TowerDefense\TowerDefense\Highscore\Medium.txt";
            string readText = File.ReadAllText(path);
            rtb_highscore.Text = readText.ToString();
            pnl_submit.Visible = true;
        }

        private void btn_highHard_Click(object sender, EventArgs e)
        {
            highscoreC = 3;
            string path = @"C:\Users\Mikkel\Documents\GitHub\TowerDefense\TowerDefense\TowerDefense\Highscore\Hard.txt";
            string readText = File.ReadAllText(path);
            rtb_highscore.Text = readText.ToString();
            pnl_submit.Visible = true;
        }

        private void btn_submit_Click(object sender, EventArgs e)
        {
            if (highscoreC == 1)
            {
                string path = @"C:\Users\Mikkel\Documents\GitHub\TowerDefense\TowerDefense\TowerDefense\Highscore\Easy.txt";
                string text = "\nName: " + txt_name.Text + "\n" + "Lives Remaining: " + txt_health.Text + "\n --------------------";
                File.AppendAllText(path, text);
            }
            if (highscoreC == 2)
            {
                string path = @"C:\Users\Mikkel\Documents\GitHub\TowerDefense\TowerDefense\TowerDefense\Highscore\Medium.txt";
                string text = "\nName: " + txt_name.Text + "\n" + "Lives Remaining: " + txt_health.Text + "\n --------------------";
                File.AppendAllText(path, text);
            }
            if (highscoreC == 3)
            {
                string path = @"C:\Users\Mikkel\Documents\GitHub\TowerDefense\TowerDefense\TowerDefense\Highscore\Hard.txt";
                string text = "\nName: " + txt_name.Text + "\n" + "Lives Remaining: " + txt_health.Text + "\n --------------------";
                File.AppendAllText(path, text);
            }
        }
        #endregion

        private void btn_exit_Click(object sender, EventArgs e)
        {
            DialogResult dlgRes;
            dlgRes = MessageBox.Show("Are you sure you'll exit?", "Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (dlgRes == DialogResult.Yes)
            {
                Form1.ActiveForm.Close();
            }
        }
    }
}
