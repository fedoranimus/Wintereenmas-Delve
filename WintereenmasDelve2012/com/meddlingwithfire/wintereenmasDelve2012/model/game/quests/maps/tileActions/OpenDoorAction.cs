using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.model.game.quests.maps;
using WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.game.quests.maps;
using WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.game;
using WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.game.quests;

namespace WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.model.game.quests.maps.tileActions
{
	public class OpenDoorAction : AbstractTileAction, IRequiresAvatar, IRequiresQuest
	{
		private MapTile _doorTileA;
		private MapTile _doorTileB;

		private AbstractQuest _quest;
		private Avatar _avatar;

		public OpenDoorAction(MapTile doorTileA, MapTile doorTileB)
			: base(false)
		{
			_doorTileA = doorTileA;
			_doorTileB = doorTileB;
		}

		public void SetQuest(AbstractQuest quest)
		{ _quest = quest; }

		public void SetAvatar(Avatar avatar)
		{ _avatar = avatar; }

		public override void Execute()
		{
			if (_quest == null)
			{ throw new Exception("OpenDoorAction Execute called, but no AbstractQuest was set!"); }

			Point avatarLocation = _quest.GetAvatarLocation(_avatar);
			Point aLocation = _quest.Map.GetMapTileLocation(_doorTileA);
			Point bLocation = _quest.Map.GetMapTileLocation(_doorTileB);

			// Update the avatars movement vector to fit the door he opened.
			Point nextPoint = aLocation;
			if (avatarLocation.X == aLocation.X && avatarLocation.Y == aLocation.Y)
			{ nextPoint = bLocation; }

			_avatar.movementVector.X = nextPoint.X - avatarLocation.X;
			_avatar.movementVector.Y = nextPoint.Y - avatarLocation.Y;

			// Remove the door tiles from the map
			_quest.Map.RemoveMapTile(_doorTileA);
			_quest.Map.RemoveMapTile(_doorTileB);
		}
	}
}
