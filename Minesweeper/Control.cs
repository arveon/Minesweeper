using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;


namespace Minesweeper
{
	static class Control
	{
		public static Highscores records;
		public static Constants.GameState curState;
		public static Menu MenuForm;
		public static GameScreen GameForm;
		public static Constants.Difficulty dif;

		public static SoundPlayer button_click;
		//public static SoundPlayer background;
		public static SoundPlayer rightclick;
		public static SoundPlayer explosion;

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
