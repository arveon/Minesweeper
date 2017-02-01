using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minesweeper
{
    public class Constants
    {
		#region form dimension specific
		public const int MENU_FORM_HEIGHT = 250;
		public const int MENU_FORM_WIDTH = 200;

		public const int MENU_BUTTON_WIDTH = 110;
		public const int MENU_BUTTON_HEIGHT = 20;

		public const int GAME_BUTTON_HEIGHT = 16;
		public const int GAME_BUTTON_WIDTH = 16;
		#endregion

		#region system	
		public enum GameState { MainMenu = 0, Game = 1, Pause = 2, Highscores = 3 };
		#endregion

		#region game specific
		public enum Difficulty { Easy = 0, Medium = 1, Hard = 2 };

        public const int EASY_WIDTH = 16;
        public const int EASY_HEIGHT = 16;
		public const int EASY_NUM_FLAGS = 10;


		public const int MEDIUM_WIDTH = 16;
        public const int MEDIUM_HEIGHT = 16;
		public const int MEDIUM_NUM_FLAGS = 40;

		public const int HARD_WIDTH = 16;
        public const int HARD_HEIGHT = 16;
		public const int HARD_NUM_FLAGS = 100;

		public enum TileState { Hidden = 0, Revealed = 1, Marked = 2 }
		#endregion
	}

}
