using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

using System.Windows.Forms;

namespace Minesweeper
{
	class DifficultySlider
	{
		Form owner;
		string[] values = {"beginner", "medium", "hard"};
		int current = 0;

		Point coords;

		Button increase, decrease;
		Label display;

		public event EventHandler DifficultyChanged;

		public DifficultySlider(Point coords, Size size, Form owner)
		{
			this.coords = coords;
			this.owner = owner;

			decrease = new Button();
			decrease.Location = new Point(coords.X, coords.Y);
			decrease.Width = (size.Width / 100) * 20;
			decrease.Height = size.Height;
			decrease.Text = "<";
			decrease.Visible = true;
			decrease.Name = "decrease";
			decrease.Click += new EventHandler(valueChanged);

			increase = new Button();
			increase.Location = new Point(coords.X + (size.Width / 100) * 80, coords.Y); 
			increase.Width = (size.Width / 100) * 20;
			increase.Height = size.Height;
			increase.Text = ">";
			increase.Visible = true;
			increase.Name = "increase";
			increase.Click += new EventHandler(valueChanged);

			display = new Label();
			display.Text = values[current];
			display.Location = new Point(100, 100);
			display.Location = new Point(coords.X + increase.Width + (size.Width / 100 * 5), coords.Y);
			display.Width = (size.Width / 100 * 50);
			display.Height = size.Height;
			display.Visible = true;

			owner.SuspendLayout();
			owner.Controls.Add(decrease);
			owner.Controls.Add(increase);
			owner.Controls.Add(display);
			owner.ResumeLayout();
		}

		public int getDifficulty()
		{
			return current;
		}

		protected void valueChanged(object sender, EventArgs args)
		{
			Button temp = (Button)sender;

			//change the value
			if (temp.Name == "increase")
				current++;
			else if (temp.Name == "decrease")
				current--;

			//cap the variable
			current = (current < 0) ? 0 : current;
			current = (current > 2) ? 2 : current;

			//update the display
			display.Text = values[current];

			DifficultyChanged(this, null);
		}
	}
}
