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
using WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.model.game.quests.maps.tileActions;
using WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.model.game.actionStrategies;

namespace WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.game
{
	public class Avatar
	{
		private AvatarClass _avatarClass;
		public Faction Faction;

		private List<Item> _equippedItems;
		private List<Item> _backpackItems;

		private int _goldInPossession;

		public String ImagePath;

		public AvatarTurnState TurnState;

		private Boolean _sentDeathMessage;

		public int ActualBodyPointsRemaining;
		public int ActualMindPointsRemaining;

		public Point movementVector;

		/// <summary>
		/// The action that the Avatar is focused on.  Will continue to consume
		/// turn cycles until the action is completely finished, or the Avatars' 
		/// BreakFocus() method has been called.
		/// </summary>
		private TurnStepAction _focusAction;

		public Avatar(Faction faction, AvatarClass avatarClass, String imagePath)
			: base()
		{
			Faction = faction;
			_avatarClass = avatarClass;
			ImagePath = imagePath;

			movementVector = new Point(0, 0);

			TurnState = new AvatarTurnState();

			ActualBodyPointsRemaining = avatarClass.BaseMaximumBodyPoints;
			ActualMindPointsRemaining = avatarClass.BaseMaximumMindPoints;

			_goldInPossession = 0;
			_equippedItems = new List<Item>();
			_backpackItems = new List<Item>();
		}

		public void BreakFocus()
		{ _focusAction = null; }

		public void DefendAgainst(int attackDamageRolled, ChanceProvider chanceProvider)
		{
			BreakFocus(); // If the Avatar is hit, break the focus of the Avatar.

			// TODO: Defend!
		}

		public Boolean IsHeroAlive
		{
			get { return ActualBodyPointsRemaining > 0; }
		}

		public String ClassDescription
		{
			get { return _avatarClass.Description; }
		}

		public Boolean CanAttackAdjacent
		{
			get { return false; } // TODO: Loop through items and check for adjacent-attacking weapons
		}

		/// <summary>
		/// Called just before the avatars turn begins.  Gives the class a chance to prep whatever one-time calculations it needs to perform. 
		/// For example, rolling for movement dice is only done once per turn.
		/// </summary>
		virtual public void StartTurn()
		{
			// Roll for movement dice
			TurnState.TotalMovementPointsForTurn = _avatarClass.BaseMovementBehavior.GetMovementPointsForTurn();
			TurnState.MovementPointsLeft = TurnState.TotalMovementPointsForTurn;
			TurnState.HasTakenAction = false;
			_sentDeathMessage = false;
		}

		virtual public TurnStepAction DoTakeTurnStep(QuestAnalyzer mapAnalyzer, ChanceProvider chanceProvider)
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
			if (TurnState.MovementPointsLeft <= 0 && TurnState.HasTakenAction) // We can continue our turn as long as we have movement points left, and have not taken our action
			{ return null; }

			// Determine the action to perform
			TurnStepAction action = null;
			for (int i = 0; i < _avatarClass.ActionStrategies.Count && action == null; i++)
			{
				AbstractActionStrategy strategy = _avatarClass.ActionStrategies[i];
				if (_focusAction == null || strategy.CanBreakFocus)
				{
					action = strategy.FindAction(this, TurnState, mapAnalyzer, chanceProvider);
					
					if (action != null && !MayTakeAction(action, TurnState))
					{ action = null; }

					if (action != null)
					{ _focusAction = null; }
				}
			}

			if (_focusAction != null && _focusAction.AcceptsAvatarFocus && _focusAction.HasMoreTurns) // If our focus action still has stuff to do, let it keep focus
			{
				if (MayTakeAction(_focusAction, TurnState))
				{ action = _focusAction; }
			}

			if (action == null) // If we can't figure anything else out, then end the turn.
			{
				action = new ConfusedTurnStepAction(this); // In the real game, this should never happen.  Avatars should always find *something* to do.
				TurnState.HasTakenAction = true;
				TurnState.MovementPointsLeft = 0;
			}

			if (action.AcceptsAvatarFocus && action.HasMoreTurns) // If the action is multi-step, focus on it until something interrupts us
			{ _focusAction = action; }

			return action;
		}

		private Boolean MayTakeAction(TurnStepAction action, AvatarTurnState turnState)
		{
			if (action.RequiresAction && turnState.HasTakenAction)
			{ return false; } // Cannot use this action if the player has already taken his action this turn.
			else if (action.RequiresMovement && turnState.MovementPointsLeft <= 0)
			{ return false; } // Cannot use this action if the player has no movement points left this turn.
			return true;
		}
	}
}
