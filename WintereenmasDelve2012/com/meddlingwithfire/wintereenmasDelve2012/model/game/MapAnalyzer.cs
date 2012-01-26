using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.game.quests.maps;
using WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.model.game.quests.maps;
using WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.model;
using WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.model.game.quests.maps.tileActions;
using WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.model.game;
using PathFinding;

namespace WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.game
{
	public class MapAnalyzer
	{
		/// <summary>
		/// The state of the map.
		/// </summary>
		private QuestMap _questMap;

		/// <summary>
		/// The positions of all of the Avatars.
		/// </summary>
		private Dictionary<Avatar, MapTile> _avatarTiles;

		private List<Point> _heroFactionVisitedLocations;

		public MapAnalyzer(QuestMap map, Dictionary<Avatar, MapTile> avatarTiles):base()
		{
			_questMap = map;
			_avatarTiles = avatarTiles;

			_heroFactionVisitedLocations = new List<Point>();
		}

		public void UpdateVisitedLocations(Avatar avatar, Point newLocation)
		{
			if (avatar.Faction == Faction.Heroes)
			{
				if (!DoesListContainLocation(_heroFactionVisitedLocations, newLocation.X, newLocation.Y))
				{ _heroFactionVisitedLocations.Add(newLocation); }
			}
		}

		public List<AbstractTileAction> GetActionsForObserverLocation(Avatar observer)
		{
			Point observerLocation = _questMap.GetMapTileLocation(_avatarTiles[observer]);

			//// Get a list of all actionable map tiles
			PointList interestingLocations = _questMap.GetActionableMapPoints(observer.Faction);

			// Filter out interesting locations that are not in the visible set
			PointList observerLocations = new PointList();
			observerLocations.Add(observerLocation);
			PointList visibleInterestingLocations = interestingLocations.Intersects(observerLocations);

			List<AbstractTileAction> actions = new List<AbstractTileAction>();
			for (int i = 0; i < visibleInterestingLocations.Count; i++)
			{
				List<AbstractTileAction> actionsAtInterestingLocation = _questMap.GetActionsForPoint(observerLocation);
				foreach (AbstractTileAction actionAtInterestingLocation in actionsAtInterestingLocation)
				{ actions.Add(actionAtInterestingLocation); }
			}

			return actions;
		}

		public List<LocationOfInterest> GetInterestingLocations(Avatar observer)
		{
			List<LocationOfInterest> locationsOfInterest = new List<LocationOfInterest>();

			Point observerLocation = _questMap.GetMapTileLocation(_avatarTiles[observer]);
			PointList visibleLocations = _questMap.GetPointsWithinLineOfSightOf(observerLocation);

			//// Get a list of all actionable map tiles
			PointList interestingLocations = _questMap.GetActionableMapPoints(observer.Faction);
			
			// Filter out interesting locations that are not in the visible set
			PointList visibleInterestingLocations = interestingLocations.Intersects(visibleLocations);

			// Create paths to each of these points
			MapTile observerMapTile = _avatarTiles[observer];
			PathfindingNode observerNode = _questMap.GetPathfindingNodeForTile(observerMapTile);
			foreach (Point point in visibleInterestingLocations)
			{
				PathfindingNode pointNode = _questMap.GetPathfindingNodeForLocation(point.X, point.Y);
				List<PathfindingNode> path = _questMap.PathfindingGraph.FindRoute(observerNode, pointNode);
				if (path != null)
				{
					List<Point> pathSteps = new List<Point>();
					for (int i = 1; i < path.Count; i++)
					{ pathSteps.Add(_questMap.GetPointForPathfindingNode(path[i])); }

					LocationOfInterest interest = new LocationOfInterest(pathSteps);
					locationsOfInterest.Add(interest);
				}
			}

			return locationsOfInterest;
		}

		public List<LocationOfInterest> GetInterestingLocationsCheating(Avatar observer)
		{
			List<LocationOfInterest> locationsOfInterest = new List<LocationOfInterest>();

			Point observerLocation = _questMap.GetMapTileLocation(_avatarTiles[observer]);
			PointList visibleLocations = _questMap.GetPointsWithinLineOfSightOf(observerLocation);

			//// Get a list of all actionable map tiles
			PointList interestingLocations = _questMap.GetActionableMapPoints(observer.Faction);

			// Create paths to each of these points
			MapTile observerMapTile = _avatarTiles[observer];
			PathfindingNode observerNode = _questMap.GetPathfindingNodeForTile(observerMapTile);
			foreach (Point point in interestingLocations)
			{
				PathfindingNode pointNode = _questMap.GetPathfindingNodeForLocation(point.X, point.Y);
				List<PathfindingNode> path = _questMap.PathfindingGraph.FindRoute(observerNode, pointNode);
				if (path != null)
				{
					List<Point> pathSteps = new List<Point>();
					for (int i = 1; i < path.Count; i++)
					{ pathSteps.Add(_questMap.GetPointForPathfindingNode(path[i])); }

					LocationOfInterest interest = new LocationOfInterest(pathSteps);
					locationsOfInterest.Add(interest);
					return locationsOfInterest;
				}
			}

			return locationsOfInterest;
		}

		public PointList GetAdjacentUnvisitedLocations(Avatar observer)
		{
			Point observerLocation = _questMap.GetMapTileLocation(_avatarTiles[observer]);
			PointList points = _questMap.GetAdjacentUnwalkedTiles(observerLocation, observer.Faction);

			// Favor points in the direction of the Avatars' movement vector
			Point desiredPoint = new Point(
				observerLocation.X + observer.movementVector.X,
				observerLocation.Y + observer.movementVector.Y
			);

			if (!points.ContainsLocation(desiredPoint.X, desiredPoint.Y))
			{ return points; }

			PointList match = new PointList() { desiredPoint };
			points = points.Intersects(match);

			return points;
		}

		private Boolean DoesListContainLocation(List<Point> list, int locationX, int locationY)
		{
			Boolean found = false;
			foreach (Point point in list)
			{
				if (point.X == locationX && point.Y == locationY)
				{
					found = true;
					break;
				}
			}
			return found;
		}
	}
}
