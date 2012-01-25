using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.model.game.movement
{
	public class ConstantMovementBehavior : IAvatarMovementBehavior
	{
		public int Movement;

		public ConstantMovementBehavior(int movement)
			: base()
		{
			Movement = movement;
		}

		public int GetMovementPointsForTurn()
		{ return Movement; }
	}
}
