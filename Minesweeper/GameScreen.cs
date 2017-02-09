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
	//class that represents the game form. also controls the game flow
	public partial class GameScreen : Form
	{
		LinkedList<LinkedList<Tile>> gameFieldTiles;//2d tile array
		LinkedList<Point> mineCoords;//list of coordinates of all mines on the field

		Timer timer;//timer to update game time

		int remainingFlags;//number of flags that user is still able to put on the field

		Image[] tilesheet;//different tile images
		Image[] button_img;//images that the reset button can take

		int secondsPassed;//counter for how many seconds have passed
		//top of the UI elements
		Label timer_display;
		Button resetButton;
		Label remainingFlags_l;
		
		//width, height of the field (in tiles) and number of mines on it
		int w = 0, h = 0, mines = 0;

		//default constructor
		public GameScreen()
		{
			InitializeComponent();
			LoadAssets();
			//depending on the difficulty, set the grid size and number of mines
			switch (Control.dif)
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
		}

		private void GameScreen_Load(object sender, EventArgs e)
		{}

		//method responsible for starting and setting up the game
		public void StartGame()
		{
			
			#region setting up backend
			//set up the top UI bar
			secondsPassed = 0;
			remainingFlags = mines;
			//init timer
			timer = new Timer();
			timer.Interval = 1000;
			timer.Tick += new EventHandler(TimerTick);
			TopUISetup();

			//set up the game field
			char[,] gameField = new char[h,w];
			mineCoords = new LinkedList<Point>();
			Random rnd = new Random();
			//add mines to game field
			for (int i = 0; i < mines; i++)
			{
				//generate random coordinates for the mine
				int x, y;
				x = rnd.Next(0, w);
				y = rnd.Next(0, h);
				//check if mine already exists there
				foreach (Point p in mineCoords)
					if (p.X == x && p.Y == y)//if mine already exists under these coordinates, reduce i and go to the next iteration of the loop
					{
						i--;
						continue;
					}
				
				//add mine to game field and to list of mines
				gameField[y, x] = 'm';
				mineCoords.AddLast(new Point(x, y));
			}

			//set up the rest of the field values depending on the mine locations
			for(int i = 0; i < h; i++)
			{
				for(int j = 0; j < w; j++)
				{
					//if it's not a mine, count number of mines around it
					if (gameField[i, j] != 'm')
					{
						int numMines = 0;//number of mines around this tile

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

						if (i != h-1 && j != w - 1)
							if(gameField[i + 1, j + 1] == 'm')
								numMines++;
						//add the value to the game field grid
						gameField[i, j] = Convert.ToChar(numMines);
					}
				}
			}
			#endregion

			#region field frontend
			//add tiles to the game field depending on the grid values
			gameFieldTiles = new LinkedList<LinkedList<Tile>>();//initialise the 2d list
			for (int i = 0; i < h; i++)
			{
				gameFieldTiles.AddLast(new LinkedList<Tile>());//initialise the 2nd dimension of the list
				for (int j = 0; j < w; j++)
				{
					//calculate the form coordinates of the buttons
					int x = Constants.GRID_OFFSET_X + (Constants.GAME_BUTTON_WIDTH * j - 1);
					int y = Constants.GRID_OFFSET_Y + (Constants.GAME_BUTTON_HEIGHT * i - 1);

					//set up tile descriptor variables
					bool tileEmpty = false;
					bool mine = false;
					Image img = tilesheet[1];
					//set values depending on the value of the tile at given place in game field
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
						default://should never occur unless there's errors in the code. visual studio complains about variables not being initialised if taken out
							img = tilesheet[0];
							tileEmpty = true;
							Console.WriteLine("SOME ERROR WHILE CREATING FIELD");
							break;
					}

					//create new tile with given values
					Tile temp = new Tile(new Point(x, y), new Size(Constants.GAME_BUTTON_WIDTH, Constants.GAME_BUTTON_HEIGHT), gameField[i, j], img, tilesheet[11], tilesheet[9], tilesheet[12], tileEmpty, new Point(j, i), mine);
					gameFieldTiles.ElementAt(i).AddLast(temp);//add tile to given field

					//subscribe form to all events raised by the tile
					temp.EmptyOpenedEvent += new EventHandler(OpenEmptyTiles);
					temp.MineOpenedEvent += new EventHandler(MineOpened);
					temp.TileClickedEvent += new EventHandler(CheckWinCondition);
					temp.TileMarkedEvent += new EventHandler(ApproveTileMark);
				}
			}
			#endregion

			//add game field and UI elements to form
			this.SuspendLayout();
			foreach(LinkedList<Tile> l in gameFieldTiles)
				foreach (Tile b in l)
				{
					Controls.Add(b.Button);
					Controls.Add(b.Image_Container);
				}
			Controls.Add(remainingFlags_l);
			Controls.Add(resetButton);
			Controls.Add(timer_display);
			this.ResumeLayout();
			this.Show();
		}

		//method responsible for loading all graphical assets used in the game
		private void LoadAssets()
		{
			//tile assets
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

			//reset button assets
			button_img = new Image[3];
			button_img[0] = Image.FromFile("assets/reset_smile.png");
			button_img[1] = Image.FromFile("assets/reset_dead.png");
			button_img[2] = Image.FromFile("assets/reset_chill.png");
		}

		//method responsible for setting up the top part of the UI
		private void TopUISetup()
		{
			//3 button locations
			Point rfLoc;//remaining flags
			Point rbLoc;//reset button
			Point tdLoc;//time display

			//depending on the difficulty, set the element locations
			switch(Control.dif)
			{
				case Constants.Difficulty.Easy:
					rfLoc = Constants.EASY_REMAINING_FLAGS_LABEL_LOCATION;
					rbLoc = Constants.EASY_RESET_BUTTON_LOCATION;
					tdLoc = Constants.EASY_TIMER_LOCATION;
					break;
				case Constants.Difficulty.Medium:
					rfLoc = Constants.MEDIUM_REMAINING_FLAGS_LABEL_LOCATION;
					rbLoc = Constants.MEDIUM_RESET_BUTTON_LOCATION;
					tdLoc = Constants.MEDIUM_TIMER_LOCATION;
					break;
				case Constants.Difficulty.Hard:
					rfLoc = Constants.HARD_REMAINING_FLAGS_LABEL_LOCATION;
					rbLoc = Constants.HARD_RESET_BUTTON_LOCATION;
					tdLoc = Constants.HARD_TIMER_LOCATION;
					break;
				default:
					rfLoc = Constants.EASY_REMAINING_FLAGS_LABEL_LOCATION;
					rbLoc = Constants.EASY_RESET_BUTTON_LOCATION;
					tdLoc = Constants.EASY_TIMER_LOCATION;
					break;
			}

			//initialise the elements
			remainingFlags_l = new Label();
			remainingFlags_l.Text = Convert.ToString(remainingFlags);
			remainingFlags_l.Location = rfLoc;
			remainingFlags_l.Font = new Font("Impact",10f,FontStyle.Regular);
			remainingFlags_l.ForeColor = Color.Green;

			resetButton = new Button();
			resetButton.Size = Constants.RESET_BUTTON_SIZE;
			resetButton.Location = rbLoc;
			resetButton.Image = button_img[0];
			resetButton.Click += new EventHandler(resetGame);

			timer_display = new Label();
			timer_display.Text = Convert.ToString(secondsPassed);
			timer_display.Location = tdLoc;
			timer_display.Font = new Font("Impact", 10f, FontStyle.Regular);
			timer_display.ForeColor = Color.Green;
		}

		//method required to open the menu on game closing and dispose of the game window
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

		//method responsible for setting the state 
		protected override void OnShown(EventArgs e)
		{
			Control.curState = Constants.GameState.Game;
			base.OnShown(e);
		}

		//event handler triggered when an empty tile is opened
		protected void OpenEmptyTiles(object sender, EventArgs args)
		{
			OpenEmptyRecursive((Tile)sender);
			//set all variables to not being processed
			foreach(LinkedList<Tile> l in gameFieldTiles)
				foreach(Tile t in l)
					t.BeingProcessed = false;
		}

		//recursively loop through all neighbour empty tiles and open them
		private void OpenEmptyRecursive(Tile tile)
		{
			if (!tile.isEmpty() && !tile.isMarked())
			{//if tile is not empty, open it and return
				tile.Open();
				return;
			}
			else if (tile.BeingProcessed || tile.isMarked())//if tile is marked and being processed, stop processing it
				return;

			//set tile to being processed
			tile.BeingProcessed = true;

			//process tiles in all 8 directions
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

			//open the tile if its not marked and not a mine
			if(!tile.isMarked() && !tile.isMine)
				tile.Open();
		}

		//event handler for losing game (mine opened)
		protected void MineOpened(object sender, EventArgs args)
		{
			timer.Enabled = false;//stop timer
			Control.curState = Constants.GameState.GameOver;//change game state
			resetButton.Image = button_img[1];//set reset button image
			//open all the mines
			foreach(Point p in mineCoords)
			{
				gameFieldTiles.ElementAt(p.Y).ElementAt(p.X).Open();
			}
		}

		//event handler for a tile being opened
		//checks for whether the game was won
		protected void CheckWinCondition(object sender, EventArgs args)
		{
			//if timer is disabled - enable it
			if (!timer.Enabled)
				timer.Enabled = true;

			//loop through all of the tiles, if any tile that is not a mine isn't open - game not won so return
			foreach(LinkedList<Tile> l in gameFieldTiles)
				foreach(Tile t in l)
				{
					if (!t.isMine && !t.isOpen())
						return;
				}

			//if method still going, game is won
			//change reset button, game state, stop timer
			resetButton.Image = button_img[2];
			Control.curState = Constants.GameState.GameOver;
			timer.Enabled = false;

			//calculate the score (mines per second * 100 to avoid floating point) 
			int score =(int)((float)mines / secondsPassed * 100);
			
			//if the score achieved is a highscore, ask for player's name and add it to the table
			if (Control.records.isHighscore(score))
			{
				string text = "You set a new highscore of " + score + ".\nPlease, enter your name:";
				string name = Microsoft.VisualBasic.Interaction.InputBox(text, "New highscore set!", "unknown");//visual basic input form, as c# doesn't have it's own input forms
				Control.records.addHighscore(score, name);
				Control.records.saveHighscores();//save highscores to a file
			}

		}

		//approve that the tile can be marked
		protected void ApproveTileMark(object sender, EventArgs args)
		{
			Tile temp = (Tile)sender;
			//if tile is not already marked
			if (!temp.isMarked())
			{
				if (remainingFlags > 0)
				{//if there are remaining flags, mark the tile and update remaining flags
					temp.Mark();
					remainingFlags--;
				}
			}
			else
			{//if tile is already mark, remove the flag and update remaining flags
				temp.Mark();
				remainingFlags++;
			}
			remainingFlags_l.Text = Convert.ToString(remainingFlags);//update the remaining flags display
		}

		//event handler for timer to update timer display
		protected void TimerTick(object sender, EventArgs args)
		{
			secondsPassed++;
			timer_display.Text = Convert.ToString(secondsPassed);
		}

		//event handler for reset button
		protected void resetGame(object sender, EventArgs args)
		{
			Control.ResetGame();
		}
	}

}


//sample code

	//creating a dialog window with yes/no options
//DialogResult gameOverWindow = MessageBox.Show("You lost. Try again?", "Game over", MessageBoxButtons.YesNo);

//if(gameOverWindow == DialogResult.Yes)
//{
//	Control.curState = Constants.GameState.Game;
//	Control.ResetGame();
//}
//else
//{
//	this.Dispose();
//	Control.MenuForm.Show();
//}
