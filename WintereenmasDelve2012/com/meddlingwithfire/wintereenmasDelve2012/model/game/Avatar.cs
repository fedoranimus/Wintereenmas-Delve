using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.model.game.items;
using WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.model.game.avatarClasses;
using WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.model.game.movement;
using WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.chance;
using WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.model.game.turnStepAction;
using WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.model.game.quests.maps;
using WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.model;
using WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.model.game;

namespace WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.game
{
	public class Avatar
	{
		private AvatarClass _avatarClass;
		public Faction Faction;

		private List<Item> _equipedItems;
		private List<Item> _backpackItems;

		private int _goldInPossession;

		public String ImagePath;

		private int _movementPointsLeftForTurn;
		private Boolean _hasTakenTurnAction;
		private Boolean _sentDeathMessage;

		public int ActualBodyPointsRemaining;
		public int ActualMindPointsRemaining;

		public Avatar(Faction faction, AvatarClass avatarClass, String imagePath)
			: base()
		{
			Faction = faction;
			_avatarClass = avatarClass;
			ImagePath = imagePath;

			ActualBodyPointsRemaining = avatarClass.BaseMaximumBodyPoints;
			ActualMindPointsRemaining = avatarClass.BaseMaximumMindPoints;

			_goldInPossession = 0;
			_equipedItems = new List<Item>();
			_backpackItems = new List<Item>();
		}

		public void DefendAgainst(int attackDamageRolled, ChanceProvider chanceProvider)
		{

		}

		public Boolean IsHeroAlive
		{
			get { return ActualBodyPointsRemaining > 0; }
		}

		public String ClassDescription
		{
			get { return _avatarClass.Description; }
		}

		/// <summary>
		/// Called just before the avatars turn begins.  Gives the class a chance to prep whatever one-time calculations it needs to perform. 
		/// For example, rolling for movement dice is only done once per turn.
		/// </summary>
		virtual public void StartTurn()
		{
			// Roll for movement dice
			_movementPointsLeftForTurn = _avatarClass.BaseMovementBehavior.GetMovementPointsForTurn();
			_hasTakenTurnAction = false;
			_sentDeathMessage = false;
		}

		virtual public TurnStepAction DoTakeTurnStep(MapAnalyzer mapAnalyzer, ChanceProvider chanceProvider)
		{
			if (!IsHeroAlive) // Can't play if dead.
			{
				if (_sentDeathMessage)
				{ return null; }
				else
				{
					_sentDeathMessage = true;
					return new DeadTurnStepAction(this);
				}
			}
			if (_movementPointsLeftForTurn <= 0 && _hasTakenTurnAction) // We can continue our turn as long as we have movement points left, and have not taken our action
			{ return null; }

			List<LocationOfInterest> movementDestinations = mapAnalyzer.GetInterestingLocations(this);
			movementDestinations = movementDestinations.OrderBy(item => item.StepsToLocation).ToList();

			// TODO: Implement AI
			TurnStepAction action = null;
			if (movementDestinations.Count <= 0)
			{
				action = new ConfusedTurnStepAction(this); // In the real game, this should never happen.  Avatars should always find *something* to do.
				_movementPointsLeftForTurn = 0; // End the turn
				_hasTakenTurnAction = true; // End the turn
			}
			else
			{
				action = new MovementTurnStepAction(this, movementDestinations[0].StepsToLocation[0]);
				_movementPointsLeftForTurn--;
			}

			//ActualBodyPointsRemaining -= 2; // Testing death scenario
			_hasTakenTurnAction = true; // TODO: only set to true if the Avatar has taken their turn action

			return action;
		}
	}
}
