using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.game;
using WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.chance;
using WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.model.game.turnStepAction;

namespace WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.model.game.actionStrategies
{
	public class SearchForInterestingLocationsStrategy : AbstractActionStrategy
	{
		public SearchForInterestingLocationsStrategy()
			: base(false)
		{ }

		override public TurnStepAction FindAction(Avatar currentAvatar, AvatarTurnState avatarTurnState, QuestAnalyzer mapAnalyzer, ChanceProvider chanceProvider)
		{
			List<LocationOfInterest> interestingDestinations = mapAnalyzer.GetInterestingLocations(currentAvatar);
			if (interestingDestinations.Count <= 0)
			{ return null; }

			interestingDestinations = interestingDestinations.OrderBy(item => item.StepsToLocation).ToList();
			MovementTurnStepAction action = new MovementTurnStepAction(currentAvatar, interestingDestinations[0].StepsToLocation);

			return action;
		}
	}
}
