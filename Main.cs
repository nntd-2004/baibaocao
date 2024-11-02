using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using WMPLib;

namespace SnakeGames
{
    public partial class Main : Form
    {
        private readonly WindowsMediaPlayer backgroundMusic = new WindowsMediaPlayer();
        private List<Circle> Snake = new List<Circle>();
        private Circle food = new Circle();
        private List<Circle> trapFoods = new List<Circle>(); 
        private List<HighCoreModel> highCoreModels = new List<HighCoreModel>();
        private Timer trapFoodTimer;
        private Random rnd = new Random();

        public Main()
        {
            InitializeComponent();
            PlayBackgroundMusic();
            new Settings();

            gameTimer.Interval = 1000 / Settings.Speed;
            gameTimer.Tick += updateSreen;
            gameTimer.Start();

            trapFoodTimer = new Timer();
            trapFoodTimer.Interval = 10000; 
            trapFoodTimer.Tick += TrapFoodTimer_Tick;
            trapFoodTimer.Start();

            startGame();
            Activate();
            easy.Text = "✓ Dễ";
            pbCanvas.Focus();
            Settings.GameOver = true;
            highCoreModels = Utils.ReadFileHighScore();
        }

        private void PlayBackgroundMusic()
        {
            try
            {
                backgroundMusic.URL = "D:\\exercise18\\soud\\game ran.mp3";
                backgroundMusic.settings.volume = 100;
                backgroundMusic.settings.setMode("loop", true);
                backgroundMusic.controls.play();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi phát nhạc: " + ex.Message);
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            backgroundMusic.controls.stop();
            trapFoodTimer.Stop();
            base.OnFormClosing(e);
        }

        private void TrapFoodTimer_Tick(object sender, EventArgs e)
        {
            if (trapFoods.Count < 15 ) 
            {
                generateTrapFood();
            }
        }

        private void updateSreen(object sender, EventArgs e)
        {
            if (Settings.GameOver)
            {
                if (Input.KeyPress(Keys.Enter))
                {
                    startGame();
                }
            }
            else
            {
                if (Input.KeyPress(Keys.Right) && Settings.direction != Directions.Left)
                {
                    Settings.direction = Directions.Right;
                }
                else if (Input.KeyPress(Keys.Left) && Settings.direction != Directions.Right)
                {
                    Settings.direction = Directions.Left;
                }
                else if (Input.KeyPress(Keys.Up) && Settings.direction != Directions.Down)
                {
                    Settings.direction = Directions.Up;
                }
                else if (Input.KeyPress(Keys.Down) && Settings.direction != Directions.Up)
                {
                    Settings.direction = Directions.Down;
                }

                movePlayer();
            }

            pbCanvas.Invalidate();
        }

        private void updateGraphics(object sender, PaintEventArgs e)
        {
            Graphics canvas = e.Graphics;

            if (!Settings.GameOver)
            {
                Brush snakeColour;

                for (int i = 0; i < Snake.Count; i++)
                {
                    snakeColour = (i == 0) ? Brushes.Black : Brushes.Green;
                    canvas.FillEllipse(snakeColour,
                                        new Rectangle(
                                            Snake[i].X * Settings.Width,
                                            Snake[i].Y * Settings.Height,
                                            Settings.Width, Settings.Height));
                }

                // Vẽ thức ăn
                canvas.FillEllipse(Brushes.Red,
                                    new Rectangle(
                                        food.X * Settings.Width,
                                        food.Y * Settings.Height,
                                        Settings.Width, Settings.Height));


                foreach (var trapFood in trapFoods)
                {
                    canvas.FillEllipse(Brushes.Blue,
                                       new Rectangle(
                                           trapFood.X * Settings.Width,
                                           trapFood.Y * Settings.Height,
                                           Settings.Width, Settings.Height));
                }
            }
            else
            {
                string gameOver = "Game Over \n" + "Điểm cuối là " + Settings.Score + "\nNhấn enter để bắt đầu lại \n";
                label3.Text = gameOver;
                label3.Visible = true;
            }
        }

        private void keyisdown(object sender, KeyEventArgs e)
        {
            Input.changeState(e.KeyCode, true);
        }

        private void keyisup(object sender, KeyEventArgs e)
        {
            Input.changeState(e.KeyCode, false);
        }

        private void startGame()
        {
            label3.Visible = false;
            new Settings();
            Snake.Clear();
            Circle head = new Circle { X = 10, Y = 5 };
            Snake.Add(head);

            label2.Text = Settings.Score.ToString();
            generateFood();
            trapFoods.Clear(); 
        }

        private void movePlayer()
        {
            for (int i = Snake.Count - 1; i >= 0; i--)
            {
                if (i == 0)
                {
                    switch (Settings.direction)
                    {
                        case Directions.Right:
                            Snake[i].X++;
                            break;
                        case Directions.Left:
                            Snake[i].X--;
                            break;
                        case Directions.Up:
                            Snake[i].Y--;
                            break;
                        case Directions.Down:
                            Snake[i].Y++;
                            break;
                    }

                    int maxXpos = pbCanvas.Size.Width / Settings.Width;
                    int maxYpos = pbCanvas.Size.Height / Settings.Height;

                    if (Snake[i].X >= maxXpos)
                    {
                        Snake[i].X = 0; 
                    }
                    else if (Snake[i].X < 0)
                    {
                        Snake[i].X = maxXpos - 1; 
                    }

                    if (Snake[i].Y < 0)
                    {
                        Snake[i].Y = maxYpos - 1; 
                    }
                    else if (Snake[i].Y >= maxYpos)
                    {
                        Snake[i].Y = 0; 
                    }

                    for (int j = 1; j < Snake.Count; j++)
                    {
                        if (Snake[i].X == Snake[j].X && Snake[i].Y == Snake[j].Y)
                        {
                            die();
                        }
                    }

                    if (Snake[0].X == food.X && Snake[0].Y == food.Y)
                    {
                        eat();
                    }

                    foreach (var trapFood in trapFoods)
                    {
                        if (Snake[0].X == trapFood.X && Snake[0].Y == trapFood.Y)
                        {
                            die(); 
                        }
                    }
                }
                else
                {
                    Snake[i].X = Snake[i - 1].X;
                    Snake[i].Y = Snake[i - 1].Y;
                }
            }
        }

        private void generateFood()
        {
            int maxXpos = pbCanvas.Size.Width / Settings.Width;
            int maxYpos = pbCanvas.Size.Height / Settings.Height;
            food = new Circle { X = rnd.Next(0, maxXpos), Y = rnd.Next(0, maxYpos) };
        }

        private void generateTrapFood()
        {
            int maxXpos = pbCanvas.Size.Width / Settings.Width;
            int maxYpos = pbCanvas.Size.Height / Settings.Height;

            Circle newTrapFood;
            do
            {
                newTrapFood = new Circle { X = rnd.Next(0, maxXpos), Y = rnd.Next(0, maxYpos) };
            } while (trapFoods.Any(t => t.X == newTrapFood.X && t.Y == newTrapFood.Y) || Snake.Any(s => s.X == newTrapFood.X && s.Y == newTrapFood.Y));

            trapFoods.Add(newTrapFood); 
        }

        private void eat()
        {
            Circle body = new Circle
            {
                X = Snake[Snake.Count - 1].X,
                Y = Snake[Snake.Count - 1].Y
            };

            Snake.Add(body);
            Settings.Score += Settings.Points;
            label2.Text = Settings.Score.ToString();
            generateFood();
        }

        private void die()
        {
            Settings.GameOver = true;
            CheckHighScore();
        }

        private void CheckHighScore()
        {
            List<HighCoreModel> SortedList = highCoreModels.OrderByDescending(o => o.Score).ToList();
            var highCore = SortedList.Where(i => i.Score <= Settings.Score).FirstOrDefault();
            if (highCore != null)
            {
                var formInputNameHighScore = new InputNameHighScore(SortedList, Settings.Score);
                formInputNameHighScore.ShowDialog();
            }
            else
            {
                if (SortedList.Count < 10)
                {
                    var formInputNameHighScore = new InputNameHighScore(SortedList, Settings.Score);
                    formInputNameHighScore.ShowDialog();
                }
            }
        }

        private void exit_application(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void hard_Click(object sender, EventArgs e)
        {
            hard.Text = "✓ Khó";
            medium.Text = "  Trung bình";
            easy.Text = "  Dễ";
            gameTimer.Interval = 1000 / (Settings.Speed * 3);
        }

        private void medium_Click(object sender, EventArgs e)
        {
            hard.Text = "  Khó";
            medium.Text = "✓ Trung bình";
            easy.Text = "  Dễ";
            gameTimer.Interval = 1000 / (Settings.Speed * 3);
        }

        private void easy_Click(object sender, EventArgs e)
        {
            hard.Text = "  Khó";
            medium.Text = "  Trung bình";
            easy.Text = "✓ Dễ";
            gameTimer.Interval = 1000 / (Settings.Speed *3);
        }

        private void historyHighScore_Click(object sender, EventArgs e)
        {
            var form = new HighScore();
            form.ShowDialog();
        }
    }
}
