using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PathFinding
{
	public class PathStep
	{
		public PathfindingNode Node; // The node that we are currently on
		public double Cost;
		public PathStep Ancestor; // The node that the Cost came from.
	}
}
