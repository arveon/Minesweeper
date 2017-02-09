using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

using System.Windows.Forms;

namespace Minesweeper
{
	//class represents a game difficulty slider (2 buttons & label) 
	//created as a separate class to allow reusability in other projects (self-sustainable)
	class DifficultySlider
	{
		Form owner;//the form slider is being added to
		string[] values = {"beginner", "medium", "hard"};//possible difficulties
		int current = 0;//current difficulty

		Point coords;//location of the slider on form

		Button increase, decrease;//buttons to manage difficulty
		Label display;//label to display currently selected difficulty

		public event EventHandler DifficultyChanged;//event that is being raised whenever difficulty is changed

		public DifficultySlider(Point coords, Size size, Form owner)
		{
			this.coords = coords;
			this.owner = owner;

			#region setting up UI elements
			//decrease difficulty button
			decrease = new Button();
			decrease.Location = new Point(coords.X, coords.Y);
			decrease.Width = (size.Width / 100) * 20;
			decrease.Height = size.Height;
			decrease.Text = "<";
			decrease.Visible = true;
			decrease.Name = "decrease";
			decrease.Click += new EventHandler(valueChanged);

			//increase difficulty button
			increase = new Button();
			increase.Location = new Point(coords.X + (size.Width / 100) * 80 + 20, coords.Y); 
			increase.Width = (size.Width / 100) * 20;
			increase.Height = size.Height;
			increase.Text = ">";
			increase.Visible = true;
			increase.Name = "increase";
			increase.Click += new EventHandler(valueChanged);

			//display label
			display = new Label();
			display.Text = values[current];
			display.Location = new Point(coords.X + increase.Width + (size.Width / 100 * 5), coords.Y);
			display.Width = (size.Width / 100 * 50 + 20);
			display.Height = size.Height;
			display.Visible = true;
			display.Font = new Font("Georgian", 10);

			//add UI elements to the parent form
			owner.SuspendLayout();
			owner.Controls.Add(decrease);
			owner.Controls.Add(increase);
			owner.Controls.Add(display);
			owner.ResumeLayout();
			#endregion
		}

		//method returns the currently selected difficulty
		public int getDifficulty()
		{
			return current;
		}

		//method is raised whenever the difficulty is changed
		protected void valueChanged(object sender, EventArgs args)
		{
			//button that was clicked
			Button temp = (Button)sender;

			//if increase button pressed - increase difficulty, if decrease button pressed - reduce difficulty
			if (temp.Name == "increase")
				current++;
			else if (temp.Name == "decrease")
				current--;

			//cap the variable
			current = (current < 0) ? 0 : current;
			current = (current > 2) ? 2 : current;

			//update the display
			display.Text = values[current];

			//raise the event
			DifficultyChanged(this, null);
		}
	}
}
