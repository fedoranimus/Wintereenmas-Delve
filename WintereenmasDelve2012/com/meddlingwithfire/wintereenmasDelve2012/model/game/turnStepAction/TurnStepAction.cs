using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.game.quests.maps;
using WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.game;
using WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.model.game.quests.maps;
using WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.storyTelling;

namespace WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.model.game.turnStepAction
{
	public class TurnStepAction
	{
		public EventHandler Complete;

		public TurnStepAction()
			: base()
		{

		}

		public virtual void Commit(QuestMap map, Dictionary<Avatar, MapTile> avatarTiles, StoryTeller storyTeller)
		{ } // Override with subclasses

		protected void DoComplete()
		{
			if (Complete != null)
			{ Complete(this, new EventArgs()); }
		}
	}
}
