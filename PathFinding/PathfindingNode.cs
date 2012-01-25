using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PathFinding
{
    public class PathfindingNode
    {
		public String Identifier;

		public PathfindingNode(String identifier=null)
			: base()
		{
			Identifier = identifier;
			if (Identifier == null)
			{ Identifier = Guid.NewGuid().ToString(); }
		}
	}
}