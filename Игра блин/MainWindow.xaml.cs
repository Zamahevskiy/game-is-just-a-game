using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Threading;
namespace Игра_блин
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool goLeft, goRight, goUp, goDown;
        int speed = 3;
        DispatcherTimer gameTimer = new DispatcherTimer();
        static int ochki;
        static int point;
        Ellipse bonus;
        Rectangle stena;
        const int Size = 50;

        public MainWindow()
        {
            InitializeComponent();
            
            Canvas1.Focus();
            gameTimer.Tick += GameTimerEvent;
            gameTimer.Interval = TimeSpan.FromMilliseconds(10);
            gameTimer.Start();
            DrawGameArea();
            
           



        }
        public void GameTimerEvent(object sender, EventArgs e)
         {
            if (goLeft == true && Canvas.GetLeft(player) > 5)
            {
                Canvas.SetLeft(player, Canvas.GetLeft(player) - speed);
            }
            if (goRight == true && Canvas.GetLeft(player) + (player.Width + 30) < Application.Current.MainWindow.Width)
            {
                Canvas.SetLeft(player, Canvas.GetLeft(player) + speed);
            }
            if (goUp == true && Canvas.GetTop(player) > 5)
            {
                Canvas.SetTop(player, Canvas.GetTop(player) - speed);
            }
            if (goDown == true && Canvas.GetTop(player) + (player.Height + 30) < Application.Current.MainWindow.Height)
            {
                Canvas.SetTop(player, Canvas.GetTop(player) + speed);
            }
        }
        private void DrawGameArea() 
        {
            ochki = 0;
            point = 0;
            bool doneDrawingBackground = false;
            int nextX = 0, nextY = 0;
            int rowCounter = 0;
            Random random = new Random();
        
            while (doneDrawingBackground == false)
            {
                int rand0 = random.Next(1, 10);
                int rand1 = random.Next(1, 11);
                Rectangle rect = new Rectangle
                {
                    Width = Size,
                    Height = Size,
                    Stroke = Brushes.Gray
                };
                bonus = new Ellipse
                {
                    Width = 20,
                    Height = 20,
                    Fill = Brushes.Green
                };
                stena = new Rectangle
                {
                    Width = Size,
                    Height = Size,
                    Fill = Brushes.Black
                };
               Canvas1.Children.Add(rect);
                Canvas.SetTop(rect, nextY);
                Canvas.SetLeft(rect, nextX);
                nextX += Size;
                if (nextX >= 750)
                {
                    nextX = 0;
                    nextY += Size;
                    rowCounter++;
                }
                if (nextY >= 550)
                    doneDrawingBackground = true;
                if (rand0 > 7)
                {
                    Canvas1.Children.Add(stena);
                    Canvas.SetTop(stena, nextY);
                    Canvas.SetLeft(stena, nextX);
                }
                if (rand1 > 8)
                {
                    if (Canvas.GetTop(stena) != nextY && Canvas.GetLeft(stena) != nextX)
                    {
                        Canvas1.Children.Add(bonus);
                        Canvas.SetTop(bonus, nextY + 15);
                        Canvas.SetLeft(bonus, nextX + 15);
                        ochki++;
                    }
                }
                ScoreLabel.Content = $"Счет 0 из {ochki}";
            }
        }
       
        public void KeyIsDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left)
            {
                goLeft = true;
            }
            if (e.Key == Key.Right)
            {
                goRight = true;
            }
            if (e.Key == Key.Up)
            {
                goUp = true;
            }
            if (e.Key == Key.Down)
            {
                goDown = true;
            }
        }
        public void KeyIsUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left)
            {
                goLeft = false;
            }
            if (e.Key == Key.Right)
            {
                goRight = false;

            }
            if (e.Key == Key.Up)
            {
                goUp = false;
            }
            if (e.Key == Key.Down)
            {
                goDown = false;
            }
        }
        private void Restart()
        {
            Canvas1.Children.Clear();
            DrawGameArea();
            ScoreLabel.HorizontalAlignment = HorizontalAlignment.Left;
            ScoreLabel.VerticalAlignment = VerticalAlignment.Top;
        }
        public void Window_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            for (int i = 1; i < Canvas1.Children.Count; i++)
            {
                UIElement child = Canvas1.Children[i];
                int bon = 5;
                int blok = 10;
                if (Canvas.GetLeft(player) == Canvas.GetLeft(child) - bon && Canvas.GetTop(player) == Canvas.GetTop(child) - bon && (child as Ellipse).Fill == Brushes.Green) //получение бонуса
                {
                    Canvas1.Children.Remove(child);
                    point++;
                }
                ScoreLabel.Content = $"Счет {point} из {ochki}";
                if (Canvas.GetLeft(player) == Canvas.GetLeft(child) + blok && Canvas.GetTop(player) == Canvas.GetTop(child) + blok && (child as Rectangle).Fill == Brushes.Black) //столкновение со стеной
                {
                    MessageBox.Show("Это конец! Ты всё про....!");
                    Restart();
                }
                if (point == ochki) 
                {
                    ScoreLabel.HorizontalAlignment = HorizontalAlignment.Center;
                    ScoreLabel.VerticalAlignment = VerticalAlignment.Center;
                    MessageBox.Show("Поздравляю! Вы прошли игру");
                    Restart();
                }
            }
        }
    }
}