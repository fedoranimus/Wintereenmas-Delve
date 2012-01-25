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
	public class DeadTurnStepAction : TurnStepAction
	{
		private Avatar _actor;

		public DeadTurnStepAction(Avatar actor)
			: base()
		{
			_actor = actor;
		}

		override public void Commit(QuestMap map, Dictionary<Avatar, MapTile> avatarTiles, StoryTeller storyTeller)
		{
			Story story = new Story();
			story.Add(storyTeller.NarratorVoice, _actor.ClassDescription + " is dead.");
			storyTeller.StoryComplete += OnStoryComplete;
			storyTeller.TellStory(story);
		}

		private void OnStoryComplete(object sender, EventArgs args)
		{ base.DoComplete(); }
	}
}
