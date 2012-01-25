using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.model.game.avatarClasses;

namespace WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.game
{
	public class Monster : Avatar
	{
		public Monster(AvatarClass avatarClass)
			: base(Faction.Xargon, avatarClass, avatarClass.ImagePath)
		{

		}
	}
}
