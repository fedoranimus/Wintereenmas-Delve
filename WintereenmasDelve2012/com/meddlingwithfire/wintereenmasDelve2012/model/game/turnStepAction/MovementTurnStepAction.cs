using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.game;
using WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.game.quests.maps;
using WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.model.game.quests.maps;
using WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.storyTelling;
using WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.game.quests;

namespace WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.model.game.turnStepAction
{
	public class MovementTurnStepAction : TurnStepAction
	{
		private Avatar _avatar;
		private List<Point> _stepsToLocation;
		private MapTile _interestingLocation;

		public MovementTurnStepAction(Avatar actor, PointList stepsToLocation, MapTile interestingLocation=null)
			: base(false)
		{
			_avatar = actor;
			_stepsToLocation = stepsToLocation;
			RequiresMovement = true;

			HasMoreTurns = (_stepsToLocation.Count > 1) ? true : false; // If there are more steps, then we can be used in subsequent turns!
			AcceptsAvatarFocus = HasMoreTurns;
		}

		override public void Commit(AbstractQuest quest, StoryTeller storyTeller)
		{
			//Story story = new Story();
			//story.Add(storyTeller.NarratorVoice, _avatar.ClassDescription + " moves");
			//storyTeller.StoryComplete += OnStoryComplete;
			//storyTeller.TellStory(story);

			// Look up the avatars tile
			MapTile avatarTile = quest.GetAvatarMapTile(_avatar);
			Point avatarCurrentLocation = quest.GetAvatarLocation(_avatar);

			// Update the avatars vector
			Point moveToPoint = _stepsToLocation[0];
			_stepsToLocation.RemoveAt(0);

			HasMoreTurns = (_stepsToLocation.Count > 0) ? true : false; // If there are more steps, then we can be used in subsequent turns!
			AcceptsAvatarFocus = HasMoreTurns;

			_avatar.movementVector.X = moveToPoint.X - avatarCurrentLocation.X;
			_avatar.movementVector.Y = moveToPoint.Y - avatarCurrentLocation.Y;
			
			_avatar.TurnState.MovementPointsLeft--;

			// Move the tile
			quest.Map.MoveMapTile(avatarTile, moveToPoint);
			quest.Map.SetFactionWalkedOn(_avatar.Faction, moveToPoint);

			DoComplete();
		}

		private void OnStoryComplete(object sender, EventArgs args)
		{ base.DoComplete(); }
	}
}
