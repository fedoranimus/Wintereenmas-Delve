using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PathFinding
{
    public class NodeConnection
    {
		public PathfindingNode Start;
		public PathfindingNode End;
		public double Cost;

		public NodeConnection(PathfindingNode start, PathfindingNode end, double cost=1.0)
			: base()
		{
			Start = start;
			End = end;
			Cost = cost;
		}

		public Boolean ContainsNode(PathfindingNode node)
		{
			return (Start == node || End == node);
		}

		public Boolean ContainsNode(String nodeIdentifier)
		{
			return (Start.Identifier.Equals(nodeIdentifier) || End.Identifier.Equals(nodeIdentifier));
		}

		public PathfindingNode FindNode(String nodeIdentifier)
		{
			if (Start.Identifier.Equals(nodeIdentifier))
			{ return Start; }
			else if (End.Identifier.Equals(nodeIdentifier))
			{ return End; }
			return null;
		}

		public PathfindingNode GetOppositeNode(PathfindingNode fromNode)
		{
			if (fromNode == Start)
			{ return End; }
			else if (fromNode == End)
			{ return Start; }
			return null;
		}
	}
}