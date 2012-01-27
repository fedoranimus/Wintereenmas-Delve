using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.game;
using WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.chance;
using WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.model.game.turnStepAction;
using WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.model.game.quests.maps.tileActions;

namespace WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.model.game.actionStrategies
{
	public class SearchForCurrentTileActionsStrategy : AbstractActionStrategy
	{
		public SearchForCurrentTileActionsStrategy()
			: base(true)
		{ }

		override public TurnStepAction FindAction(Avatar currentAvatar, AvatarTurnState avatarTurnState, QuestAnalyzer mapAnalyzer, ChanceProvider chanceProvider)
		{
			List<AbstractTileAction> actionsForCurrentLocation = mapAnalyzer.GetActionsAtObserverLocation(currentAvatar);
			if (actionsForCurrentLocation.Count <= 0)
			{ return null; }

			AbstractTileAction tileAction = actionsForCurrentLocation[0];
			TurnStepAction action = new ActionableTurnStepAction(tileAction, currentAvatar);

			return action;
		}
	}
}
