using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.model.game.quests.maps.tiles
{
	public class StaircaseTileSet : MapTileSet
	{
		public StaircaseTileSet()
			: base()
		{
			AddTile(new MapTile("Images/MapTiles/StaircaseA.png", false, false, false, false, false), new Point(0, 0));
			AddTile(new MapTile("Images/MapTiles/StaircaseB.png", false, false, false, false, false), new Point(1, 0));
			AddTile(new MapTile("Images/MapTiles/StaircaseC.png", false, false, false, false, false), new Point(0, 1));
			AddTile(new MapTile("Images/MapTiles/StaircaseD.png", false, false, false, false, false), new Point(1, 1));
		}
	}
}
