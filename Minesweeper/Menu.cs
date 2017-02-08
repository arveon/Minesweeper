using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;

namespace Minesweeper
{
    public partial class Menu : Form
    {
		LinkedList<Button> buttons;
		DifficultySlider difficulty;

		public Menu()
        {
			Control.records = new Highscores("highscores.txt");
			Control.records.loadHighscores();

			Control.button_click = new SoundPlayer("sounds/button_click.wav");
			Control.rightclick = new SoundPlayer("sounds/rightclick.wav");
			Control.explosion = new SoundPlayer("sounds/explosion.wav");
            InitializeComponent();
        }

        private void Menu_Load(object sender, EventArgs e)
        {
			buttons = new LinkedList<Button>();
			difficulty = new DifficultySlider(new Point(Constants.DIFFICULTY_BAR_X, Constants.DIFFICULTY_BAR_Y), new Size(Constants.DIFFICULTY_BAR_WIDTH, Constants.DIFFICULTY_BAR_HEIGHT), this);


			#region Setting up menu buttons
			Button temp = new Button();
			temp.Name = "start";
			temp.Text = "Start game";
			temp.Size = new Size(Constants.MENU_BUTTON_WIDTH, Constants.MENU_BUTTON_HEIGHT);
			temp.Location = new Point(45, 50);
			temp.UseVisualStyleBackColor = true;
			temp.Click += new EventHandler(this.start_clicked);
			buttons.AddLast(temp);

			temp = new Button();
			temp.Name = "highscores";
			temp.Text = "Highscores";
			temp.Size = new Size(Constants.MENU_BUTTON_WIDTH, Constants.MENU_BUTTON_HEIGHT);
			temp.Location = new Point(45, 80);
			temp.UseVisualStyleBackColor = true;
			temp.Click += new EventHandler(DisplayHighscores);
			buttons.AddLast(temp);

			temp = new Button();
			temp.Name = "exit";
			temp.Text = "Exit";
			temp.Size = new Size(Constants.MENU_BUTTON_WIDTH, Constants.MENU_BUTTON_HEIGHT);
			temp.Location = new Point(45, 140);
			temp.UseVisualStyleBackColor = true;
			temp.Click += new EventHandler(this.exit_clicked);
			buttons.AddLast(temp);


			temp = new Button();
			temp.Name = "about";
			temp.Text = "About";
			temp.Size = new Size(Constants.MENU_BUTTON_WIDTH, Constants.MENU_BUTTON_HEIGHT);
			temp.Location = new Point(45, 110);
			temp.UseVisualStyleBackColor = true;
			temp.Click += new EventHandler(this.about_clicked);
			buttons.AddLast(temp);
			#endregion

			//need to suspend layout when adding elements to the form and resume it after
			this.SuspendLayout();
			for (int i = 0; i < buttons.Count; i++)
			{
				Controls.Add(buttons.ElementAt(i));
			}
			this.ResumeLayout();

			//set up control fields
			Control.MenuForm = this;
            Control.GameForm = new GameScreen();
            Control.GameForm.Hide();

			difficulty.DifficultyChanged += new EventHandler(updateDifficulty);
        }

		public void start_clicked(object sender, EventArgs e)
		{
			Control.button_click.Play();
			Control.GameForm = new GameScreen();
			Control.GameForm.StartGame();
			this.Hide();
		}

		public void exit_clicked(object sender, EventArgs e)
		{
			Control.button_click.Play();
			Application.Exit();
		}

		protected override void OnShown(EventArgs e)
		{
			Control.curState = Constants.GameState.MainMenu;
			base.OnShown(e);
		}

		protected void updateDifficulty(object sender, EventArgs args)
		{
			Control.button_click.Play();
			int newdif = difficulty.getDifficulty();
			switch(newdif)
			{
				case 0: 
					Control.dif = Constants.Difficulty.Easy;
					break;
				case 1:
					Control.dif = Constants.Difficulty.Medium;
					break;
				case 2:
					Control.dif = Constants.Difficulty.Hard;
					break;
			}
		}

		protected void DisplayHighscores(object sender, EventArgs args)
		{
			Control.button_click.Play();
			MessageBox.Show(Control.records.getHighscoresLine(), "Highscores");
			//Control.records.printHighscores();
		}

		protected void about_clicked(object sender, EventArgs args)
		{
			Control.button_click.Play();
			MessageBox.Show("A replica of Windows Minesweeper game.\nCreated by Aleksejs Loginovs as a university assignment.", "About");
		}
	}
}
