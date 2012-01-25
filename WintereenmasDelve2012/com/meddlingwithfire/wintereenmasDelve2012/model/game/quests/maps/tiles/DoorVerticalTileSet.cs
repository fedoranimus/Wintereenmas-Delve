using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.model.game.quests.maps.tileActions;

namespace WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.model.game.quests.maps.tiles
{
	public class DoorVerticalTileSet : MapTileSet
	{
		public DoorVerticalTileSet()
			: base()
		{
			MapTile leftTile = new MapTile("Images/MapTiles/DoorVerticalLeft.png", false, false, false, true, false, false, false, true, false, false);
			MapTile rightTile = new MapTile("Images/MapTiles/DoorVerticalRight.png", false, true, false, false, false, false, false, false, false, true);

			OpenDoorAction openDoorAction = new OpenDoorAction(leftTile, rightTile);
			leftTile.AddAction(openDoorAction);
			rightTile.AddAction(openDoorAction);

			AddTile(leftTile, new Point(0, 0));
			AddTile(rightTile, new Point(1, 0));
		}
	}
}
