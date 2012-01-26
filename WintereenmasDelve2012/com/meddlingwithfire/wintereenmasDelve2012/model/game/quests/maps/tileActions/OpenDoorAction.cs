using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.model.game.quests.maps;
using WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.game.quests.maps;
using WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.game;

namespace WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.model.game.quests.maps.tileActions
{
	public class OpenDoorAction : AbstractTileAction, IRequiresModifiableMap, IRequiresAvatar, IRequiresAvatarTiles
	{
		private MapTile _doorTileA;
		private MapTile _doorTileB;

		private QuestMap _map;
		private Avatar _avatar;
		private Dictionary<Avatar, MapTile> _avatarTiles;

		public OpenDoorAction(MapTile doorTileA, MapTile doorTileB)
			: base()
		{
			_doorTileA = doorTileA;
			_doorTileB = doorTileB;
		}

		public void SetModifiableMap(QuestMap map)
		{ _map = map; }

		public void SetAvatar(Avatar avatar)
		{ _avatar = avatar; }

		public void SetAvatarTiles(Dictionary<Avatar, MapTile> avatarTiles)
		{ _avatarTiles = avatarTiles; }

		public override void Execute()
		{
			if (_map == null)
			{ throw new Exception("OpenDoorAction Execute called, but no IModifiableMap was set!"); }

			Point avatarLocation = _map.GetMapTileLocation(_avatarTiles[_avatar]);
			Point aLocation = _map.GetMapTileLocation(_doorTileA);
			Point bLocation = _map.GetMapTileLocation(_doorTileB);

			// Update the avatars movement vector to fit the door he opened.
			Point nextPoint = aLocation;
			if (avatarLocation.X == aLocation.X && avatarLocation.Y == aLocation.Y)
			{ nextPoint = bLocation; }

			_avatar.movementVector.X = nextPoint.X - avatarLocation.X;
			_avatar.movementVector.Y = nextPoint.Y - avatarLocation.Y;

			// Remove the door tiles from the map
			_map.RemoveMapTile(_doorTileA);
			_map.RemoveMapTile(_doorTileB);
		}
	}
}
