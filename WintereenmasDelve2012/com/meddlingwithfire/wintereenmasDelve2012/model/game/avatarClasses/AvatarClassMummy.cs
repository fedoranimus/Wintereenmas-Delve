using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.model.game.movement;

namespace WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.model.game.avatarClasses
{
	public class AvatarClassMummy : AvatarClass
	{
		public AvatarClassMummy()
			: base(3, 4, 2, 0, new ConstantMovementBehavior(4), "Images/MapTiles/MonsterUnknown.png", "Mummy")
		{

		}
	}
}
