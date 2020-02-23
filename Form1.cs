using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Snake
{
    public partial class Form1 : Form
    {
        Random random = new Random();
        private List<Circle> Snake = new List<Circle>();
        private Circle food = new Circle();
        private int maxxPos;
        private int maxyPos;
        public Form1()
        {
            InitializeComponent();
            new Settings();
            maxxPos = pbCanvas.Size.Width / Settings.Width;
            maxyPos = pbCanvas.Size.Height / Settings.Height;
            gameTimer.Interval = 1000 / Settings.Speed;
            gameTimer.Tick += UpdateScreen;
            gameTimer.Start();
        }
        private void StartGame()
        {
            int i = random.Next(0, maxxPos);
            int j = random.Next(0, maxyPos);
            lblGameOverfc.Visible = false;
            Snake.Clear();
            Circle head = new Circle();
            head.x = i;
            head.y = j;
            Snake.Add(head);
            new Settings();
            lblGameOverfc.Text = Settings.Score.ToString();
            GenerateFood();
        }
        private void GenerateFood()
        {
            Circle food = new Circle();
            food.x = random.Next(0, maxxPos);
            food.y = random.Next(0, maxyPos);
        }
        private void UpdateScreen(object sender , EventArgs e)
        {

        }
        private void pbCanvas_Paint(object sender, PaintEventArgs e)
        {

        }
        private void MovePlayer()
        {
            for (int i = Snake.Count - 1; i >= 0; i --)
            {
                if (i == 0)
                {
                    switch (Settings.direction)
                    {
                        case Direction.Right:
                            Snake[i].x++;
                            break;
                        case Direction.Left:
                            Snake[i].x--;
                            break;
                        case Direction.Down:
                            Snake[i].y++;
                            break;
                        case Direction.Up:
                            Snake[i].y--;
                            break;
                    }
                    if(Snake[i].x < 0 || Snake [i].y < 0 ||Snake [i].x >= maxxPos || Snake[i].y >= maxyPos)
                    {
                        Die();
                    }
                    for (int j = 1; j < Snake.Count; j++)
                    {
                        if (Snake [i].x == Snake[j].x && Snake[i].y == Snake[j].y)
                        {
                            Die();
                        }
                    }
                    if (Snake[i].y == food.y && Snake[i].x == food.x)
                    {
                        Eat();
                    }
                }
                else
                {
                    Snake[i].x = Snake[i + 1].x;
                    Snake[i].y = Snake[i + 1].y;
                }
            }
        }
        private void Die()
        {
            Settings.GameOver = true;
        }
        private void Eat()
        {
            Circle food = new Circle();
            food.x = Snake[Snake.Count - 1].x;
            food.y = Snake[Snake.Count - 1].y;
            Snake.Add(food);
            Settings.Score += Settings.Points;
            label1.Text = "Score: " + Settings.Score.ToString();
            GenerateFood();
        }
    }
}
