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
		LinkedList<LinkedList<Tile>> gameFieldTiles;
		LinkedList<Point> mineCoords;

		Image[] tilesheet;

		public GameScreen()
		{
			InitializeComponent();
		}

		private void GameScreen_Load(object sender, EventArgs e)
		{}

		public void StartGame()
		{
			Console.WriteLine("gameReset");
			LoadAssets();
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

			#region setting up a field backend
			//set up the game field
			char[,] gameField = new char[w,h];
			mineCoords = new LinkedList<Point>();
			Random rnd = new Random();
			//add mines to game field
			for (int i = 0; i < mines; i++)
			{
				int x, y;
				x = rnd.Next(0, w);
				y = rnd.Next(0, h);
				gameField[y, x] = 'm';
				mineCoords.AddLast(new Point(x, y));
				
			}
			//set up the rest of the field
			for(int i = 0; i < h; i++)
			{
				for(int j = 0; j < w; j++)
				{
					//if it's not a mine, count number of mines around it
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
					}
				}
			}
			#endregion

			#region field frontend
			//add buttons(labels) to the game field
			gameFieldTiles = new LinkedList<LinkedList<Tile>>();
			for (int i = 0; i < h; i++)
			{
				gameFieldTiles.AddLast(new LinkedList<Tile>());
				for (int j = 0; j < w; j++)
				{
					int x = 20 + (Constants.GAME_BUTTON_WIDTH * j - 1);
					int y = 20 + (Constants.GAME_BUTTON_HEIGHT * i - 1);

					bool tileEmpty = false;
					bool mine = false;
					Image img = tilesheet[1];
					switch(Convert.ToInt32(gameField[i,j]))
					{
						case 0: 
							img = tilesheet[0];
							tileEmpty = true;
							break;
						case 1: 
							img = tilesheet[1];
							break;
						case 2:
							img = tilesheet[2];
							break;
						case 3:
							img = tilesheet[3];
							break;
						case 4:
							img = tilesheet[4];
							break;
						case 5:
							img = tilesheet[5];
							break;
						case 6:
							img = tilesheet[6];
							break;
						case 7:
							img = tilesheet[7];
							break;
						case 8:
							img = tilesheet[8];
							break;
						case 'm': 
							img = tilesheet[10];
							mine = true;
							break;
						default:
							img = tilesheet[0];
							tileEmpty = true;
							Console.WriteLine("SOME ERROR WHILE CREATING FIELD");
							break;
					}

					Tile temp = new Tile(new Point(x, y), new Size(Constants.GAME_BUTTON_WIDTH, Constants.GAME_BUTTON_HEIGHT), gameField[i, j], img, tilesheet[11], tilesheet[9], tilesheet[12], tileEmpty, new Point(j, i), mine);
					gameFieldTiles.ElementAt(i).AddLast(temp);
					temp.EmptyOpenedEvent += new EventHandler(OpenEmptyTiles);
					temp.MineOpenedEvent += new EventHandler(MineOpened);
					temp.TileClickedEvent += new EventHandler(CheckWinCondition);
				}
			}
			#endregion

			//add game field to screen
			this.SuspendLayout();
			foreach(LinkedList<Tile> l in gameFieldTiles)
				foreach (Tile b in l)
				{
					Controls.Add(b.Button);
					Controls.Add(b.Image_Container);
				}
			this.ResumeLayout();
			this.Show();
		}

		private void LoadAssets()
		{
			tilesheet = new Image[13];
			tilesheet[0] = Image.FromFile("assets/zero.png");
			tilesheet[1] = Image.FromFile("assets/one.png");
			tilesheet[2] = Image.FromFile("assets/two.png");
			tilesheet[3] = Image.FromFile("assets/three.png");
			tilesheet[4] = Image.FromFile("assets/four.png");
			tilesheet[5] = Image.FromFile("assets/five.png");
			tilesheet[6] = Image.FromFile("assets/six.png");
			tilesheet[7] = Image.FromFile("assets/seven.png");
			tilesheet[8] = Image.FromFile("assets/eight.png");
			tilesheet[9] = Image.FromFile("assets/flag.png");
			tilesheet[10] = Image.FromFile("assets/mine.png");
			tilesheet[11] = Image.FromFile("assets/empty.png");
			tilesheet[12] = Image.FromFile("assets/mine_blown.png");
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
                this.Dispose();//hide current window
			}
        }

		protected override void OnShown(EventArgs e)
		{
			Control.curState = Constants.GameState.Game;
			base.OnShown(e);
		}

		protected void OpenEmptyTiles(object sender, EventArgs args)
		{
			OpenEmptyRecursive((Tile)sender);
			foreach(LinkedList<Tile> l in gameFieldTiles)
			{
				foreach(Tile t in l)
				{
					t.BeingProcessed = false;
				}
			}
		}

		private void OpenEmptyRecursive(Tile tile)
		{
			if (!tile.isEmpty())
			{
				tile.Open();
				return;
			}
			else if (tile.BeingProcessed)
				return;

			tile.BeingProcessed = true;

			if(tile.GridCoords.X - 1 > -1)
				OpenEmptyRecursive(gameFieldTiles.ElementAt(tile.GridCoords.Y).ElementAt(tile.GridCoords.X-1));
			if(tile.GridCoords.X - 1 > -1 && tile.GridCoords.Y - 1 > -1)
				OpenEmptyRecursive(gameFieldTiles.ElementAt(tile.GridCoords.Y-1).ElementAt(tile.GridCoords.X - 1));
			if (tile.GridCoords.Y - 1 > -1)
				OpenEmptyRecursive(gameFieldTiles.ElementAt(tile.GridCoords.Y - 1).ElementAt(tile.GridCoords.X));
			if (tile.GridCoords.Y - 1 > -1 && tile.GridCoords.X + 1 < gameFieldTiles.ElementAt(1).Count)
				OpenEmptyRecursive(gameFieldTiles.ElementAt(tile.GridCoords.Y - 1).ElementAt(tile.GridCoords.X + 1));
			if (tile.GridCoords.X + 1 < gameFieldTiles.ElementAt(1).Count)
				OpenEmptyRecursive(gameFieldTiles.ElementAt(tile.GridCoords.Y).ElementAt(tile.GridCoords.X + 1));
			if (tile.GridCoords.X + 1 < gameFieldTiles.ElementAt(1).Count && tile.GridCoords.Y + 1 < gameFieldTiles.Count)
				OpenEmptyRecursive(gameFieldTiles.ElementAt(tile.GridCoords.Y + 1).ElementAt(tile.GridCoords.X + 1));
			if (tile.GridCoords.X < gameFieldTiles.ElementAt(1).Count && tile.GridCoords.Y + 1 < gameFieldTiles.ElementAt(1).Count)
				OpenEmptyRecursive(gameFieldTiles.ElementAt(tile.GridCoords.Y + 1).ElementAt(tile.GridCoords.X));
			if (tile.GridCoords.X - 1 > -1 && tile.GridCoords.Y + 1 < gameFieldTiles.ElementAt(1).Count)
				OpenEmptyRecursive(gameFieldTiles.ElementAt(tile.GridCoords.Y + 1).ElementAt(tile.GridCoords.X - 1));

			tile.Open();
		}

		protected void MineOpened(object sender, EventArgs args)
		{
			foreach(Point p in mineCoords)
			{
				gameFieldTiles.ElementAt(p.Y).ElementAt(p.X).Open();
			}
			Control.curState = Constants.GameState.GameOver;
			DialogResult gameOverWindow = MessageBox.Show("You lost. Try again?", "Game over", MessageBoxButtons.YesNo);
			if(gameOverWindow == DialogResult.Yes)
			{
				//Control.curState = Constants.GameState.Game;
				Control.ResetGame();
			}
			else
			{
				this.Dispose();
				Control.MenuForm.Show();
			}
		}

		protected void CheckWinCondition(object sender, EventArgs args)
		{
			
			foreach(LinkedList<Tile> l in gameFieldTiles)
				foreach(Tile t in l)
				{
					if (!t.isMine && !t.isOpen())
						return;
				}

			DialogResult winScr = MessageBox.Show("Congratulations, you won!");
			Control.curState = Constants.GameState.MainMenu;
			this.Dispose();
			Control.MenuForm.Show();
		}
	}

}
