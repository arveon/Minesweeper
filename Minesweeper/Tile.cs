using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Minesweeper
{
	class Tile
	{
		Image flag;
		Image hidden;
		Image mine_blown;

		public PictureBox Button { get; set; }
		public PictureBox Image_Container { get; set; }
		public Point GridCoords { get; set; }
		bool empty;
		public bool isMine;

		public bool BeingProcessed { get; set; }//required to avoid checking the tile twice while opening empty tiles

		public event EventHandler MineOpenedEvent;
		public event EventHandler EmptyOpenedEvent;
		public event EventHandler TileClickedEvent;

		Constants.TileState state;

		public Tile(Point coords, Size dimensions, char value, Image bottom_pic, Image tile_pic, Image flag_pic, Image mine_blown, bool isEmpty, Point gridCoords, bool mine)
		{
			isMine = mine;
			this.empty = isEmpty;
			this.GridCoords = gridCoords;

			flag = flag_pic;
			hidden = tile_pic;
			this.mine_blown = mine_blown;

			Button = new PictureBox();
			Button.Image = tile_pic;
			Button.Location = coords;
			Button.Size = dimensions;
			Button.Name = Convert.ToString(value);
			Button.Visible = true;
			Button.MouseClick += new MouseEventHandler(tileClicked);

			state = Constants.TileState.Hidden;

			Image_Container = new PictureBox();
			Image_Container.Image = bottom_pic;
			Image_Container.SetBounds(coords.X, coords.Y, dimensions.Width, dimensions.Height);
			Image_Container.Name = Convert.ToString(value);
			 
		}

		protected void tileClicked(object src, MouseEventArgs args)
		{
			if (Control.curState != Constants.GameState.GameOver)
			{
				switch (args.Button)
				{
					case MouseButtons.Right:
						if (state == Constants.TileState.Hidden)
						{
							state = Constants.TileState.Marked;
							Button.Image = flag;
						}
						else
						{
							state = Constants.TileState.Hidden;
							Button.Image = hidden;
						}
						break;
					case MouseButtons.Left:
						if (state != Constants.TileState.Marked)
						{
							state = Constants.TileState.Revealed;
							Button.Visible = false;
						}

						if (empty)
							EmptyOpenedEvent(this, null);
						else if (isMine)
						{
							Image_Container.Image = mine_blown;
							MineOpenedEvent(this, null);
						}

						TileClickedEvent(this, null);
						break;
				}
			}
		}

		public bool isOpen()
		{
			return (state == Constants.TileState.Revealed);
		}

		public bool isMarked()
		{
			return (state == Constants.TileState.Marked);
		}

		public bool isEmpty()
		{
			return empty;
		}

		public void Open()
		{
			state = Constants.TileState.Revealed;
			Button.Visible = false;
		}

	}
}
