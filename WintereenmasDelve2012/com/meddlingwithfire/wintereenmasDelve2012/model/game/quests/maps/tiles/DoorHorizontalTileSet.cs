using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.model.game.quests.maps.tileActions;

namespace WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.model.game.quests.maps.tiles
{
	public class DoorHorizontalTileSet : MapTileSet
	{
		public DoorHorizontalTileSet()
			: base()
		{
			MapTile upperTile = new MapTile("Images/MapTiles/DoorHorizontalTop.png", false, false, false, false, true, false, false, false, true, false);
			MapTile lowerTile = new MapTile("Images/MapTiles/DoorHorizontalBottom.png", false, false, true, false, false, false, true, false, false, false);

			OpenDoorAction openDoorAction = new OpenDoorAction(upperTile, lowerTile);
			upperTile.AddAction(openDoorAction);
			lowerTile.AddAction(openDoorAction);

			AddTile(upperTile, new Point(0, 0));
			AddTile(lowerTile, new Point(0, 1));
		}
	}
}