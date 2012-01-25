using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.game;

namespace WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.model.game.quests.maps.tileActions
{
	public class AbstractTileAction
	{
		public AbstractTileAction()
			: base()
		{

		}

		virtual public Boolean IsAvailableTo(Faction faction)
		{
			switch (faction)
			{
				case Faction.Heroes:
					return true;
			}
			return false;
		}

		virtual public void Execute()
		{ } // Override by subclasses
	}
}
