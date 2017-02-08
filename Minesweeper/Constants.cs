using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Minesweeper
{
    public class Constants
    {
		#region form dimension specific
		public const int MENU_FORM_HEIGHT = 250;
		public const int MENU_FORM_WIDTH = 200;

		public const int MENU_BUTTON_WIDTH = 110;
		public const int MENU_BUTTON_HEIGHT = 20;

		public const int DIFFICULTY_BAR_X = 20;
		public const int DIFFICULTY_BAR_Y = 200;

		public const int DIFFICULTY_BAR_WIDTH = 100;
		public const int DIFFICULTY_BAR_HEIGHT = 20;

		public const int GAME_BUTTON_HEIGHT = 16;
		public const int GAME_BUTTON_WIDTH = 16;

		public const int EASY_SCREEN_WIDTH = 150;
		public const int EASY_SCREEN_HEIGHT = 185;

		public const int MEDIUM_SCREEN_WIDTH = 261;
		public const int MEDIUM_SCREEN_HEIGHT = 297;

		public const int HARD_SCREEN_WIDTH = 389;
		public const int HARD_SCREEN_HEIGHT = 425;

		public const int GRID_OFFSET_X = 3;
		public const int GRID_OFFSET_Y = 40;

		public static Point EASY_REMAINING_FLAGS_LABEL_LOCATION = new Point (120,13);
		public static Point EASY_RESET_BUTTON_LOCATION = new Point(60,5);
		public static Point EASY_TIMER_LOCATION = new Point(10,13);

		public static Point MEDIUM_REMAINING_FLAGS_LABEL_LOCATION = new Point(210, 13);
		public static Point MEDIUM_RESET_BUTTON_LOCATION = new Point(114, 5);
		public static Point MEDIUM_TIMER_LOCATION = new Point(20, 13);

		public static Point HARD_REMAINING_FLAGS_LABEL_LOCATION = new Point(350, 13);
		public static Point HARD_RESET_BUTTON_LOCATION = new Point(180, 5);
		public static Point HARD_TIMER_LOCATION = new Point(25, 13);

		public static Size RESET_BUTTON_SIZE = new Size(30,30);
		#endregion

		#region system	
		public enum GameState { MainMenu = 0, Game = 1, Pause = 2, Highscores = 3, GameOver = 4 };
		public static int HIGHSCORES_NUMBER = 5;
		#endregion

		#region game specific
		public enum Difficulty { Easy = 0, Medium = 1, Hard = 2 };

        public const int EASY_WIDTH = 9;
        public const int EASY_HEIGHT = 9;
		public const int EASY_NUM_FLAGS = 10;


		public const int MEDIUM_WIDTH = 16;
        public const int MEDIUM_HEIGHT = 16;
		public const int MEDIUM_NUM_FLAGS = 40;

		public const int HARD_WIDTH = 24;
        public const int HARD_HEIGHT = 24;
		public const int HARD_NUM_FLAGS = 99;

		public enum TileState { Hidden = 0, Revealed = 1, Marked = 2 }
		#endregion
	}

}
