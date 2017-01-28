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
		public GameScreen()
		{
			InitializeComponent();
		}

		private void GameScreen_Load(object sender, EventArgs e)
		{
			this.FormClosing += new FormClosingEventHandler(form_closing);
		}

		private void form_closing(object sender, EventArgs e)
		{
			Control.MenuForm.Show();
			this.Dispose();
		}
	}
}
