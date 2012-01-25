using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PathFinding;

namespace TestHarness
{
	public class TestPathFinding
	{

		public void Run()
		{
			List<NodeConnection> connections = new List<NodeConnection>();

			PathfindingNode nodeA = new PathfindingNode() { Identifier = "A" };
			PathfindingNode nodeB = new PathfindingNode() { Identifier = "B" };
			PathfindingNode nodeC = new PathfindingNode() { Identifier = "C" };
			PathfindingNode nodeD = new PathfindingNode() { Identifier = "D" };
			PathfindingNode nodeE = new PathfindingNode() { Identifier = "E" };
			PathfindingNode nodeF = new PathfindingNode() { Identifier = "F" };

			PathGraph pathFinder = new PathGraph();
			pathFinder.AddConnection(nodeA, nodeB, 1);
			pathFinder.AddConnection(nodeA, nodeC, 4);
			pathFinder.AddConnection(nodeA, nodeD, 8);
			pathFinder.AddConnection(nodeB, nodeC, 6);
			pathFinder.AddConnection(nodeB, nodeC, 10);
			pathFinder.AddConnection(nodeC, nodeD, 3);
			pathFinder.AddConnection(nodeC, nodeE, 8);
			pathFinder.AddConnection(nodeD, nodeE, 1);

			pathFinder.RemoveAllConnectionsBetween(nodeA, nodeC);
			pathFinder.RemoveAllConnectionsBetween(nodeD, nodeE);

			List<PathfindingNode> fastestPath = pathFinder.FindRoute(nodeA, nodeE);

			if (fastestPath == null)
			{ Console.WriteLine("No path found!"); }
			else
			{
				Console.WriteLine("Fastest route is:");
				//while (fastestPath.Parent != null)
				//{

				//    Console.WriteLine(fastestPath.Node + " cost " + fastestPath.Cost);
				//    fastestPath = fastestPath.Parent;
				//}
				for (int i = 0; i < fastestPath.Count; i++)
				{
					PathfindingNode node = fastestPath[i];
					Console.WriteLine(i + ": " + node.Identifier);
				}
			}
		}
	}
}
