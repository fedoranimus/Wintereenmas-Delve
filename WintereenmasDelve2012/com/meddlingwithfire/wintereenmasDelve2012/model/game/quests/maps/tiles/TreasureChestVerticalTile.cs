using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.model.game.quests.maps.tileActions;

namespace WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.model.game.quests.maps.tiles
{
	public class TreasureChestVerticalTile : MapTile
	{
		public TreasureChestVerticalTile()
			: base("Images/MapTiles/TreasureChestVertical.png", false, false, false, false, false)
		{
			Actions.Add(new OpenTreasureChestAction(this));
		}
	}
}
