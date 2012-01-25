using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PathFinding
{
    public class PathGraph
    {
		private List<PathfindingNode> _nodes;
		private List<NodeConnection> _connections;

		public PathGraph():base()
		{
			_nodes = new List<PathfindingNode>();
			_connections = new List<NodeConnection>();
		}

		public void AddNode(PathfindingNode node)
		{
			_nodes.Add(node);
		}

		public void RemoveNode(PathfindingNode node)
		{
			// Remove the node
			_nodes.Remove(node);

			// Remove any connections that have the node in it
			for(int i=_connections.Count-1; i>= 0; i--)
			{
				NodeConnection connection = _connections[i];
				if (connection.ContainsNode(node))
				{ _connections.RemoveAt(i); }
			}
		}

		public PathfindingNode FindNodeByIdentifier(String identifier)
		{
			foreach (PathfindingNode node in _nodes)
			{
				if (node.Identifier.Equals(identifier))
				{ return node; }
			}
			return null;
		}

		public Boolean HasDirectConnectionBetween(PathfindingNode nodeA, PathfindingNode nodeB)
		{
			foreach (NodeConnection connection in _connections)
			{
				if (connection.Start == nodeA && connection.End == nodeB)
				{ return true; }
				if (connection.Start == nodeB && connection.End == nodeA)
				{ return true; }
			}
			return false;
		}

		public void AddConnection(PathfindingNode nodeA, PathfindingNode nodeB, int cost)
		{
			if (!_nodes.Contains(nodeA))
			{ _nodes.Add(nodeA); }
			if (!_nodes.Contains(nodeB))
			{ _nodes.Add(nodeB); }

			NodeConnection connection = new NodeConnection(nodeA, nodeB, cost);
			_connections.Add(connection);
		}

		public void RemoveAllConnectionsBetween(PathfindingNode nodeA, PathfindingNode nodeB)
		{
			for (int i = _connections.Count - 1; i >= 0; i--)
			{
				NodeConnection connection = _connections[i];
				if (connection.ContainsNode(nodeA) && connection.ContainsNode(nodeB))
				{ _connections.RemoveAt(i); }
			}
		}

		public List<PathfindingNode> FindRoute(PathfindingNode start, PathfindingNode end)
		{ return DoFindRoute(_connections, start, end); }

		public List<PathfindingNode> FindRoute(String startNodeIdentifier, String endNodeIdentifier)
		{
			PathfindingNode start = FindNode(_connections, startNodeIdentifier);
			PathfindingNode end = FindNode(_connections, endNodeIdentifier);
			return DoFindRoute(_connections, start, end);
		}

		private List<PathfindingNode> DoFindRoute(List<NodeConnection> connections, PathfindingNode startNode, PathfindingNode endNode)
		{
			System.Diagnostics.Debug.WriteLine("PathGraph DoFindRoute(connections, startNode, endNode)");
			DateTime startTime = DateTime.Now;

			PathStep destinationPoint = new PathStep() { Node = endNode, Cost = 0, Ancestor = null };
			List<PathStep> visitedPoints = new List<PathStep>();
			visitedPoints.Add(destinationPoint);

			List<PathStep> pointsLeftToVisit = GetNewMoveableCoordinatesFromPoint(destinationPoint, visitedPoints);
			List<PathStep> newPoints;

			PathStep startPoint = null;
			PathStep currentPoint = null;
			while (pointsLeftToVisit.Count > 0 && startPoint == null)
			{
				currentPoint = pointsLeftToVisit[0];
				pointsLeftToVisit.RemoveAt(0);
				visitedPoints.Add(currentPoint);
				newPoints = GetNewMoveableCoordinatesFromPoint(currentPoint, visitedPoints);
				foreach (PathStep point in newPoints)
				{
					pointsLeftToVisit.Add(point);
					if (point.Node == startNode)
					{
						startPoint = point;
						break;
					}
				}
			}

			if (startPoint == null)
			{ return null; } // Then we could not find a path!

			List<PathfindingNode> path = new List<PathfindingNode>();
			PathStep pathStep = startPoint;
			while (pathStep != null)
			{
				path.Add(pathStep.Node);
				pathStep = pathStep.Ancestor;
			}

			DateTime endTime = DateTime.Now;
			TimeSpan duration = endTime.Subtract(startTime);
			System.Diagnostics.Debug.WriteLine("PathGraph DoFindRoute duration: "+duration.TotalMilliseconds+"ms");

			return path;
		}

		private List<PathStep> GetNewMoveableCoordinatesFromPoint(PathStep parentPoint, List<PathStep> visitedPoints)
		{
			List<PathStep> moves = new List<PathStep>();

			foreach (NodeConnection connection in _connections)
			{
				if (connection.ContainsNode(parentPoint.Node))
				{
					// Make sure we have not visited the OTHER node yet.
					PathfindingNode otherNode = connection.GetOppositeNode(parentPoint.Node);
					PathStep existingPathStep = null;
					foreach (PathStep point in visitedPoints)
					{
						if (point.Node == otherNode)
						{ existingPathStep = point; }
					}

					double totalCost = parentPoint.Cost + connection.Cost;
					if (existingPathStep != null)
					{
						if (existingPathStep.Cost > totalCost)
						{
							existingPathStep.Cost = totalCost;
							existingPathStep.Ancestor = parentPoint;
						}
					}
					else
					{ moves.Add(new PathStep() { Node = otherNode, Cost = totalCost, Ancestor = parentPoint }); }
				}
			}

			return moves;
		}

		public PathfindingNode FindNode(List<NodeConnection> connections, String nodeIdentifier)
		{
			PathfindingNode foundNode = null;
			foreach (NodeConnection connection in connections)
			{
				foundNode = connection.FindNode(nodeIdentifier);
				if (foundNode != null)
				{ break; }
			}
			return foundNode;
		}

		public Boolean ConnectionsHasNode(List<NodeConnection> connections, PathfindingNode node)
		{
			foreach (NodeConnection connection in connections)
			{
				if (connection.ContainsNode(node))
				{ return true; }
			}
			return false;
		}

		public Boolean ConnectionsHasNode(List<NodeConnection> connections, String nodeIdentifier)
		{
			foreach (NodeConnection connection in connections)
			{
				if (connection.FindNode(nodeIdentifier) != null)
				{ return true; }
			}
			return false;
		}

		public List<NodeConnection> GetConnectionsForNode(List<NodeConnection> connections, PathfindingNode node)
		{
			List<NodeConnection> matchingConnections = new List<NodeConnection>();
			foreach (NodeConnection connection in connections)
			{
				if (connection.ContainsNode(node))
				{ matchingConnections.Add(connection); }
			}
			return matchingConnections;
		}
	}
}