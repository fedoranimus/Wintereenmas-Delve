using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.game;
using WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.game.quests.maps;

namespace WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.model.game.quests.maps.tileActions
{
	public class OpenTreasureChestAction : AbstractTileAction, IRequiresModifiableMap
	{
		private MapTile _tile;
		private QuestMap _map;

		public OpenTreasureChestAction(MapTile tile)
			: base()
		{
			_tile = tile;
		}

		public void SetModifiableMap(QuestMap map)
		{ _map = map; }

		public override void Execute()
		{
			//if (_hero == null)
			//{ throw new Exception("OpenTreasureChestAction Execute() called, but Hero has not been set!"); }
			
			//TODO: Draw a random treasure card & apply to hero

			// Fuck dat shit.  Remove the tile.
			_map.RemoveMapTile(_tile);
		}
	}
}
