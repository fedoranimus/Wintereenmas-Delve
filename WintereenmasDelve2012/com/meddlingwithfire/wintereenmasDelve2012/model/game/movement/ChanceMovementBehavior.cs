using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.chance;

namespace WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.model.game.movement
{
	public class ChanceMovementBehavior : IAvatarMovementBehavior
	{
		public int NumberOFMovementDice;
		private ChanceProvider _chanceProvider;

		public ChanceMovementBehavior(ChanceProvider chanceProvider, int numberOfMovementDice)
			: base()
		{
			_chanceProvider = chanceProvider;
			NumberOFMovementDice = numberOfMovementDice;
		}

		public int GetMovementPointsForTurn()
		{
			int totalMovementPoints = 0;
			for (int i = 0; i < NumberOFMovementDice; i++)
			{ totalMovementPoints += _chanceProvider.NextSubmission().Value; }
			return totalMovementPoints;
		}
	}
}
