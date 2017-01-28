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
		public GameScreen(Constants.Difficulty dif)
		{
			InitializeComponent();
		}

		private void GameScreen_Load(object sender, EventArgs e){}

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

		//public new void Show()
		//{
		//	Control.curState = Constants.GameState.Game;
		//	Console.WriteLine("GameState: " + Control.curState);
		//	base.Show();
		//}

	}
}
