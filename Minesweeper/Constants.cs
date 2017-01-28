using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minesweeper
{
    public class Constants
    {
		public enum GameState { MainMenu = 0, Game = 1, Pause = 2, Highscores = 3 };
        public enum Difficulty { Easy = 0, Medium = 1, Hard = 2 };

        public int EASY_WIDTH = 16;
        public int EASY_HEIGHT = 16;

        public int MEDIUM_WIDTH = 16;
        public int MEDIUM_HEIGHT = 16;

        public int HARD_WIDTH = 16;
        public int HARD_HEIGHT = 16;

    }

}
