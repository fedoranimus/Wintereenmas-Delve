using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.model.game.quests.maps.tileActions;

namespace WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.model.game.quests.maps
{
	public class MapTileList : List<MapTile>
	{

		public Boolean BlocksMovement
		{
			get
			{
				foreach(MapTile tile in this)
				{
					if (tile.BlocksMovement)
					{ return true; }
				}
				return false;
			}
		}

		public Boolean BlocksWest
		{
			get
			{
				foreach (MapTile tile in this)
				{
					if (tile.BlocksWest)
					{ return true; }
				}
				return false;
			}
		}

		public Boolean BlocksNorth
		{
			get
			{
				foreach (MapTile tile in this)
				{
					if (tile.BlocksNorth)
					{ return true; }
				}
				return false;
			}
		}

		public Boolean BlocksEast
		{
			get
			{
				foreach (MapTile tile in this)
				{
					if (tile.BlocksEast)
					{ return true; }
				}
				return false;
			}
		}

		public Boolean BlocksSouth
		{
			get
			{
				foreach (MapTile tile in this)
				{
					if (tile.BlocksSouth)
					{ return true; }
				}
				return false;
			}
		}

		public Boolean BlocksLineOfSight
		{
			get
			{
				foreach (MapTile tile in this)
				{
					if (tile.BlocksLineOfSight)
					{ return true; }
				}
				return false;
			}
		}

		public Boolean BlocksLineOfSightNorth
		{
			get
			{
				foreach (MapTile tile in this)
				{
					if (tile.BlocksLineOfSightNorth)
					{ return true; }
				}
				return false;
			}
		}

		public Boolean BlocksLineOfSightWest
		{
			get
			{
				foreach (MapTile tile in this)
				{
					if (tile.BlocksLineOfSightWest)
					{ return true; }
				}
				return false;
			}
		}

		public Boolean BlocksLineOfSightSouth
		{
			get
			{
				foreach (MapTile tile in this)
				{
					if (tile.BlocksLineOfSightSouth)
					{ return true; }
				}
				return false;
			}
		}

		public Boolean BlocksLineOfSightEast
		{
			get
			{
				foreach (MapTile tile in this)
				{
					if (tile.BlocksLineOfSightEast)
					{ return true; }
				}
				return false;
			}
		}

		public List<AbstractTileAction> Actions
		{
			get
			{
				List<AbstractTileAction> actions = new List<AbstractTileAction>();
				foreach (MapTile tile in this)
				{
					foreach (AbstractTileAction action in tile.Actions)
					{ actions.Add(action); }
				}
				return actions;
			}
		}
	}
}
