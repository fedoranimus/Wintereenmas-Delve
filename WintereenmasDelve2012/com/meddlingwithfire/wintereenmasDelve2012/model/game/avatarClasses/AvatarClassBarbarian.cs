using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.model.game.avatarClasses;
using WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.model.game.items;
using WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.model.game.movement;
using WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.chance;

namespace WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.model.game.avatarClasses
{
	public class AvatarClassBarbarian : AvatarClass
	{
		public AvatarClassBarbarian(ChanceProvider chanceProvider)
			: base(3, 2, 8, 2, new ChanceMovementBehavior(chanceProvider, 2), "Images/MapTiles/HeroBarbarian.png", "Barbarian")
		{

		}

		public override List<Item> GenerateStartingItems()
		{
			List<Item> startingItems = new List<Item>();

			return startingItems;
		}
	}
}
