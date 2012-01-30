using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.game.quests.maps;
using WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.game;
using WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.model.game.quests.maps;
using WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.storyTelling;
using WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.game.quests;

namespace WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.model.game.turnStepAction
{
	public class TurnStepAction
	{
		public EventHandler Complete;

		public Boolean AcceptsAvatarFocus;
		public Boolean HasMoreTurns;
		public Boolean RequiresAction;
		public Boolean RequiresMovement;

		public TurnStepAction(Boolean acceptsAvatarFocus)
			: base()
		{
			AcceptsAvatarFocus = acceptsAvatarFocus;
			HasMoreTurns = false;
			RequiresAction = false;
			RequiresMovement = false;
		}

		public virtual void Commit(AbstractQuest map, StoryTeller storyTeller)
		{ } // Override with subclasses

		protected void DoComplete()
		{
			if (Complete != null)
			{ Complete(this, new EventArgs()); }
		}
	}
}
