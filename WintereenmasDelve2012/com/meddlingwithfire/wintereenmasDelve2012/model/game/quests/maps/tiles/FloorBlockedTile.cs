using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.model.game.quests.maps.tiles
{
	public class FloorBlockedTile : MapTile
	{
		public FloorBlockedTile()
			: base("Images/MapTiles/FloorTileBlocked.png", true, true, true, true, true, true, true, true, true, true)
		{ }

		override public Object Clone()
		{ return new FloorBlockedTile(); }
	}
}
