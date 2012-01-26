using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.game;
using WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.game.quests.maps;
using WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.model.game.quests.maps;
using WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.storyTelling;

namespace WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.model.game.turnStepAction
{
	public class MovementTurnStepAction : TurnStepAction
	{
		private Avatar _avatar;
		private Point _moveToPoint;

		public MovementTurnStepAction(Avatar actor, Point toLocation)
			: base()
		{
			_avatar = actor;
			_moveToPoint = toLocation;
		}

		override public void Commit(QuestMap map, Dictionary<Avatar, MapTile> avatarTiles, StoryTeller storyTeller)
		{
			Story story = new Story();
			story.Add(storyTeller.NarratorVoice, _avatar.ClassDescription + " moves");
			storyTeller.StoryComplete += OnStoryComplete;
			storyTeller.TellStory(story);

			// Look up the avatars tile
			MapTile avatarTile = avatarTiles[_avatar];
			Point avatarCurrentLocation = map.GetMapTileLocation(avatarTile);

			// Update the avatars vector
			_avatar.movementVector.X = _moveToPoint.X - avatarCurrentLocation.X;
			_avatar.movementVector.Y = _moveToPoint.Y - avatarCurrentLocation.Y;

			// Move the tile
			map.MoveMapTile(avatarTile, _moveToPoint);
			map.SetFactionWalkedOn(_avatar.Faction, _moveToPoint);
		}

		private void OnStoryComplete(object sender, EventArgs args)
		{ base.DoComplete(); }
	}
}
