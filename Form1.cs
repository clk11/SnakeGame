using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Timers;
namespace SnakeGame
{
    public partial class Form1 : Form
    {

        private string direction = "Right";
        private System.Timers.Timer myTimer = new System.Timers.Timer();
        private List<Control> Snake = new List<Control>();
        private Control Food { get; set; }
        public string DirectionException { get; set; }       
        public Form1()
        {
            InitializeComponent();
            //===========================                      
            this.newFood();
            this.button1.LocationChanged += new EventHandler(this.FindingTheFood);
            this.GoRight();
            StartTimer();            
        }       
        private void newFood()
        {
            Button x = new Button();
            x.BackColor = System.Drawing.Color.Orange;
            x.Enabled = false;
            x.Name = "Food";
            x.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;           
            x.Size = new System.Drawing.Size(18, 20);
            x.TabIndex = 2;
            x.UseVisualStyleBackColor = false;
            Random RandomForX = new Random();
            Random RandomForY = new Random();
            x.Location = new Point(RandomForX.Next(584), RandomForY.Next(576));
            Controls.Add(x);
            this.groupBox1.Controls.Add(x);
            this.Food = x;
        }
        private bool GameOver2()
        {
            bool validation = false;
            for (int i = 0; i < this.Snake.Count; i++)
            {
                int DifferenceX = this.maxPozition(this.Snake[i].Location.X, this.button1.Location.X) - this.minPozition(this.Snake[i].Location.X, this.button1.Location.X);
                int DifferenceY = this.maxPozition(this.Snake[i].Location.Y, this.button1.Location.Y) - this.minPozition(this.Snake[i].Location.Y, this.button1.Location.Y);
                if (DifferenceX <= 10 && DifferenceY <= 10)
                {
                    validation = true;
                    this.myTimer.Stop();
                    break;
                }                       
            }
            return validation;
        }
        private bool GameOver()
        {
            if ((this.button1.Location.X <= -14 || this.button1.Location.X >= 596) || (this.button1.Location.Y <= -14 || this.button1.Location.Y >= 586))
            {
                myTimer.Stop();
                myTimer.Dispose();
                return true;
            }
            else
            {
                return false;
            }
        }
        private void ExitButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void GoRight()
        {
            int oldOne = this.button1.Location.Y;
            int i = this.button1.Location.X;
            i += 10;
            this.button1.Location = new Point(i, oldOne);
        }
        private void GoDown()
        {
            int oldOne = this.button1.Location.X;
            int i = this.button1.Location.Y;
            i += 10;
            this.button1.Location = new Point(oldOne, i);
        }
        private void GoLeft()
        {
            int oldOne = this.button1.Location.Y;
            int i = button1.Location.X;
            i -= 10;
            this.button1.Location = new Point(i, oldOne);
        }
        private void GoUp()
        {
            int oldOne = this.button1.Location.X;
            int i = this.button1.Location.Y;
            i -= 10;
            this.button1.Location = new Point(oldOne, i);
        }
        private void ExitButton_KeyDown(object sender, KeyEventArgs e)
        {            
            this.GameOver();
            if (e.KeyCode.ToString() != this.DirectionException.ToString())
            {
                if (e.KeyCode == Keys.W)
                {
                    this.direction = "Up";
                }
                if (e.KeyCode == Keys.S)
                {
                    this.direction = "Down";
                }
                if (e.KeyCode == Keys.A)
                {
                    this.direction = "Left";
                }
                if (e.KeyCode == Keys.D)
                {
                    this.direction = "Right";
                }
            }
        }
        private void Movement()
        {       
            if (this.direction == "Right")
            {
                this.GoRight();
                this.DirectionException = "A";
            }
            if (this.direction == "Left")
            {
                this.GoLeft();
                this.DirectionException = "D";
            }
            if (this.direction == "Up")
            {
                this.GoUp();
                this.DirectionException = "S";
            }
            if (this.direction == "Down")
            {
                this.GoDown();
                this.DirectionException = "W";
            }
            if (this.GameOver2() == true)
            {
                MessageBox.Show("Game Over");
                Application.Restart();
            }
            if (this.GameOver() == true)
            {
                MessageBox.Show("Game Over");
                Application.Restart();
            }
        }
        private void StartTimer()
        {
            myTimer = new System.Timers.Timer(100);
            myTimer.Elapsed += Action;
            myTimer.AutoReset = true;
            myTimer.Enabled = true;
        }
        private void Action(Object source,ElapsedEventArgs e)
        {            
            this.Movement();
            if (this.Snake.Count > 0)
            {
                Point Ptg = new Point();
                Ptg = this.Snake[this.Snake.Count - 1].Location;
                if (this.direction == "Right")
                {
                    this.Snake[this.Snake.Count - 1].Location = new Point(this.button1.Location.X - 11, this.button1.Location.Y);
                }
                if (this.direction == "Left")
                {
                    this.Snake[this.Snake.Count - 1].Location = new Point(this.button1.Location.X + 11, this.button1.Location.Y);
                }
                if (this.direction == "Up")
                {
                    this.Snake[this.Snake.Count - 1].Location = new Point(this.button1.Location.X, this.button1.Location.Y + 11);
                }
                if (this.direction == "Down")
                {
                    this.Snake[this.Snake.Count - 1].Location = new Point(this.button1.Location.X, this.button1.Location.Y - 11);
                }
                for (int i = this.Snake.Count - 2; i >= 0; i--)
                {
                    Point a = new Point();
                    a = this.Snake[i].Location;
                    this.Snake[i].Location = Ptg;
                    Ptg = a;
                }
            }
        }
        public int maxPozition(int a, int b)
        {
            if (a > b)
            {
                return a;
            }
            else
            {
                return b;
            }
        }
        public int minPozition(int a,int b)
        {
            if (a < b)
            {
                return a;
            }
            else
            {
                return b;
            }
        } 
        private void FindingTheFood(object sender,EventArgs e)
        {
            int DifferenceX = this.maxPozition(this.button1.Location.X, this.Food.Location.X) - this.minPozition(this.button1.Location.X, this.Food.Location.X);
            int DifferenceY = this.maxPozition(this.button1.Location.Y, this.Food.Location.Y) - this.minPozition(this.button1.Location.Y, this.Food.Location.Y);
            if (DifferenceX <= 11 && DifferenceY <= 11)
            {
                this.ScoreBoard.Visible = true;
                int Score = int.Parse(this.ScoreBoard.Text);
                Score += 100;
                this.ScoreBoard.Text = Score.ToString();
                Button TailComponent = new Button();
                TailComponent.BackColor = System.Drawing.Color.Brown;
                TailComponent.Enabled = false;
                TailComponent.Name = "TailComponent" + (this.Snake.Count);
                TailComponent.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
                TailComponent.Size = new System.Drawing.Size(20, 25);
                TailComponent.TabIndex = 2;
                TailComponent.UseVisualStyleBackColor = false;                
                Controls.Add(TailComponent);
                this.groupBox1.Controls.Add(TailComponent);
                if (this.direction == "Right")
                {
                    TailComponent.Location = new Point(this.button1.Location.X - 11, this.button1.Location.Y);
                }
                if (this.direction == "Left")
                {
                    TailComponent.Location = new Point(this.button1.Location.X + 11, this.button1.Location.Y);
                }
                if (this.direction == "Up")
                {
                    TailComponent.Location = new Point(this.button1.Location.X, this.button1.Location.Y + 11);
                }
                if (this.direction == "Down")
                {
                    TailComponent.Location = new Point(this.button1.Location.X, this.button1.Location.Y - 11);
                }
                this.Snake.Add(TailComponent);
                this.Food.Dispose();
                this.newFood();                
            }
        }       
    }
}
