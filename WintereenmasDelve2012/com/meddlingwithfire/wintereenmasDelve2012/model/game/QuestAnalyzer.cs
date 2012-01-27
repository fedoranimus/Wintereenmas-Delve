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
using WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.game.quests;

namespace WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.game
{
	public class QuestAnalyzer
	{
		/// <summary>
		/// The state of the quest.
		/// </summary>
		private AbstractQuest _quest;

		public QuestAnalyzer(AbstractQuest quest):base()
		{
			_quest = quest;
		}

		public List<AbstractTileAction> GetActionsAtObserverLocation(Avatar observer)
		{
			MapTile observerTile = _quest.GetAvatarMapTile(observer);
			Point observerLocation = _quest.Map.GetMapTileLocation(observerTile);

			//// Get a list of all actionable map tiles
			PointList interestingLocations = _quest.Map.GetActionableMapPoints(observer.Faction);

			// Filter out interesting locations that are not in the visible set
			PointList observerLocations = new PointList();
			observerLocations.Add(observerLocation);
			PointList visibleInterestingLocations = interestingLocations.Intersects(observerLocations);

			List<AbstractTileAction> actions = new List<AbstractTileAction>();
			for (int i = 0; i < visibleInterestingLocations.Count; i++)
			{
				List<AbstractTileAction> actionsAtInterestingLocation = _quest.Map.GetActionsForPoint(observerLocation);
				foreach (AbstractTileAction actionAtInterestingLocation in actionsAtInterestingLocation)
				{ actions.Add(actionAtInterestingLocation); }
			}

			return actions;
		}

		public List<LocationOfInterest> GetInterestingLocations(Avatar observer)
		{
			List<LocationOfInterest> locationsOfInterest = new List<LocationOfInterest>();

			MapTile observerTile = _quest.GetAvatarMapTile(observer);
			Point observerLocation = _quest.Map.GetMapTileLocation(observerTile);
			PointList visibleLocations = _quest.Map.GetPointsWithinLineOfSightOf(observerLocation);

			//// Get a list of all actionable map tiles
			PointList interestingLocations = _quest.Map.GetActionableMapPoints(observer.Faction);
			
			// Filter out interesting locations that are not in the visible set
			PointList visibleInterestingLocations = interestingLocations.Intersects(visibleLocations);

			// Create paths to each of these points
			PathfindingNode observerNode = _quest.Map.GetPathfindingNodeForTile(observerTile);
			foreach (Point point in visibleInterestingLocations)
			{
				PathfindingNode pointNode = _quest.Map.GetPathfindingNodeForLocation(point.X, point.Y);
				List<PathfindingNode> path = _quest.Map.PathfindingGraph.FindRoute(observerNode, pointNode);
				if (path != null)
				{
					PointList pathSteps = new PointList();
					for (int i = 1; i < path.Count; i++)
					{ pathSteps.Add(_quest.Map.GetPointForPathfindingNode(path[i])); }

					LocationOfInterest interest = new LocationOfInterest(pathSteps);
					locationsOfInterest.Add(interest);
				}
			}

			return locationsOfInterest;
		}

		public List<LocationOfInterest> GetInterestingLocationsCheating(Avatar observer)
		{
			List<LocationOfInterest> locationsOfInterest = new List<LocationOfInterest>();

			MapTile observerTile = _quest.GetAvatarMapTile(observer);
			Point observerLocation = _quest.Map.GetMapTileLocation(observerTile);
			PointList visibleLocations = _quest.Map.GetPointsWithinLineOfSightOf(observerLocation);

			//// Get a list of all actionable map tiles
			PointList interestingLocations = _quest.Map.GetActionableMapPoints(observer.Faction);

			// Find the closest location by straight-line distance.  It was too expensive to calculate Pathfinding distance.
			PathfindingNode observerNode = _quest.Map.GetPathfindingNodeForTile(observerTile);
			Point closestDistanceInterestingLocation = null;
			double closestDistance = float.MaxValue;
			foreach (Point point in interestingLocations)
			{
				double distance = Math.Sqrt(Math.Pow(observerLocation.X - point.X, 2) + Math.Pow(observerLocation.Y - point.Y, 2));
				if (distance < closestDistance)
				{
					closestDistanceInterestingLocation = point;
					closestDistance = distance;
				}
			}

			if (closestDistanceInterestingLocation != null)
			{
				PathfindingNode pointNode = _quest.Map.GetPathfindingNodeForLocation(closestDistanceInterestingLocation.X, closestDistanceInterestingLocation.Y);
				List<PathfindingNode> path = _quest.Map.PathfindingGraph.FindRoute(observerNode, pointNode);
				if (path != null)
				{
					PointList pathSteps = new PointList();
					for (int i = 1; i < path.Count; i++)
					{ pathSteps.Add(_quest.Map.GetPointForPathfindingNode(path[i])); }

					LocationOfInterest interest = new LocationOfInterest(pathSteps);
					locationsOfInterest.Add(interest);
					return locationsOfInterest;
				}
			}

			return locationsOfInterest;
		}

		public PointList GetAdjacentUnvisitedLocations(Avatar observer)
		{
			MapTile observerTile = _quest.GetAvatarMapTile(observer);
			Point observerLocation = _quest.Map.GetMapTileLocation(observerTile);
			PointList points = _quest.Map.GetAdjacentUnwalkedTiles(observerLocation, observer.Faction);

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

		public List<Avatar> GetAdjacentEnemies(Avatar observer, Boolean includeDiagonalTiles)
		{
			List<Avatar> adjacentEnemies = new List<Avatar>();

			MapTile observerMapTile = _quest.GetAvatarMapTile(observer);
			Point observerLocation = _quest.Map.GetMapTileLocation(observerMapTile);

			double minimumDistance = includeDiagonalTiles ? Math.Sqrt(2) : 1;
			List<Avatar> allEnemies = _quest.GetEnemiesOfAvatar(observer);
			foreach (Avatar enemy in allEnemies)
			{
				MapTile enemyMapTile = _quest.GetAvatarMapTile(enemy);
				Point enemyPoint = _quest.Map.GetMapTileLocation(enemyMapTile);
				double distance = Math.Sqrt(Math.Pow(enemyPoint.X - observerLocation.X, 2) + Math.Pow(enemyPoint.Y - observerLocation.Y, 2));
				if (distance <= minimumDistance)
				{ adjacentEnemies.Add(enemy); }
			}

			return adjacentEnemies;
		}

		public List<Avatar> GetVisibleEnemies(Avatar observer)
		{
			List<Avatar> visibleEnemies = new List<Avatar>();

			MapTile observerMapTile = _quest.GetAvatarMapTile(observer);
			Point observerLocation = _quest.Map.GetMapTileLocation(observerMapTile);
			PointList visibleLocations = _quest.Map.GetPointsWithinLineOfSightOf(observerLocation);

			List<Avatar> allEnemies = _quest.GetEnemiesOfAvatar(observer);
			foreach (Avatar enemy in allEnemies)
			{
				MapTile enemyMapTile = _quest.GetAvatarMapTile(enemy);
				Point enemyPoint = _quest.Map.GetMapTileLocation(enemyMapTile);
				if (visibleLocations.ContainsLocation(enemyPoint.X, enemyPoint.Y))
				{ visibleEnemies.Add(enemy); }
			}
			return visibleEnemies;
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
