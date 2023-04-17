using System;

namespace Minesweeper
{
    internal class SimpleTile : Tile
    {
        public SimpleTile(int tileLeft, int tileTop) : base(tileLeft, tileTop)
        {
            Click += new EventHandler(TileClick);
        }

        private void TileClick(object sender, EventArgs e)
        {
            int Left = (sender as Tile).Left / GameManager.tileSize;
            int Top = (sender as Tile).Top / GameManager.tileSize;
            GameManager.OpenTiles(Left, Top);
            GameManager.CheckMap();
        }
    }
}
