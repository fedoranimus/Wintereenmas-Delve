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
		private Avatar _actor;
		private Point _moveToPoint;

		public MovementTurnStepAction(Avatar actor, Point toLocation)
			: base()
		{
			_actor = actor;
			_moveToPoint = toLocation;
		}

		override public void Commit(QuestMap map, Dictionary<Avatar, MapTile> avatarTiles, StoryTeller storyTeller)
		{
			Story story = new Story();
			story.Add(storyTeller.NarratorVoice, _actor.ClassDescription + " moves");
			storyTeller.StoryComplete += OnStoryComplete;
			storyTeller.TellStory(story);

			// Look up the avatars tile
			MapTile avatarTile = avatarTiles[_actor];

			// Move the tile
			map.MoveMapTile(avatarTile, _moveToPoint);
		}

		private void OnStoryComplete(object sender, EventArgs args)
		{ base.DoComplete(); }
	}
}
