using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.model.game.turnStepAction;
using WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.game;
using WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.chance;

namespace WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.model.game.actionStrategies
{
	public class SearchForAdjacentUnvisitedLocationsStrategy : AbstractActionStrategy
	{
		public SearchForAdjacentUnvisitedLocationsStrategy()
			: base(false)
		{

		}

		override public TurnStepAction FindAction(Avatar currentAvatar, AvatarTurnState avatarTurnState, MapAnalyzer mapAnalyzer, ChanceProvider chanceProvider)
		{
			PointList unwalkedTiles = mapAnalyzer.GetAdjacentUnvisitedLocations(currentAvatar);
			if (unwalkedTiles.Count <= 0)
			{ return null; }

			// Just pick the first tile, and see if that opens up any other actions...
			PointList path = new PointList() { unwalkedTiles[0] };

			MovementTurnStepAction action = new MovementTurnStepAction(currentAvatar, path);
			return action;
		}
	}
}
