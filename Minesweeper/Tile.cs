using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Minesweeper
{
	//class represents a single game tile
	class Tile
	{
		//images the given tile can switch between depending on user actions
		Image flag;
		Image hidden;
		Image mine_blown;

		//top and bottom image containers
		public PictureBox Button { get; set; }
		public PictureBox Image_Container { get; set; }
		//coordinates of the tile
		public Point GridCoords { get; set; }
		
		//bools determining	 the type of the tile
		bool empty;
		public bool isMine;

		public bool BeingProcessed { get; set; }//required for the recursive algorithm that opens all of the empty tiles (to avoid checking the same tile multiple times)

		//events that can be raised by a tile
		public event EventHandler MineOpenedEvent;
		public event EventHandler EmptyOpenedEvent;
		public event EventHandler TileClickedEvent;
		public event EventHandler TileMarkedEvent;

		//state of the tile
		Constants.TileState state;

		//Creates a tile with the provided properties
		//coords - coordinates of the tile on form
		//dimensions - size of the tile
		//value - type/value of the tile (1,2,3...8, m)
		//bottom_pic - picture shown when tile is revealed
		//flag_pic - picture shown when tile is marked
		//mine_blown - picture shown when the tile is a mine and it's blown
		//isEmpty - whether the tile is 0 or not
		//gridCoords - coordinates on the game grid
		//mine - boolean saying whether the tile is a mine
		public Tile(Point coords, Size dimensions, char value, Image bottom_pic, Image tile_pic, Image flag_pic, Image mine_blown, bool isEmpty, Point gridCoords, bool mine)
		{
			//set up all of the fields
			isMine = mine;
			this.empty = isEmpty;
			this.GridCoords = gridCoords;

			flag = flag_pic;
			hidden = tile_pic;
			this.mine_blown = mine_blown;

			//set up the PictureBoxes
			Button = new PictureBox();
			Button.Image = tile_pic;
			Button.Location = coords;
			Button.Size = dimensions;
			Button.Name = Convert.ToString(value);
			Button.Visible = true;
			Button.MouseClick += new MouseEventHandler(tileClicked);

			Image_Container = new PictureBox();
			Image_Container.Image = bottom_pic;
			Image_Container.SetBounds(coords.X, coords.Y, dimensions.Width, dimensions.Height);
			Image_Container.Name = Convert.ToString(value);

			//set initial tile state to hidden
			state = Constants.TileState.Hidden;
		}

		//method called when the tile is clicked
		protected void tileClicked(object src, MouseEventArgs args)
		{
			//if game is over, user can't click tiles
			if (Control.curState != Constants.GameState.GameOver)
			{
				//check which mouse button was clicked
				switch (args.Button)
				{
					case MouseButtons.Right://if right button was clicked, play sound and rise appropriate event
						Control.rightclick.Play();
						TileMarkedEvent(this, null);
						break;
					case MouseButtons.Left://if left mouse button was clicked
						if (state != Constants.TileState.Marked)
						{//only do stuff if the tile isn't marked
							Control.button_click.Play();
							//reveal the value
							state = Constants.TileState.Revealed;
							Button.Visible = false;

							//raise tile clicked event
							TileClickedEvent(this, null);
							if (empty)
							{//if tile was empty, raise emptyOpenedEvent
								EmptyOpenedEvent(this, null);
							}
							else if (isMine)
							{//if tile is mine, play sound and raise MineOpenedEvent
								Control.explosion.Play();
								Image_Container.Image = mine_blown;
								MineOpenedEvent(this, null);
							}
						}
							break;
				}
			}
		}

		//return true if the tile is revealed, false if anything else
		public bool isOpen()
		{
			return (state == Constants.TileState.Revealed);
		}

		//return true if the tile is marked, false if anything else
		public bool isMarked()
		{
			return (state == Constants.TileState.Marked);
		}

		//return true if tile value is 0, false if anything else
		public bool isEmpty()
		{
			return empty;
		}

		//method that reveals the tile manually
		public void Open()
		{
			state = Constants.TileState.Revealed;//change state
			Button.Visible = false;//reveal
			
			if(!isMine)
				TileClickedEvent(this, null);//raise event
		}

		//method marks the tile
		public void Mark()
		{
			//if current state isn't hidden (is marked) - remove mark
			if (state != Constants.TileState.Hidden)
			{
				state = Constants.TileState.Hidden;
				Button.Image = hidden;
			}
			else
			{//otherwise - mark the tile
				state = Constants.TileState.Marked;
				Button.Image = flag;
			}
		}

	}
}
