using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.game;

namespace WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.model.game.items
{


	public class Item
	{
		public List<ItemSlot> RequiresSlots;
		public String Name;

		public Boolean CouldBeEquipedBy(Avatar avatar)
		{ return true; }

	}
}
