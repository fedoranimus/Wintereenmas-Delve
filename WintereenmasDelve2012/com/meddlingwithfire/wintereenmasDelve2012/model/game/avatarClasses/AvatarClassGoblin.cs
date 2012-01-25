using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.model.game.movement;

namespace WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.model.game.avatarClasses
{
	public class AvatarClassGoblin : AvatarClass
	{
		public AvatarClassGoblin()
			: base(2, 1, 1, 1, new ConstantMovementBehavior(10), "Images/MapTiles/Goblin.png", "Goblin")
		{ }
	}
}
