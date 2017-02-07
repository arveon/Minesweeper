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
		public PictureBox Button { get; set; }
		public PictureBox Image_Container { get; set; }

		Constants.TileState state;

		public Tile(Point coords, Size dimensions, char value, Image bottom_pic, Image tile_pic, Image flag_pic)
		{
			flag = flag_pic;
			hidden = tile_pic;

			Button = new PictureBox();
			//Button.SetBounds(coords.X, coords.Y, dimensions.X, dimensions.Y);
			Button.Image = tile_pic;
			Button.Location = coords;
			Button.Size = dimensions;
			Button.Name = Convert.ToString(value);
			Button.Visible = false;
			Button.MouseClick += new MouseEventHandler(tileClicked);

			state = Constants.TileState.Hidden;

			Image_Container = new PictureBox();
			Image_Container.Image = bottom_pic;
			Image_Container.SetBounds(coords.X, coords.Y, dimensions.Width, dimensions.Height);
			Image_Container.Name = Convert.ToString(value);
			 
		}

		protected void tileClicked(object src, MouseEventArgs args)
		{	
			switch(args.Button)
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
					break;
			}
		}

	}
}
