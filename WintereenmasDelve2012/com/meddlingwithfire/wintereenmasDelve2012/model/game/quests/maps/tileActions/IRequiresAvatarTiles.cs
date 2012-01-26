﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.game;

namespace WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.model.game.quests.maps.tileActions
{
	public interface IRequiresAvatarTiles
	{
		void SetAvatarTiles(Dictionary<Avatar, MapTile> avatarTiles);
	}
}
