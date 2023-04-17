using System.Drawing;
using System.Windows.Forms;

namespace Minesweeper
{
    public class Tile : Button
    {
        public int bombScore = 0;

        public bool hasFlag = false;

        public Tile(int tileLeft, int tileTop)
        {
            Size = new Size(GameManager.tileSize, GameManager.tileSize);
            FlatStyle = FlatStyle.Flat;
            BackColor = Color.LightGray;
            Left = tileLeft;
            Top = tileTop;
            Font = new Font("Microsoft Yi Baiti", 12, FontStyle.Bold);

            MouseUp += new MouseEventHandler(PutFlag);
        }

        private void PutFlag(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                hasFlag = !hasFlag;

                if (hasFlag)
                {
                    (sender as Tile).Text = "F";
                    GameManager.flagCounter--;
                }
                else
                {
                    (sender as Tile).Text = "";
                    GameManager.flagCounter++;
                }
                GameManager.flagLabel.Text = $"Flags remaining: {GameManager.flagCounter}";
                GameManager.CheckMap();
            }
        }
    }
}
