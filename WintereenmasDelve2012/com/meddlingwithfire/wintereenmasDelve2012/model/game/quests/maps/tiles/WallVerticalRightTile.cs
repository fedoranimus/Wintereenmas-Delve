using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.model.game.quests.maps.tiles
{
	public class WallVerticalRightTile : MapTile
	{
		public WallVerticalRightTile()
			: base("Images/MapTiles/WallVerticalRight.png", false, false, false, true, false, false, false, true, false, false)
		{ }

		override public Object Clone()
		{ return new WallVerticalRightTile(); }
	}
}
