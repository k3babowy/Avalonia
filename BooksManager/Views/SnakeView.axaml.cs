using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.Controls.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BooksManager.Views
{
    public partial class SnakeView : UserControl
    {
        private const int SnakeSquareSize = 20;
        private const int SnakeStartLength = 5;
        private const int MoveInterval = 150;
        private const int GameWidth = 800;
        private const int GameHeight = 450;

        private List<Point> snake;
        private Point food;
        private Direction currentDirection;
        private bool isGameRunning;
        private int score;

        private Canvas gameCanvas;
        private TextBlock gameOverText;
        private TextBlock scoreText;

        public SnakeView()
        {
            InitializeComponent();
            snake = new List<Point>();
            gameCanvas = this.FindControl<Canvas>("GameCanvas");
            gameOverText = this.FindControl<TextBlock>("GameOverText");
            scoreText = this.FindControl<TextBlock>("ScoreText");
            KeyDown += OnKeyDown;
            Focusable = true;
            StartGame();
        }

        public void StartGame()
        {
            snake.Clear();
            int startX = GameWidth / 2;
            int startY = GameHeight / 2;

            for (int i = 0; i < SnakeStartLength; i++)
            {
                snake.Add(new Point(startX - i * SnakeSquareSize, startY));
            }
            currentDirection = Direction.Right;
            GenerateFood();
            isGameRunning = true;
            score = 0;
            gameOverText.IsVisible = false;
            scoreText.Text = $"Score: {score}";
            _ = GameLoop();
        }

        private async Task GameLoop()
        {
            while (isGameRunning)
            {
                MoveSnake();
                await Task.Delay(MoveInterval);
            }
        }

        private void MoveSnake()
        {
            if (!isGameRunning) return;

            Point head = snake[0];
            Point newHead = GetNextHeadPosition(head);

            if (newHead.X < 0 || newHead.X >= GameWidth || newHead.Y < 0 || newHead.Y >= GameHeight)
            {
                GameOver();
                return;
            }

            if (IsSnakeCollision(newHead))
            {
                GameOver();
                return;
            }

            snake.Insert(0, newHead);


            if (IsHeadCollidingWithFood())
            {
                score++;
                scoreText.Text = $"Score: {score}";
                GenerateFood();
            }
            else
            {
                snake.RemoveAt(snake.Count - 1);
            }

            UpdateGameCanvas();
        }

        private bool IsHeadCollidingWithFood()
        {
            Point head = snake[0];

            return head.X >= food.X && head.X < food.X + SnakeSquareSize &&
                   head.Y >= food.Y && head.Y < food.Y + SnakeSquareSize;
        }

        private Point GetNextHeadPosition(Point currentHead)
        {
            Point newHead = currentHead;

            switch (currentDirection)
            {
                case Direction.Up:
                    newHead = new Point(currentHead.X, currentHead.Y - SnakeSquareSize);
                    break;
                case Direction.Down:
                    newHead = new Point(currentHead.X, currentHead.Y + SnakeSquareSize);
                    break;
                case Direction.Left:
                    newHead = new Point(currentHead.X - SnakeSquareSize, currentHead.Y);
                    break;
                case Direction.Right:
                    newHead = new Point(currentHead.X + SnakeSquareSize, currentHead.Y);
                    break;
            }

            return newHead;
        }

        private bool IsSnakeCollision(Point head)
        {
            return snake.Skip(1).Any(segment => segment == head);
        }

        private void GenerateFood()
        {
            Random random = new Random();
            int maxX = GameWidth / SnakeSquareSize;
            int maxY = GameHeight / SnakeSquareSize;

            int foodX, foodY;
            do
            {
                foodX = random.Next(0, maxX) * SnakeSquareSize;
                foodY = random.Next(0, maxY) * SnakeSquareSize;
            } while (snake.Any(segment => segment.X == foodX && segment.Y == foodY));

            food = new Point(foodX, foodY);
            UpdateGameCanvas();
        }


        private void UpdateGameCanvas()
        {
            gameCanvas.Children.Clear();

            foreach (var segment in snake)
            {
                DrawSnakeSegment(segment);
            }

            DrawFood(food);
        }

        private void DrawSnakeSegment(Point segment)
        {
            var snakePart = new Rectangle
            {
                Width = SnakeSquareSize,
                Height = SnakeSquareSize,
                Fill = Brushes.Green
            };
            Canvas.SetLeft(snakePart, segment.X);
            Canvas.SetTop(snakePart, segment.Y);
            gameCanvas.Children.Add(snakePart);
        }

        private void DrawFood(Point foodPosition)
        {
            var foodRectangle = new Rectangle
            {
                Width = SnakeSquareSize,
                Height = SnakeSquareSize,
                Fill = Brushes.Red
            };
            Canvas.SetLeft(foodRectangle, foodPosition.X);
            Canvas.SetTop(foodRectangle, foodPosition.Y);
            gameCanvas.Children.Add(foodRectangle);
        }

        private void GameOver()
        {
            isGameRunning = false;
            gameOverText.IsVisible = true;
        }

        private void RestartGame()
        {
            StartGame();
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    RestartGame();
                    break;
                case Key.Up:
                    if (currentDirection != Direction.Down)
                        currentDirection = Direction.Up;
                    break;
                case Key.Down:
                    if (currentDirection != Direction.Up)
                        currentDirection = Direction.Down;
                    break;
                case Key.Left:
                    if (currentDirection != Direction.Right)
                        currentDirection = Direction.Left;
                    break;
                case Key.Right:
                    if (currentDirection != Direction.Left)
                        currentDirection = Direction.Right;
                    break;
            }
        }

        private enum Direction
        {
            Up,
            Down,
            Left,
            Right
        }
    }
}
