using System;

namespace Saper
{
    public class Cell
    {
        public bool IsMine { get; set; }       // Признак мины
        public bool IsRevealed { get; set; }   // Признак раскрытой клетки
        public bool IsFlagged { get; set; }    // Признак установленного флага
        public int AdjacentMines { get; set; } // Количество мин рядом

        public Cell()
        {
            IsMine = false;
            IsRevealed = false;
            IsFlagged = false;
            AdjacentMines = 0;
        }
    }

    public class Setka
    {
        public int Rows { get; }      // Количество строк
        public int Cols { get; }      // Количество столбцов
        private Cell[,] grid;         // Игровое поле
        private bool isGameOver;
        public Setka(int rows, int cols)
        {
            Rows = rows;
            Cols = cols;
            grid = new Cell[rows, cols];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    grid[i, j] = new Cell();
                }
            }
        }

        // Проверка, находится ли координата в пределах поля
        public bool IsInBounds(int row, int col) => row >= 0 && row < Rows && col >= 0 && col < Cols;

        public void ToggleFlag(int row, int col)
        {
            if (IsInBounds(row, col) && !grid[row, col].IsRevealed)
            {
                grid[row, col].IsFlagged = !grid[row, col].IsFlagged;
            }
        }

        public void PlaceMine(int row, int col)
        {
            if (IsInBounds(row, col) && !grid[row, col].IsMine)
            {
                grid[row, col].IsMine = true;
                IncrementNeighbors(row, col);
            }
        }

        private void IncrementNeighbors(int row, int col)
        {
            for (int i = row - 1; i <= row + 1; i++)
            {
                for (int j = col - 1; j <= col + 1; j++)
                {
                    if (IsInBounds(i, j) && !grid[i, j].IsMine)
                    {
                        grid[i, j].AdjacentMines++;
                    }
                }
            }
        }

        public void OpenCell(int row, int col)
        {
            if (isGameOver || !IsInBounds(row, col) || grid[row, col].IsRevealed || grid[row, col].IsFlagged)
                return;

            var cell = grid[row, col];
            cell.IsRevealed = true;

            if (cell.IsMine)
            {
                Console.WriteLine("Game Over! You hit a mine.");
                EndGame();
                return;
            }

            if (cell.AdjacentMines == 0)
            {
                RevealEmptyCells(row, col);
            }
        }

        private void RevealEmptyCells(int row, int col)
        {
            for (int i = row - 1; i <= row + 1; i++)
            {
                for (int j = col - 1; j <= col + 1; j++)
                {
                    if (IsInBounds(i, j) && !grid[i, j].IsRevealed && !grid[i, j].IsMine)
                    {
                        grid[i, j].IsRevealed = true;

                        if (grid[i, j].AdjacentMines == 0)
                        {
                            RevealEmptyCells(i, j);
                        }
                    }
                }
            }
        }

        private void EndGame()
        {
            isGameOver = true;

            // Раскрываем всё поле
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Cols; j++)
                {
                    grid[i, j].IsRevealed = true;
                }
            }

            // Сообщение о проигрыше
            Console.WriteLine("Game Over. Press the difficulty level to start a new game.");
            PrintGrid();
        }

        public void PrintGrid()
        {
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Cols; j++)
                {
                    var cell = grid[i, j];
                    if (cell.IsRevealed)
                    {
                        Console.Write(cell.IsMine ? "M " : $"{cell.AdjacentMines} ");
                    }
                    else if (cell.IsFlagged)
                    {
                        Console.Write("F ");
                    }
                    else
                    {
                        Console.Write("# ");
                    }
                }
                Console.WriteLine();
            }
        }
        public Cell GetCell(int row, int col)
        {
            if (!IsInBounds(row, col))
                throw new ArgumentOutOfRangeException("Координаты вне границ игрового поля");
            return grid[row, col];
        }

        public bool IsMine(int row, int col)
        {
            if (!IsInBounds(row, col))
                return false;
            return grid[row, col].IsMine;
        }

    }
}
