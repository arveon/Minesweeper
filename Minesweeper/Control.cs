using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;


namespace Minesweeper
{
	//control class that contains all of the objects that need to be available globally
	//(eg forms, difficulty, highscores)
	static class Control
	{
		public static Highscores records;
		public static Constants.GameState curState;//reflects the current game state
		public static Menu MenuForm;//contains the main menu form
		public static GameScreen GameForm;//game form
		public static Constants.Difficulty dif;

		//sound effects (need to be available globally)
		public static SoundPlayer button_click;
		public static SoundPlayer rightclick;
		public static SoundPlayer explosion;

		//method is responsible for reloading the game form thus resetting the game
		public static void ResetGame()
		{
			curState = Constants.GameState.Game;
			GameForm.Dispose();
			GameForm = new GameScreen();
			GameForm.StartGame();
			GameForm.Show();
		}
	}
}
