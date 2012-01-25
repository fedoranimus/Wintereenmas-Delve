using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PathFinding;

namespace WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.model.game.quests.maps
{
	public class QuestMapRoom
	{
		/// <summary>
		/// The list of nodes that this section contains
		/// </summary>
		public List<PathfindingNode> Nodes;

		public QuestMapRoom()
			: base()
		{
			Nodes = new List<PathfindingNode>();
		}
	}
}
