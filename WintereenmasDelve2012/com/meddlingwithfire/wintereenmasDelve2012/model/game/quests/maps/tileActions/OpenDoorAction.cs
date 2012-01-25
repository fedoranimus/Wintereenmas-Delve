using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.model.game.quests.maps;

namespace WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.model.game.quests.maps.tileActions
{
	public class OpenDoorAction : AbstractTileAction, IRequiresModifiableMap
	{
		private MapTile _doorTileA;
		private MapTile _doorTileB;

		private IModifiableMap _map;

		public OpenDoorAction(MapTile doorTileA, MapTile doorTileB)
			: base()
		{
			_doorTileA = doorTileA;
			_doorTileB = doorTileB;
		}

		public void SetModifiableMap(IModifiableMap map)
		{ _map = map; }

		public override void Execute()
		{
			if (_map == null)
			{ throw new Exception("OpenDoorAction Execute called, but no IModifiableMap was set!"); }

			// Remove the door tiles from the map
			_map.RemoveMapTile(_doorTileA);
			_map.RemoveMapTile(_doorTileB);
		}
	}
}
