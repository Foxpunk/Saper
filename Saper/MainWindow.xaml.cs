using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using System.IO;
namespace Saper
{

    public partial class MainWindow : Window
    {
        private TableRecord tableRecord;
        private Game game;
        private Button[,] buttons;
        private DispatcherTimer timer;
        private int elapsedTime;
        private int remainingMines;

        public MainWindow()
        {
            InitializeComponent();
            tableRecord = new TableRecord();
        }
        private void StartGameFromSave()
        {
            SetWindowSize(game.GetDifficulty());
            GenerateGrid();
            UpdateGrid();
            remainingMines = game.GetTotalMines();
            MinesLeftText.Text = $"Mines Left: {remainingMines}";
            TimerText.Text = $"Time: {game.GetElapsedTime()}";  

            // Запуск таймера
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += (sender, e) =>
            {
                elapsedTime++;
                TimerText.Text = $"Time: {elapsedTime}";
            };
            timer.Start();
        }

        private void Easy_Click(object sender, RoutedEventArgs e)
        {
            StartGame("easy");
        }

        private void Medium_Click(object sender, RoutedEventArgs e)
        {
            StartGame("medium");
        }

        private void Hard_Click(object sender, RoutedEventArgs e)
        {
            StartGame("hard");
        }


        private void StartGame(string difficulty)
        {
            game = new Game();
            game.Init(difficulty);
            SetWindowSize(difficulty);
            GenerateGrid();
            remainingMines = game.GetTotalMines();  
            elapsedTime = 0;

            GameGrid.Visibility = Visibility.Visible;

            MinesLeftText.Text = $"Mines: {remainingMines}";
            TimerText.Text = "Time: 0";

            VictoryMessage.Visibility = Visibility.Collapsed;
            MinesLeftText.Visibility = Visibility.Visible;
            TimerText.Visibility = Visibility.Visible;

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += (sender, e) =>
            {
                elapsedTime++;
                TimerText.Text = $"Time: {elapsedTime}";
            };
            timer.Start();
        }

        private void SetWindowSize(string difficulty)
        {
            int rows = game.GetRows();
            int cols = game.GetCols();
            int buttonSize = 0;
            if (game.GetRows() == 9)
            {
                buttonSize = 50; 
            }
            else if (game.GetRows() == 16)
            {
                buttonSize = 40;  
            }
            else
            {
                buttonSize = 30;  
            }
            this.Width = cols * buttonSize + 50;  
            this.Height = rows * buttonSize + 150; 
            this.MinWidth = 400;
            this.MinHeight = 500;
        }


        private void GenerateGrid()
        {
            GameGrid.Children.Clear();
            GameGrid.RowDefinitions.Clear();
            GameGrid.ColumnDefinitions.Clear();

            int rows = game.GetRows();
            int cols = game.GetCols();
            buttons = new Button[rows, cols];

            double buttonSize = 0;
            if (game.GetRows() == 9)
            {
                buttonSize = 50; 
            }
            else if (game.GetRows() == 16)
            {
                buttonSize = 40; 
            }
            else
            {
                buttonSize = 30;  
            }
            for (int i = 0; i < rows; i++)
                GameGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(buttonSize) });

            for (int j = 0; j < cols; j++)
                GameGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(buttonSize) });

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    Button button = new Button
                    {
                        Tag = new Tuple<int, int>(i, j),
                        Content = "",  
                        FontSize = 16,
                        FontWeight = FontWeights.Bold,
                        Width = buttonSize, 
                        Height = buttonSize,  
                        Margin = new Thickness(0), 
                        HorizontalContentAlignment = HorizontalAlignment.Center,  
                        VerticalContentAlignment = VerticalAlignment.Center 
                    };

                    button.Click += Cell_Click;
                    button.MouseRightButtonDown += Cell_RightClick;
                    Grid.SetRow(button, i);
                    Grid.SetColumn(button, j);
                    GameGrid.Children.Add(button);
                    buttons[i, j] = button;
                }
            }
        }

        private void Cell_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            var coordinates = (Tuple<int, int>)button.Tag;
            int row = coordinates.Item1;
            int col = coordinates.Item2;

            game.Touch(row, col, leftClick: true);
            if (game.IsGameOver())
            {
                ShowLossMessage();
            }
            else
            {
                UpdateGrid();
            }
        }

        private void ShowLossMessage()
        {
            VictoryMessage.Text = "Вы проиграли! Попробуйте снова.";
            VictoryMessage.Visibility = Visibility.Visible;
            MinesLeftText.Visibility = Visibility.Collapsed;
            TimerText.Visibility = Visibility.Collapsed;
            timer.Stop();
        }
        private void Cell_RightClick(object sender, MouseButtonEventArgs e)
        {
            Button button = sender as Button;
            var coordinates = (Tuple<int, int>)button.Tag;
            int row = coordinates.Item1;
            int col = coordinates.Item2;

            game.Touch(row, col, leftClick: false);
            UpdateGrid();
        }

        private void UpdateGrid()
        {
            int rows = game.GetRows();
            int cols = game.GetCols();
            bool isGameWon = true;

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    char cellState = game.GetCellState(i, j);
                    Button button = buttons[i, j];

                    if (cellState == '#')
                    {
                        button.Content = null;
                        button.IsEnabled = true;
                    }
                    else if (cellState == 'F')
                    {
                        button.Content = "F";
                        button.Foreground = Brushes.Red;
                    }
                    else if (cellState == 'M')
                    {
                        button.Content = "M";
                        button.Foreground = Brushes.Black;
                        button.IsEnabled = false;
                    }
                    else
                    {
                        if (cellState != '0')
                        {
                            button.Content = cellState.ToString();
                            button.Foreground = GetColorForNumber(cellState);
                        }
                        else
                        {
                            button.Content = null;
                        }

                        button.IsEnabled = false;
                    }

                    if ((cellState == 'M' && !button.Content.ToString().Equals("F")) ||
                        (cellState != 'M' && cellState != 'F' && cellState != '0' && button.IsEnabled))
                    {
                        isGameWon = false;
                    }
                }
            }
            if (isGameWon)
            {
                timer.Stop();
                ShowNameInputWindow(game.GetDifficulty(), elapsedTime);
                VictoryMessage.Text = "Поздравляем! Вы выиграли!";
                VictoryMessage.Visibility = Visibility.Visible;
            }

        }
        private void ShowTableWindow(object sender, RoutedEventArgs e)
        {
            var tableWindow = new TableWindow(tableRecord.GetRecords());
            tableWindow.ShowDialog();
        }
        private Brush GetColorForNumber(char number)
        {
            switch (number)
            {
                case '1': return Brushes.Blue;
                case '2': return Brushes.Green;
                case '3': return Brushes.Red;
                case '4': return Brushes.DarkBlue;
                case '5': return Brushes.Brown;
                case '6': return Brushes.Cyan;
                case '7': return Brushes.Black;
                case '8': return Brushes.Gray;
                default: return Brushes.Black;
            }
        }
        private void ShowNameInputWindow(string difficulty, int time)
        {
            var inputWindow = new NameInputWindow();
            if (inputWindow.ShowDialog() == true)
            {
                string playerName = inputWindow.PlayerName;
                if (!string.IsNullOrWhiteSpace(playerName))
                {
                    tableRecord.UpdateRecord(playerName, difficulty, time);
                }
            }
        }


    }
}
