using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.model.game.quests.maps.tileActions;
using WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.game.quests.maps;
using WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.game;
using WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.model.game.quests.maps;
using WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.storyTelling;

namespace WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.model.game.turnStepAction
{
	public class ActionableTurnStepAction : TurnStepAction
	{
		private AbstractTileAction _tileAction;
		private Avatar _avatar;

		public ActionableTurnStepAction(AbstractTileAction tileAction, Avatar avatar)
		{
			_tileAction = tileAction;
			_avatar = avatar;
		}

		override public void Commit(QuestMap map, Dictionary<Avatar, MapTile> avatarTiles, StoryTeller storyTeller)
		{
			if (_tileAction is IRequiresModifiableMap)
			{ ((IRequiresModifiableMap)_tileAction).SetModifiableMap(map); }
			if (_tileAction is IRequiresAvatar)
			{ ((IRequiresAvatar)_tileAction).SetAvatar(_avatar); }
			if (_tileAction is IRequiresAvatarTiles)
			{ ((IRequiresAvatarTiles)_tileAction).SetAvatarTiles(avatarTiles); }

			_tileAction.Execute();

			DoComplete();
		}
	}
}
