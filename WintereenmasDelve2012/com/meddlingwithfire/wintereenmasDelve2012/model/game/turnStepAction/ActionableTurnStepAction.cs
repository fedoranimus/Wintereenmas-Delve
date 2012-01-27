using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.model.game.quests.maps.tileActions;
using WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.game.quests.maps;
using WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.game;
using WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.model.game.quests.maps;
using WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.storyTelling;
using WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.game.quests;

namespace WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.model.game.turnStepAction
{
	public class ActionableTurnStepAction : TurnStepAction
	{
		private AbstractTileAction _tileAction;
		private Avatar _avatar;

		public ActionableTurnStepAction(AbstractTileAction tileAction, Avatar avatar):base(false)
		{
			_tileAction = tileAction;
			_avatar = avatar;
		}

		override public void Commit(AbstractQuest quest, StoryTeller storyTeller)
		{
			if (_tileAction is IRequiresModifiableMap)
			{ ((IRequiresModifiableMap)_tileAction).SetModifiableMap(quest.Map); }
			if (_tileAction is IRequiresAvatar)
			{ ((IRequiresAvatar)_tileAction).SetAvatar(_avatar); }
			if (_tileAction is IRequiresQuest)
			{ ((IRequiresQuest)_tileAction).SetQuest(quest); }

			if (_tileAction.ConsumesTurnAction)
			{
				_avatar.TurnState.HasTakenAction = true;
				if (_avatar.TurnState.TotalMovementPointsForTurn != _avatar.TurnState.MovementPointsLeft)
				{ _avatar.TurnState.MovementPointsLeft = 0; }
			}
			
			_tileAction.Execute();

			DoComplete();
		}
	}
}
