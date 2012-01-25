using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.model.game.items;
using WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.model.game.avatarClasses;

namespace WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.game
{
	public class Hero : Avatar
	{
		public Hero(AvatarClass heroClass)
			: base(Faction.Heroes, heroClass, heroClass.ImagePath)
		{
			
		}
	}
}