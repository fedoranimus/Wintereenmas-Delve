using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.game;

namespace WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.model.game.quests.maps.tileActions
{
	public class OpenTreasureChestAction : AbstractTileAction, IRequiresHero
	{
		private Hero _hero;

		public OpenTreasureChestAction()
			: base()
		{

		}

		public void setHero(Hero hero)
		{ _hero = hero; }

		public override void Execute()
		{
			if (_hero == null)
			{ throw new Exception("OpenTreasureChestAction Execute() called, but Hero has not been set!"); }
			
			//TODO: Draw a random treasure card
		}
	}
}
