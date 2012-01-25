using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.model.game.quests.maps.tiles
{
	public class FloorEmptyTile : MapTile
	{
		public FloorEmptyTile()
			: base("Images/MapTiles/FloorTileEmpty.png", false, false, false, false, false)
		{ }

		override public Object Clone()
		{ return new FloorEmptyTile(); }
	}
}
