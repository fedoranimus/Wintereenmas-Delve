using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.model.game.movement;
using WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.model.game.items;
using WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.chance;

namespace WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.model.game.avatarClasses
{
	public class AvatarClassDwarf : AvatarClass
	{
		public AvatarClassDwarf(ChanceProvider chanceProvider)
			: base(2, 2, 7, 3, new ChanceMovementBehavior(chanceProvider, 2), "Images/MapTiles/HeroDwarf.png", "Dwarf")
		{

		}

		public override List<Item> GenerateStartingItems()
		{
			List<Item> startingItems = new List<Item>();

			return startingItems;
		}
	}
}
