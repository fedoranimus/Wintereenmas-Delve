using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.model.game.quests.maps.tiles
{
	public class WallHorizontalBottomTile : MapTile
	{
		public WallHorizontalBottomTile()
			: base("Images/MapTiles/WallHorizontalBottom.png", false, false, false, false, true, false, false, false, true, false)
		{ }

		override public Object Clone()
		{ return new WallHorizontalBottomTile(); }
	}
}
