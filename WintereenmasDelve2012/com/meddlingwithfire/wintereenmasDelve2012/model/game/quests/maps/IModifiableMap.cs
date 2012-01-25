using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.model.game.quests.maps
{
	public interface IModifiableMap
	{
		void RemoveMapTile(MapTile tile);
	}
}
