using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Minesweeper
{
	static class Control
	{
		public static Constants.GameState curState;
		public static Menu MenuForm;
		public static GameScreen GameForm;

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
