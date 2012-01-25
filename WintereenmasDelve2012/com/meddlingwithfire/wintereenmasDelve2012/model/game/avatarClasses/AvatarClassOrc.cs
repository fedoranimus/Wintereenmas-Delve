using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.model.game.movement;

namespace WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.model.game.avatarClasses
{
	public class AvatarClassOrc : AvatarClass
	{
		public AvatarClassOrc()
			: base(3, 2, 1, 2, new ConstantMovementBehavior(8), "Images/MapTiles/Orc.png", "Orc")
		{ }
	}
}
