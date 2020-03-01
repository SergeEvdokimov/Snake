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
            StartGame();
        }
        private void StartGame()
        {
            Random random = new Random();
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
            int maxxPos = pbCanvas.Size.Width / Settings.Width;
            int maxyPos = pbCanvas.Size.Height / Settings.Height;
            Random random = new Random();
            food = new Circle();
            food.x = random.Next(0, maxxPos);
            food.y = random.Next(0, maxyPos);
        }
        private void UpdateScreen(object sender, EventArgs e)
        {
            if (Settings.GameOver)
            {
                if (Input.KeyPressed(Keys.Enter))
                {
                    StartGame();
                }
            }
            else
            {
                if(Input.KeyPressed(Keys.Up) && Settings.direction !=  Direction.Down)
                {
                    Settings.direction = Direction.Up;
                }
                else if (Input.KeyPressed(Keys.Down) && Settings.direction != Direction.Up)
                {
                    Settings.direction = Direction.Down;
                }
                else if (Input.KeyPressed(Keys.Left) && Settings.direction != Direction.Right)
                {
                    Settings.direction = Direction.Left;
                }
                else if (Input.KeyPressed(Keys.Right) && Settings.direction != Direction.Left)
                {
                    Settings.direction = Direction.Right;
                }
                MovePlayer();
            }
            pbCanvas.Invalidate();
        }
        private void Form1_KeyUp(object sender, KeyEventArgs e )
        {
            Input.ChangeState(e.KeyCode, false);
        }
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            Input.ChangeState(e.KeyCode, true);
        }
        private void pbCanvas_Paint(object sender, PaintEventArgs e)
        {
            Graphics canvas = e.Graphics;
            if(!Settings.GameOver)
            {
                Brush snakeColor;
                for(int i =0; i < Snake.Count; i ++)
                {
                    if (i == 0)
                    {
                        snakeColor = Brushes.Aqua;
                    }
                    else
                    {
                        snakeColor = Brushes.Gold;
                    }
                    canvas.FillEllipse(snakeColor,
                    new Rectangle(Snake[i].x * Settings.Width,
                    Snake[i].y * Settings.Height,
                    Settings.Width,
                    Settings.Height));
                    canvas.FillEllipse(Brushes.Blue,
                    new Rectangle(food.x * Settings.Width,
                    food.y * Settings.Height,
                    Settings.Width,
                    Settings.Height));
                }
            }
            else
            {
                string gameOver = "Игра окончена \nТвой фнальный счет:" + Settings.Score + "\nНажми Enter чтобы попробовать снова";
                lblGameOverfc.Text = gameOver;
                lblGameOverfc.Visible = true;
            }
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
                    Snake[i].x = Snake[i - 1].x;
                    Snake[i].y = Snake[i - 1].y;
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
            label1.Text = "Счет: " + Settings.Score.ToString();
            GenerateFood();
        }
    }
}
