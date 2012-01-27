using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.model.game.turnStepAction;
using WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.game;
using WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.chance;

namespace WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.model.game.actionStrategies
{
	public class CheatAndFindUnseenInterestingLocations : AbstractActionStrategy
	{
		public CheatAndFindUnseenInterestingLocations()
			: base(false)
		{

		}

		override public TurnStepAction FindAction(Avatar currentAvatar, AvatarTurnState avatarTurnState, QuestAnalyzer mapAnalyzer, ChanceProvider chanceProvider)
		{
			List<LocationOfInterest> interestingDestinations = mapAnalyzer.GetInterestingLocationsCheating(currentAvatar);
			if (interestingDestinations.Count <= 0)
			{ return null; }

			interestingDestinations = interestingDestinations.OrderBy(item => item.StepsToLocation).ToList();

			MovementTurnStepAction action = new MovementTurnStepAction(currentAvatar, interestingDestinations[0].StepsToLocation);
			return action;
		}
	}
}
