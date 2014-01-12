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
        // test
        public static GUI gui;
        // Dificulity choosen
        public static int difc = 0;
        // Highscore Choosen
        int highscoreC = 0;
        // Classes
        public static GameWorld gw;
        //Mouse local position on the form
        public static Point localMousePos;
        //Checks if the GUI should be drawn
        public static int drawBuildGUI = 0;
        public static bool guiIsClicked = false;
        public static Point guiPos;

        public Form1()
        {

            InitializeComponent();
            // Enable the timer and timer interval
            timer1.Enabled = true;
            timer1.Interval = 40;
            // Menu
            // Pnl Visible
            pnl_Main.Visible = true;
            pnl_dif.Visible = false;
            pnl_high.Visible = false;
            // pnl & btn colors
            pnl_Main.BackColor = Color.Transparent;
            pnl_dif.BackColor = Color.Transparent;
            pnl_high.BackColor = Color.Transparent;
            pnl_name.BackColor = Color.Blue;
            btn_dif.BackColor = Color.Transparent;
            btn_high.BackColor = Color.Transparent;
            btn_exit.BackColor = Color.Transparent;
            // GUI
            MouseUp += new MouseEventHandler(Form1_MouseUp);
            MouseDown += new MouseEventHandler(Form1_MouseDown);

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
            localMousePos = this.PointToClient(Cursor.Position);
            if (difc > 0)
            {
                if (gui == null)
                {
                    gui = new GUI();
                }
                if (drawBuildGUI > 1 && drawBuildGUI < 6)
                {
                    if (gw.currentState == State.build)
                        gui.DrawGUI(CreateGraphics());
                }
                if (gw == null)
                {
                    gw = new GameWorld(CreateGraphics(), this.DisplayRectangle, 14, 8, difc);
                }
                if (gw.life <= 1)
                {
                    pnl_name.Visible = true;
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
            highscoreC = 1;
            string path = @"Highscore/Easy.txt";
            string readText = File.ReadAllText(path);
            rtb_highscore.Text = readText.ToString();
        }

        private void btn_highMedium_Click(object sender, EventArgs e)
        {
            highscoreC = 2;
            string path = @"Highscore/Medium.txt";
            string readText = File.ReadAllText(path);
            rtb_highscore.Text = readText.ToString();
            // bla
        }

        private void btn_highHard_Click(object sender, EventArgs e)
        {
            highscoreC = 3;
            string path = @"Highscore/Hard.txt";
            string readText = File.ReadAllText(path);
            rtb_highscore.Text = readText.ToString();
        }

        private void btn_submit_Click(object sender, EventArgs e)
        {
            if (highscoreC == 1)
            {
                string path = @"Highscore/Easy.txt";
                string text = "\nName: " + txt_name.Text + "\n" + "Lives Remaining: out of 10 \n --------------------";
                File.AppendAllText(path, text);
            }
            if (highscoreC == 2)
            {
                string path = @"C:\Users\Mikkel\Documents\GitHub\TowerDefense\TowerDefense\TowerDefense\Highscore\Medium.txt";
                string text = "\nName: " + txt_name.Text + "\n" + "Lives Remaining:  out of 5\n --------------------";
                File.AppendAllText(path, text);
            }
            if (highscoreC == 3)
            {
                string path = @"C:\Users\Mikkel\Documents\GitHub\TowerDefense\TowerDefense\TowerDefense\Highscore\Hard.txt";
                string text = "\nName: " + txt_name.Text + "\n" + "Lives Remaining: out of 3\n --------------------";
                File.AppendAllText(path, text);
            }
        }
        #endregion
        // Btn Exit
        private void btn_exit_Click(object sender, EventArgs e)
        {
            DialogResult dlgRes;
            dlgRes = MessageBox.Show("Are you sure you'll exit?", "Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (dlgRes == DialogResult.Yes)
            {
                Form1.ActiveForm.Close();
            }
        }
        // Rempves the GUI
        void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            Graphics g = CreateGraphics();
            if (e.Button == MouseButtons.Left)
            {
                drawBuildGUI = 0;
                gw.tl.Clear();
                gw.guiRect = new RectangleF(0, 0, 10, 10);
            }
            guiIsClicked = false;
        }
        // Creates The GUI
        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                guiIsClicked = true;


                foreach (Environment environment in gw.environment)
                {
                    if (gw.mouseRect.IntersectsWith(environment.CollisionRect))
                    {
                        if (environment is Water)
                        {
                            drawBuildGUI = 3;
                        }
                        if (environment is Island)
                        {
                            drawBuildGUI = 4;
                        }
                    }
                }
                foreach (Tower tower in gw.towers)
                {
                    if (gw.mouseRect.IntersectsWith(tower.CollisionRect))
                    {
                        if (tower is Tower)
                        {
                            drawBuildGUI = 2;
                        }
                    }
                }
                //Set position

                guiPos = ActiveForm.PointToClient(Cursor.Position);
            }
        }
    }
}
