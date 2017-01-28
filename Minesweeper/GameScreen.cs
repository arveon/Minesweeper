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
	public partial class GameScreen : Form
	{
		public Constants.Difficulty Diff { get; set; }
		LinkedList<LinkedList<Label>> gameFieldCover;//replace with PictureBox 
		char[,] gameField;

		public GameScreen()
		{
			InitializeComponent();
		}

		private void GameScreen_Load(object sender, EventArgs e)
		{}

		public void StartGame()
		{
			int w = 0, h = 0, mines = 0;
			switch (Diff)
			{
				case Constants.Difficulty.Easy:
					w = Constants.EASY_WIDTH;
					h = Constants.EASY_HEIGHT;
					mines = Constants.EASY_NUM_FLAGS;
					break;
				case Constants.Difficulty.Medium:
					w = Constants.MEDIUM_WIDTH;
					h = Constants.MEDIUM_HEIGHT;
					mines = Constants.MEDIUM_NUM_FLAGS;
					break;
				case Constants.Difficulty.Hard:
					w = Constants.HARD_WIDTH;
					h = Constants.HARD_HEIGHT;
					mines = Constants.HARD_NUM_FLAGS;
					break;
			}

			//set up the game field
			gameField = new char[w,h];
			Random rnd = new Random();
			//add mines to game field
			for (int i = 0; i < mines; i++)
			{
				
				int x, y;
				x = rnd.Next(0, w);
				y = rnd.Next(0, h);
				gameField[y, x] = 'm';
			}
			//set up the rest of the field
			for(int i = 0; i < h; i++)
			{
				for(int j = 0; j < w; j++)
				{
					if (gameField[i, j] != 'm')
					{
						int numMines = 0;

						if (i != 0)
							if(gameField[i - 1, j] == 'm')
								numMines++;

						if (i != h-1)
							if(gameField[i + 1, j] == 'm')
								numMines++;

						if (j != 0)
							if(gameField[i, j - 1] == 'm')
								numMines++;

						if (j != w-1) 
							if(gameField[i, j + 1] == 'm')
								numMines++;

						if (i != 0 && j != 0)
							if(gameField[i - 1, j - 1] == 'm')
								numMines++;

						if (i != 0 && j != w-1)
							if(gameField[i - 1, j + 1] == 'm')
								numMines++;

						if (i != h-1 && j != 0)
							if(gameField[i + 1, j - 1] == 'm')
								numMines++;

						if (i != h-1 && j != h-1)
							if(gameField[i + 1, j + 1] == 'm')
								numMines++;

						gameField[i, j] = Convert.ToChar(numMines);
						Console.Write(numMines);
					}
					else
						Console.Write('m');
				}
				Console.WriteLine();
			}

			//add buttons(labels) to the game field
			gameFieldCover = new LinkedList<LinkedList<Label>>();
			for (int i = 0; i < h; i++)
			{
				gameFieldCover.AddLast(new LinkedList<Label>());
				for (int j = 0; j < w; j++)
				{
					Label temp = new Label();
					temp.Font = new Font(FontFamily.GenericMonospace, 6, FontStyle.Regular);
					temp.BackColor = Color.Aqua;
					temp.AutoSize = false;
					temp.Padding = new Padding(0);
					temp.Margin = new Padding(0);
					temp.Name = j + ";" + i;
					temp.Text = j + ";" + i;
					temp.Size = new Size(Constants.GAME_BUTTON_WIDTH, Constants.GAME_BUTTON_HEIGHT);

					int x = 20 + (Constants.GAME_BUTTON_WIDTH * j - 1);
					int y = 20 + (Constants.GAME_BUTTON_HEIGHT * i - 1);
					temp.Location = new Point(x, y);

					temp.Click += new EventHandler(tile_clicked);
					

					gameFieldCover.ElementAt(i).AddLast(temp);
				}
			}

			//add game field to screen
			this.SuspendLayout();
			foreach(LinkedList<Label> l in gameFieldCover)
				foreach(Label b in l)
					Controls.Add(b);
			this.ResumeLayout();
			this.Show();
		}

		private void tile_clicked(object src, EventArgs e)
		{
			Label b = (Label)src;
			b.Hide();
		}

		//method responsible for handling window closing
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            if (e.CloseReason == CloseReason.UserClosing)
            {
				e.Cancel = true;//cancel the event
				Control.curState = Constants.GameState.MainMenu;
                Control.MenuForm.Show();//show menu window
                this.Hide();//hide current window
			}
        }

		protected override void OnShown(EventArgs e)
		{
			Control.curState = Constants.GameState.Game;
			base.OnShown(e);
		}

	}
}
