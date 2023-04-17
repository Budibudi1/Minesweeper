using System;
using System.Drawing;
using System.Windows.Forms;

namespace Minesweeper
{
    public partial class Form1 : Form
    {
        private const int minSize = 5;
        private const int maxSize = 25;

        public Form1()
        {
            GameManager.form1 = this;
            InitializeComponent();
            PreInitializationInterface();
        }

        public void PreInitializationInterface()
        {
            this.ClientSize = new Size(275, 350);
            this.Text = "Minesweeper";

            GenerateBoxes(25, GenerateVariants());
            GenerateBoxes(150, "Easy.Medium.Hard");
            GenerateLabels(25, "Choose field size:");
            GenerateLabels(150, "Choose game mode:");
            GenerateButton();
        }

        public void RemoveInterface()
        {
            for (int i = this.Controls.Count - 1; i >= 0; i--)
            {
                this.Controls[i].Dispose();
            }
        }

        private void GenerateBoxes(int left, string variants)
        {
            ComboBox comboBox = new ComboBox()
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Size = new Size(100, 30),
                Left = left,
                Top = 40,
                DataSource = variants.Split('.')
            };

            this.Controls.Add(comboBox);
        }

        private void GenerateLabels(int left, string text)
        {
            Label label = new Label()
            {
                Size = new Size(100, 30),
                Top = 5,
                Left = left,
                Text = text,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Microsoft Ya Hei", 9, FontStyle.Regular)
            };
            this.Controls.Add(label);
        }

        private string GenerateVariants()
        {
            string[] variants = new string[maxSize + 1 - minSize];

            for (int i = minSize; i < maxSize + 1; i++)
            {
                variants[i - minSize] = $"{i} × {i}";
            }

            return String.Join(".", variants);
        }

        private void GenerateButton()
        {
            Button button = new Button()
            {
                Size = new Size(75, 30),
                Top = 100,
                Left = 100,
                Text = "Start",
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Microsoft Ya Hei", 9, FontStyle.Regular),
            };
            button.Click += StartClick;
            this.Controls.Add(button);
        }

        private void StartClick(object sender, EventArgs e)
        {
            int mapSize = int.Parse(this.Controls[0].Text.Substring(0, 2));
            int bombsQuantity = 0;

            switch (this.Controls[1].Text)
            {
                case "Easy":
                    bombsQuantity = mapSize * mapSize / 4;
                    break;
                case "Medium":
                    bombsQuantity = mapSize * mapSize / 3;
                    break;
                case "Hard":
                    bombsQuantity = mapSize * mapSize / 2;
                    break;
            }

            RemoveInterface();
            GameManager.GameInitialization(mapSize, bombsQuantity);
        }
    }
}
