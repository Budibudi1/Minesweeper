using System;

namespace Minesweeper
{
    internal class BombTile : Tile
    {
        public BombTile(int tileLeft, int tileTop) : base(tileLeft, tileTop)
        {
            Left = tileLeft;
            Top = tileTop;
            bombScore = -1;
            Click += new EventHandler(BombClick);
        }

        private void BombClick(object sender, EventArgs e)
        {
            if (hasFlag)
                return;
            Text = "*";
            GameManager.GameOver();
        }
    }
}
