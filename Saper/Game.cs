using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows;
namespace Saper
{
    public class Game
    {
        private Setka gameBoard;
        private int rows;
        private int cols;
        private int mines;
        private bool isInitialized = false;
        private DateTime startTime;
        public Game() { }
        public void Init(string difficulty)
        {
            switch (difficulty.ToLower())
            {
                case "easy":
                    rows = 9;
                    cols = 9;
                    mines = 10;
                    break;
                case "medium":
                    rows = 16;
                    cols = 16;
                    mines = 40;
                    break;
                case "hard":
                    rows = 24;
                    cols = 16;
                    mines = 69;
                    break;
                default:
                    throw new ArgumentException("Unknown difficulty level.");
            }

            gameBoard = new Setka(rows, cols);
            startTime = DateTime.Now;
            Console.WriteLine($"Game initialized with size {rows}x{cols}. Mines will be placed after the first move.");
        }

        public void FirstClick(int row, int col)
        {
            if (isInitialized)
                return;

            InitializeMines(row, col);
            isInitialized = true;
            OpenCell(row, col);
        }

        private void InitializeMines(int safeRow, int safeCol)
        {
            Random rand = new Random();
            int placedMines = 0;

            var forbiddenCells = GetNeighbors(safeRow, safeCol);
            forbiddenCells.Add((safeRow, safeCol)); 

            while (placedMines < mines)
            {
                int row = rand.Next(rows);
                int col = rand.Next(cols);
                if (forbiddenCells.Contains((row, col)) || gameBoard.IsMine(row, col))
                    continue;
                gameBoard.PlaceMine(row, col);
                placedMines++;
            }
            Console.WriteLine("Mines placed!");
        }


        public void Touch(int row, int col, bool leftClick)
        {
            if (!isInitialized)
            {
                FirstClick(row, col);
                return;
            }

            if (leftClick)
            {
                OpenCell(row, col);
            }
            else
            {
                ToggleFlag(row, col);
            }
        }

        private void OpenCell(int row, int col)
        {
            if (!gameBoard.IsInBounds(row, col))
            {
                Console.WriteLine("Invalid cell.");
                return;
            }

            gameBoard.OpenCell(row, col);
        }

        private void ToggleFlag(int row, int col)
        {
            if (!gameBoard.IsInBounds(row, col))
            {
                Console.WriteLine("Invalid cell.");
                return;
            }

            gameBoard.ToggleFlag(row, col);
        }

        private List<(int, int)> GetNeighbors(int row, int col)
        {
            var neighbors = new List<(int, int)>();
            for (int i = row - 1; i <= row + 1; i++)
            {
                for (int j = col - 1; j <= col + 1; j++)
                {
                    if (gameBoard.IsInBounds(i, j) && !(i == row && j == col))
                        neighbors.Add((i, j));
                }
            }
            return neighbors;
        }

        public int GetRows() => gameBoard.Rows;

        public int GetCols() => gameBoard.Cols;

        public char GetCellState(int row, int col)
        {
            if (!gameBoard.IsInBounds(row, col))
                throw new ArgumentOutOfRangeException("Координаты вне границ игрового поля");

            var cell = gameBoard.GetCell(row, col);
            if (cell.IsRevealed)
            {
                return cell.IsMine ? 'M' : cell.AdjacentMines.ToString()[0];
            }
            return cell.IsFlagged ? 'F' : '#';
        }

        public int GetTotalMines() => mines;

        public bool IsGameOver()
        {
            return false; 
        }

        public string GetDifficulty()
        {
            if (rows == 9 && cols == 9 && mines == 10) return "easy";
            if (rows == 16 && cols == 16 && mines == 40) return "medium";
            if (rows == 24 && cols == 16 && mines == 69) return "hard";
            return "unknown";
        }
        public int GetElapsedTime()
        {
            return (int)(DateTime.Now - startTime).TotalSeconds;
        }
    }
}
