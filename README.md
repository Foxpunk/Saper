# Minesweeper Game (C# WPF)

**Сапер** — классическая игра, реализованная на языке C# с использованием WPF (Windows Presentation Foundation). Цель игры — открыть все клетки поля, избегая мин.

## Описание

Игра состоит из поля с ячейками, в которых могут быть скрыты мины. Игрок должен открыть все клетки, не наткнувшись на мину. Открытые клетки могут показывать количество мин, окружающих эту клетку. Если игрок открывает клетку с миной, игра заканчивается.

## Технологии

- **Язык программирования:** C#
- **Графический интерфейс:** WPF (Windows Presentation Foundation)
- **Среда разработки:** Visual Studio 2022

## Установка и запуск

### Требования

- .NET Framework 4.7.2 или выше
- Visual Studio 2022 

### Шаги для запуска

1. Клонируйте репозиторий:
   ```bash
   git clone https://github.com/Foxpunk/Saper


## Иерархия классов

В процессе разработки игры "Сапер" были созданы следующие ключевые классы, которые реализуют функционал игры.

### 1. Класс `Cell`

Класс `Cell` представляет собой отдельную ячейку на игровом поле. Каждый объект этого класса содержит информацию о состоянии ячейки.

#### Поля:
- `bool IsMine` — указывает, содержит ли ячейка мину.
- `bool IsRevealed` — указывает, открыта ли ячейка.
- `bool IsFlagged` — указывает, помечена ли ячейка флагом.
- `int AdjacentMines` — количество мин в соседних ячейках.

#### Методы:
- `Cell()` — конструктор, инициализирующий поля по умолчанию.

### 2. Класс `Setka`

Класс `Setka` управляет игровым полем и реализует логику игры. Он инициализирует поле, обрабатывает открытия ячеек и размещение мин.

#### Поля:
- `int Rows` — количество строк на игровом поле.
- `int Cols` — количество столбцов на игровом поле.
- `Cell[,] grid` — двумерный массив объектов `Cell`, представляющий игровое поле.
- `bool isGameOver` — указывает, закончена ли игра.

#### Методы:
- `Setka(int rows, int cols)` — конструктор для инициализации поля с заданными размерами.
- `IsInBounds(int row, int col)` — проверка, находятся ли указанные координаты в пределах поля.
- `ToggleFlag(int row, int col)` — переключение флага на ячейке.
- `PlaceMine(int row, int col)` — размещение мины в ячейке.
- `OpenCell(int row, int col)` — открытие ячейки, проверка на мину.
- `PrintGrid()` — вывод состояния поля (для отладки).
- `GetCell(int row, int col)` — возвращает объект `Cell` по координатам.
- `IsMine(int row, int col)` — проверка, содержит ли ячейка мину.

### 3. Класс `TableRecord`

Класс `TableRecord` управляет таблицей рекордов и сохраняет их в файл.

#### Поля:
- `const string FileName` — имя файла для сохранения рекордов.
- `Dictionary<string, PlayerRecord> records` — словарь с данными о рекордах игроков.

#### Методы:
- `TableRecord()` — конструктор, загружает рекорды из файла.
- `LoadRecords()` — загрузка рекордов из текстового файла.
- `SaveRecords()` — сохранение рекордов в файл.
- `UpdateRecord(string name, string difficulty, int time)` — обновление рекорда игрока.
- `List<PlayerRecord> GetRecords()` — получение списка всех рекордов.

### 4. Класс `PlayerRecord`

Класс `PlayerRecord` хранит информацию о рекорде конкретного игрока.

#### Поля:
- `string Name` — имя игрока.
- `int EasyTime` — лучшее время на уровне "легкий".
- `int MediumTime` — лучшее время на уровне "средний".
- `int HardTime` — лучшее время на уровне "сложный".

#### Методы:
- `PlayerRecord(string name, int easyTime = -1, int mediumTime = -1, int hardTime = -1)` — конструктор, инициализирует рекорд игрока.
- `string GetFormattedEasyTime()` — возвращает строковое представление времени на уровне "легкий".
- `string GetFormattedMediumTime()` — возвращает строковое представление времени на уровне "средний".
- `string GetFormattedHardTime()` — возвращает строковое представление времени на уровне "сложный".
- `string ToString()` — возвращает строковое представление рекорда.

### 5. Класс `Game`

Класс `Game` управляет всей логикой игры, инициализирует поле, размещает мины, обрабатывает ходы игрока.

#### Поля:
- `Setka gameBoard` — объект игрового поля.
- `int rows` — количество строк на поле.
- `int cols` — количество столбцов на поле.
- `int mines` — количество мин на поле.
- `bool isInitialized` — флаг, указывающий, инициализировано ли поле.
- `DateTime startTime` — время начала игры.

#### Методы:
- `Init(string difficulty)` — инициализация игры в зависимости от сложности.
- `FirstClick(int row, int col)` — обработка первого клика игрока.
- `Touch(int row, int col, bool leftClick)` — обработка кликов (открытие ячеек или установка флага).
- `OpenCell(int row, int col)` — открытие ячейки.
- `ToggleFlag(int row, int col)` — переключение состояния флага.
- `List<(int, int)> GetNeighbors(int row, int col)` — получение списка соседних ячеек.
- `int GetRows()` — получение количества строк.
- `int GetCols()` — получение количества столбцов.
- `char GetCellState(int row, int col)` — получение состояния ячейки.
- `int GetTotalMines()` — возвращает количество мин на поле.
- `bool IsGameOver()` — проверка окончания игры.
- `string GetDifficulty()` — получение уровня сложности.
- `int GetElapsedTime()` — подсчет времени, прошедшего с начала игры.
