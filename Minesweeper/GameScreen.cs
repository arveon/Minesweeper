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
		LinkedList<LinkedList<Tile>> gameFieldTiles;//replace with PictureBox 
		//char[,] gameField;

		Image[] tilesheet;

		public GameScreen()
		{
			InitializeComponent();
		}

		private void GameScreen_Load(object sender, EventArgs e)
		{}

		public void StartGame()
		{
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
						//Console.Write(numMines);
					}
					//else
						//Console.Write('m');
				}
				//Console.WriteLine();
			}
			#endregion

			//for(int i = 0; )

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


					Image img = tilesheet[1];
					Console.Write(i + ":" + j + " " + gameField[i,j] + " ");
					switch(Convert.ToInt32(gameField[i,j]))
					{
						case 0: 
							img = tilesheet[0];
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
							break;
						//default:
						//	img = tilesheet[0];
						//	Console.WriteLine("DEFAULT");
						//	break;
					}

					gameFieldTiles.ElementAt(i).AddLast(new Tile(new Point(x, y), new Size(Constants.GAME_BUTTON_WIDTH, Constants.GAME_BUTTON_HEIGHT), gameField[i,j], img, tilesheet[11], tilesheet[9]));
					//Console.WriteLine("button_created");
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
			tilesheet = new Image[12];
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
