using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.model.game.quests.maps.tiles
{
	public class WallHorizontalTopTile : MapTile
	{
		public WallHorizontalTopTile()
			: base("Images/MapTiles/WallHorizontalTop.png", false, false, true, false, false, false, true, false, false, false)
		{ }

		override public Object Clone()
		{ return new WallHorizontalTopTile(); }
	}
}
