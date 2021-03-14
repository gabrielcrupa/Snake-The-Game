using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Snake
{
    public partial class Form1 : Form
    {
        private List<Circle> Snake = new List<Circle>();
        private Circle food = new Circle();

        public Form1()
        {
            InitializeComponent();

            //Setari
            new Settings();

            //Setare viteza joc
            gameTimer.Interval = 1500 / Settings.Speed;
            gameTimer.Tick += UpdateScreen;
            gameTimer.Start();

            //Start
            StartGame();
        }

        public void ChangeLabel(string label)
        {
            label1.Text = label;
        }

        private void StartGame()
        {
            lblGameOver.Visible = false;

            //Setari
            new Settings();

            //Creare sarpe 
            Snake.Clear();
            
            Circle head = new Circle();
            head.X = 10; head.Y = 5; Snake.Add(head);
            Circle body = new Circle(); body.X = 10; body.Y = 5; Snake.Add(body);
            Circle body2 = new Circle(); body2.X = 10; body2.Y = 5; Snake.Add(body2);

            lblScore.Text = Settings.Score.ToString();
            GenerateFood();

        }

        //Generare mancare
        private void GenerateFood()
        {
            int maxXPos = pbCanvas.Size.Width / Settings.Width;
            int maxYPos = pbCanvas.Size.Height / Settings.Height;

            Random random = new Random();
            food = new Circle();
            food.X = random.Next(0, maxXPos);
            food.Y = random.Next(0, maxYPos);
        }


        private void UpdateScreen(object sender, EventArgs e)
        {
            //Verificare Game Over
            if (Settings.GameOver)
            {
                //Verificare apasare Enter
                if (Input.KeyPressed(Keys.Enter))
                {
                    StartGame();
                }
            }
            else
            {
                if (Input.KeyPressed(Keys.Right) && Settings.direction != Direction.Left)
                    Settings.direction = Direction.Right;
                else if (Input.KeyPressed(Keys.Left) && Settings.direction != Direction.Right)
                    Settings.direction = Direction.Left;
                else if (Input.KeyPressed(Keys.Up) && Settings.direction != Direction.Down)
                    Settings.direction = Direction.Up;
                else if (Input.KeyPressed(Keys.Down) && Settings.direction != Direction.Up)
                    Settings.direction = Direction.Down;

                MovePlayer();
            }

            pbCanvas.Invalidate();

        }

        private void pbCanvas_Paint(object sender, PaintEventArgs e)
        {
            Graphics canvas = e.Graphics;

            if(!Settings.GameOver)
            {
                //Setare culoare 
                Brush snakeColour;

                //Desenare sarpe
                for(int i = 0; i< Snake.Count; i++ )
                {

                    if (i == 0)
                        snakeColour = Brushes.Green;     //Desenare sarpe
                    else
                        snakeColour = Brushes.Green;    //Desenare body
                    canvas.FillEllipse(snakeColour,
                        new Rectangle(Snake[i].X * Settings.Width,
                                      Snake[i].Y * Settings.Height,
                                      Settings.Width, Settings.Height));

                    //Desenare mancare
                    canvas.FillEllipse(Brushes.Orange,
                        new Rectangle(food.X * Settings.Width,
                             food.Y * Settings.Height, Settings.Width, Settings.Height));

                }
            }
            else
            {
                string gameOver = " GAME OVER! Apasati tasta Enter pentru a incepe un joc nou.";
                lblGameOver.Text = gameOver;
                lblGameOver.Visible = true;
            }
        }

        
        private void MovePlayer()
        {
            for(int i = Snake.Count -1; i >= 0; i--)
            {
                //Mutare cap
                if(i == 0)
                {
                    switch (Settings.direction)
                    {
                        case Direction.Right:
                            Snake[i].X++;
                            break;
                        case Direction.Left:
                            Snake[i].X--;
                            break;
                        case Direction.Up:
                            Snake[i].Y--;
                            break;
                        case Direction.Down:
                            Snake[i].Y++;
                            break;
                    }


                    //Obtine pozitie X,Y
                    int maxXPos = pbCanvas.Size.Width/Settings.Width;
                    int maxYPos = pbCanvas.Size.Height/Settings.Height;

                    //Detectare lovire pereti
                    if (Snake[i].X < 0 || Snake[i].Y < 0
                        || Snake[i].X >= maxXPos || Snake[i].Y >= maxYPos)
                    {
                        Die();
                    }

                    //Detectare lovire sarpe
                    for (int j = 1; j < Snake.Count; j++)
                    {
                        if(Snake[i].X == Snake[j].X && 
                           Snake[i].Y == Snake[j].Y )
                        {
                            Die();
                        }
                    }

                    //Detectare lovire mancare
                    if(Snake[0].X == food.X && Snake[0].Y == food.Y)
                    {
                        Eat();
                    }

                }
                else
                {
                    //Miscare corp sarpe
                    Snake[i].X = Snake[i - 1].X;
                    Snake[i].Y = Snake[i - 1].Y;
                }
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            Input.ChangeState(e.KeyCode, true);
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            Input.ChangeState(e.KeyCode, false);
        }
        private void Eat()
        {
            Circle circle = new Circle
            {
                X = Snake[Snake.Count - 1].X,
                Y = Snake[Snake.Count - 1].Y
            };
            Snake.Add(circle);

            Settings.Score += Settings.Points;
            lblScore.Text = Settings.Score.ToString();

            GenerateFood();
        }

        private void Die()
        {
            Settings.GameOver = true;
        }

        private void newGameToolStripMenuItem_Click(object sender, EventArgs e)
        {

            StartGame();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void aboutMeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Realizator : Crupa Gabriel\nInfo Romana An 2 \nSubgrupa 3", "Despre mine", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void instructiuniToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string instr = "Apasati tasta Enter pentru a incepe un joc nou. " +
                "Controlarea sarpelui se face din tastele sageti." + 
                "Trebuie sa mancati punctele de pe mapa si sa nu va loviti de margini.";
            string title = "Ajutor";
            MessageBox.Show( instr , title,  MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
