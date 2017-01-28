using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Minesweeper
{
    public partial class Menu : Form
    {
		LinkedList<Button> buttons;

		public Menu()
        {
            InitializeComponent();
        }

        private void Menu_Load(object sender, EventArgs e)
        {
			buttons = new LinkedList<Button>();

			Button temp = new Button();
			temp.Name = "start";
			temp.Text = "Start game";
			temp.Size = new Size(110, 20);
			temp.Location = new Point(45, 50);
			temp.UseVisualStyleBackColor = true;
			temp.Click += new EventHandler(this.start_clicked);
			buttons.AddLast(temp);

			temp = new Button();
			temp.Name = "highscores";
			temp.Text = "Highscores";
			temp.Size = new Size(110, 20);
			temp.Location = new Point(45, 80);
			temp.UseVisualStyleBackColor = true;
			buttons.AddLast(temp);

			temp = new Button();
			temp.Name = "exit";
			temp.Text = "Exit";
			temp.Size = new Size(110, 20);
			temp.Location = new Point(45, 110);
			temp.UseVisualStyleBackColor = true;
			temp.Click += new EventHandler(this.exit_clicked);
			buttons.AddLast(temp);

			this.SuspendLayout();
			for (int i = 0; i < buttons.Count; i++)
			{
				Controls.Add(buttons.ElementAt(i));
			}
			this.ResumeLayout();

			Control.MenuForm = this;
			Control.CurrentForm = this;
        }

		public void start_clicked(object sender, EventArgs e)
		{
			GameScreen screen = new GameScreen();
			screen.Show();
			this.Hide();
		}

		public void exit_clicked(object sender, EventArgs e)
		{
			Application.Exit();
		}
	}
}
