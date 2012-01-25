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

		public List<LocationOfInterest> GetInterestingLocations(Avatar observer)
		{
			List<LocationOfInterest> locationsOfInterest = new List<LocationOfInterest>();

			Point observerLocation = _questMap.GetMapTileLocation(_avatarTiles[observer]);
			PointList visibleLocations = _questMap.GetPointsWithinLineOfSightOf(observerLocation);

			//// Get a list of all actionable map tiles
			PointList interestingLocations = _questMap.GetActionableMapPoints(observer.Faction);
			
			// TODO: Filter out interesting locations that are not in the visible set
			PointList visibleInterestingLocations = interestingLocations.Intersects(visibleLocations);

			// Create paths to each of these points
			MapTile observerMapTile = _avatarTiles[observer];
			PathfindingNode observerNode = _questMap.GetPathfindingNodeForTile(observerMapTile);
			foreach (Point point in visibleInterestingLocations)
			{
				PathfindingNode pointNode = _questMap.GetPathfindingNodeForLocation(point.X, point.Y);
				List<PathfindingNode> path = _questMap.PathfindingGraph.FindRoute(observerNode, pointNode);
			}

			return null;
		}

		public List<Point> GetUnvisitedMapCoordinatesOrderedByProximity(Avatar observer, int maximumSteps)
		{
			List<Point> unvisitedCoordinates = new List<Point>();

			Point observerLocation = _questMap.GetMapTileLocation(_avatarTiles[observer]);
			unvisitedCoordinates.Add(new Point(observerLocation.X + 1, observerLocation.Y));

			return unvisitedCoordinates;
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
