using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.model.game.quests.maps.tiles
{
	public class WallVerticalLeftTile:MapTile
	{
		public WallVerticalLeftTile()
			: base("Images/MapTiles/WallVerticalLeft.png", false, true, false, false, false, false, false, false, false, true)
		{ }

		override public Object Clone()
		{ return new WallVerticalLeftTile(); }
	}
}
