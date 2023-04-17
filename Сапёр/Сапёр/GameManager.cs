using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Minesweeper
{
    public static class GameManager
    {
        public const int tileSize = 25;

        public static int mapSize = 0;
        public static int bombsQuantity = 0;
        public static int flagCounter = 0;

        public static Form1 form1;

        public static Tile[,] tileMap;

        public static Panel gameField;

        public static Label flagLabel;

        public static void GameInitialization(int _mapSize, int _bombsQuantity)
        {
            mapSize = _mapSize;
            bombsQuantity = _bombsQuantity;
            flagCounter = _bombsQuantity;
            tileMap = new Tile[mapSize, mapSize];
            CreateField();
            CreateTiles();
            CountTileBombScore();
        }

        private static void CreateField()
        {
            Panel panel = new Panel()
            {
                Size = new Size(tileSize * mapSize, tileSize * mapSize),
                Left = 0,
                Top = 0
            };

            form1.Controls.Add(panel);
            form1.ClientSize = new Size(tileSize * mapSize + 200, tileSize * mapSize);
            gameField = panel;
            CreateMenu();
        }

        private static void CreateMenu()
        {
            flagLabel = new Label()
            {
                Size = new Size(160, 30),
                Top = 30,
                Left = tileSize * mapSize + 20,
                Text = $"Flags remaining: {flagCounter}",
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("Microsoft Ya Hei", 12, FontStyle.Regular)
            };

            Button button = new Button()
            {
                Size = new Size(100, 30),
                Top = 80,
                Left = tileSize * mapSize + 50,
                Text = "New Game",
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Microsoft Ya Hei", 9, FontStyle.Regular),
            };
            button.Click += ResetGame;
            form1.Controls.Add(flagLabel);
            form1.Controls.Add(button);
        }

        private static void ResetGame(object sender, EventArgs e)
        {
            for (int i = 0; i < mapSize; i++)
            {
                for (int j = 0; j < mapSize; j++)
                {
                    tileMap[i, j].Dispose();
                }
            }

            form1.RemoveInterface();
            form1.PreInitializationInterface();
        }

        private static void CreateTiles()
        {
            List<int> bombList = RandomizeBombs();

            for (int i = 0; i < mapSize; i++)
            {
                for (int j = 0; j < mapSize; j++)
                {
                    if (bombList.Contains((i * mapSize) + j))
                    {
                        BombTile tile = new BombTile(i * tileSize, j * tileSize);
                        gameField.Controls.Add(tile);
                        tileMap[i, j] = tile;
                    }
                    else
                    {
                        SimpleTile tile = new SimpleTile(i * tileSize, j * tileSize);
                        gameField.Controls.Add(tile);
                        tileMap[i, j] = tile;
                    }
                }
            }
        }

        private static List<int> RandomizeBombs()
        {
            Random random = new Random();
            List<int> bombList = new List<int>();
            int tilesQuantity = mapSize * mapSize;

            while (bombList.Count < bombsQuantity)
            {
                int r = random.Next(0, tilesQuantity);
                if (bombList.Contains(r))
                    continue;
                else
                    bombList.Add(r);
            }

            return bombList;
        }

        private static void CountTileBombScore()
        {
            for (int i = 0; i < mapSize; i++)
            {
                for (int j = 0; j < mapSize; j++)
                {
                    if (!(tileMap[i, j].bombScore < 0))
                        continue;

                    for (int k = i - 1; k < i + 2; k++)
                    {
                        for (int l = j - 1; l < j + 2; l++)
                        {
                            if (OutOfBorder(k, l) || tileMap[k, l].bombScore < 0)
                                continue;
                            tileMap[k, l].bombScore += 1;
                        }
                    }
                }
            }
        }

        private static bool OutOfBorder(int x, int y)
        {
            if (x < 0 || y < 0 || x > mapSize - 1 || y > mapSize - 1)
            {
                return true;
            }

            return false;
        }

        private static void OpenTile(int x, int y)
        {
            tileMap[x, y].Enabled = false;
            int a = tileMap[x, y].bombScore;
            tileMap[x, y].Text = a.ToString();
        }

        public static void OpenTiles(int x, int y)
        {
            if (tileMap[x, y].hasFlag)
                return;

            OpenTile(x, y);

            for (int i = x - 1; i < x + 2; i++)
            {
                for (int j = y - 1; j < y + 2; j++)
                {
                    if (OutOfBorder(i, j) || !tileMap[i, j].Enabled || tileMap[i, j].hasFlag)
                        continue;
                    if (tileMap[i, j].bombScore == 0)
                        OpenTiles(i, j);
                    else if (tileMap[i, j].bombScore > 0)
                        OpenTile(i, j);
                }
            }
        }

        public static void GameOver()
        {
            gameField.Enabled = false;
            ShowBombs();
            MessageBox.Show("Game Over");
        }

        private static void ShowBombs()
        {
            for (int i = 0; i < mapSize; i++)
            {
                for (int j = 0; j < mapSize; j++)
                {
                    if (tileMap[i, j].bombScore < 0)
                    {
                        tileMap[i, j].Text = "*";
                    }
                }
            }
        }

        public static void CheckMap()
        {
            if (flagCounter != 0)
                return;
            int tilesQuantity = mapSize * mapSize;
            int counter = 0;
            for (int i = 0; i < mapSize; i++)
            {
                for (int j = 0; j < mapSize; j++)
                {
                    if (tileMap[i, j].bombScore < 0 && tileMap[i, j].hasFlag || !tileMap[i, j].Enabled)
                        counter++;
                }
            }

            if (counter == tilesQuantity)
            {
                MessageBox.Show("You Win!");
                gameField.Enabled = false;
            }
        }
    }
}
