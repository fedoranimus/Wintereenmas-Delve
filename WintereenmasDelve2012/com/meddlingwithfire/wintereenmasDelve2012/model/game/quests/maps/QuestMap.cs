using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PathFinding;
using WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.model.game.quests.maps;
using WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.model.game.quests.maps.tiles;
using WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.model;
using WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.model.game.quests.maps.tileActions;

namespace WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.game.quests.maps
{
	public class QuestMap
	{
		private const int MAP_WIDTH_IN_TILES = 26;
		private const int MAP_HEIGHT_IN_TILES = 19;

		public PathGraph PathfindingGraph;

		public List<Point> HeroStartingLocations;

		/// <summary>
		/// Keys are the zero-based index of the coordinates "0x0" represents the origin, "25x18" represents the lower right edge.
		/// </summary>
		private Dictionary<String, MapTileList> _questTiles;

		/// <summary>
		/// Enables us to quickly pull the location out for the map tile.
		/// </summary>
		private Dictionary<MapTile, Point> _mapTileLocations;

		private Dictionary<PathfindingNode, Point> _pathfindingPoints;

		private Dictionary<Faction, PointList> _tilesWalkedOn;

		public QuestMap(Point staircaseOrigin)
			: base()
		{
			_questTiles = new Dictionary<string, MapTileList>();
			_mapTileLocations = new Dictionary<MapTile, Point>();
			_pathfindingPoints = new Dictionary<PathfindingNode, Point>();
			_tilesWalkedOn = new Dictionary<Faction, PointList>();

			HeroStartingLocations = new List<Point>();
			HeroStartingLocations.Add(new Point(staircaseOrigin.X, staircaseOrigin.Y));
			HeroStartingLocations.Add(new Point(staircaseOrigin.X + 1, staircaseOrigin.Y));
			HeroStartingLocations.Add(new Point(staircaseOrigin.X, staircaseOrigin.Y + 1));
			HeroStartingLocations.Add(new Point(staircaseOrigin.X + 1, staircaseOrigin.Y + 1));

			CreateInitialEmptyMap();

			// Add in the staircase
			AddMapTileSetTo(staircaseOrigin, new StaircaseTileSet());

			CreateInitialWalls();
		}

		public void SetFactionWalkedOn(Faction faction, Point point)
		{
			PointList factionWalkedOn = null;
			if (!_tilesWalkedOn.ContainsKey(faction))
			{
				factionWalkedOn = new PointList();
				_tilesWalkedOn[faction] = factionWalkedOn;
			}
			else
			{ factionWalkedOn = _tilesWalkedOn[faction]; }

			if (!factionWalkedOn.ContainsLocation(point.X, point.Y))
			{ factionWalkedOn.Add(point); }
		}

		public PointList GetAdjacentUnwalkedTiles(Point origin, Faction faction)
		{
			PointList list = GetVisibleAdjacentPoints(origin, new PointList());

			PointList factionWalkedOnList = new PointList();
			if (_tilesWalkedOn.ContainsKey(faction))
			{ factionWalkedOnList = _tilesWalkedOn[faction]; }
			
			for(int i=list.Count-1; i >= 0; i--)
			{
				Point point = list[i];
				if (factionWalkedOnList.ContainsLocation(point.X, point.Y))
				{ list.RemoveAt(i); }
			}

			return list;
		}

		public void RemoveMapTile(MapTile removeTile)
		{
			// Look through all the tiles for the specific map tile instance
			for (int y = 0; y < MAP_HEIGHT_IN_TILES; y++)
			{
				for (int x = 0; x < MAP_WIDTH_IN_TILES; x++)
				{
					MapTileList tiles = GetMapTilesForLocation(x, y);
					for(int i=tiles.Count-1; i >= 0; i--)
					{
						MapTile tile = tiles[i];
						if (tile == removeTile)
						{
							tiles.RemoveAt(i);
							_mapTileLocations.Remove(tile);
						}
					}
				}
			}
		}

		public int TilesWide
		{
			get { return MAP_WIDTH_IN_TILES; }
		}

		public int TilesTall
		{
			get { return MAP_HEIGHT_IN_TILES; }
		}

		/// <summary>
		/// Moves the map tile from one location to another.  Primarily used to move the Hero & monster tiles around the board.
		/// </summary>
		/// <param name="tile"></param>
		/// <param name="toPoint"></param>
		public void MoveMapTile(MapTile tile, Point toPoint)
		{
			Point tileLocation = _mapTileLocations[tile];
			MapTileList locationTiles = GetMapTilesForPoint(tileLocation);
			if (!locationTiles.Contains(tile))
			{ throw new Exception("QuestMap MoveMapTile(tile, toPoint) called, but no tile was found!"); }
			else
			{
				// Remove it from its' old location
				locationTiles.Remove(tile);
				
				// And add it to the new location
				MapTileList newLocationTiles = GetMapTilesForPoint(toPoint);
				newLocationTiles.Add(tile);
				_mapTileLocations[tile] = toPoint;
			}
		}

		public Point GetMapTileLocation(MapTile mapTile)
		{
			for (int y = 0; y < MAP_HEIGHT_IN_TILES; y++)
			{
				for (int x = 0; x < MAP_WIDTH_IN_TILES; x++)
				{
					MapTileList tiles = GetMapTilesForLocation(x, y);
					if (tiles.Contains(mapTile))
					{ return new Point(x, y); }
				}
			}
			return null;
		}

		public void AddMapTileSetTo(Point origin, MapTileSet tileSet)
		{
			List<MapTile> tiles = tileSet.MapTiles;
			for (int i = 0; i < tiles.Count; i++)
			{
				MapTile tile = tiles[i];
				Point relativeLocation = tileSet.GetRelativeLocationFor(tile);
				MapTileList locationTiles = GetMapTilesForLocation(origin.X + relativeLocation.X, origin.Y + relativeLocation.Y);
				locationTiles.Add(tile);
				_mapTileLocations[tile] = new Point(origin.X + relativeLocation.X, origin.Y + relativeLocation.Y);
			}
		}

		public MapTileList GetMapTilesForPoint(Point point)
		{ return GetMapTilesForLocation(point.X, point.Y); }

		public MapTileList GetMapTilesForLocation(int x, int y)
		{
			String key = GetKeyForLocation(x, y);
			MapTileList mapTiles = null;
			if (!_questTiles.ContainsKey(key))
			{
				mapTiles = new MapTileList();
				_questTiles[key] = mapTiles;
			}
			else
			{ mapTiles = _questTiles[key]; }
			
			return mapTiles;
		}

		private void CreateInitialEmptyMap()
		{
			// Create the base flooring and walls
			FillAreaWithTile(new Point(0, 0), MAP_WIDTH_IN_TILES, MAP_HEIGHT_IN_TILES, new FloorEmptyTile());
		}

		private void CreateInitialWalls()
		{
			WallInArea(new Point(0, 0), MAP_WIDTH_IN_TILES, MAP_HEIGHT_IN_TILES);

			// Create the room borders
			WallInArea(new Point(1, 1), 4, 3); // Room A
			WallInArea(new Point(5, 1), 4, 3); // Room B
			WallInArea(new Point(9, 1), 3, 5); // Room C
			WallInArea(new Point(1, 4), 4, 5); // Room D
			WallInArea(new Point(5, 4), 4, 5); // Room E

			WallInArea(new Point(1, 10), 4, 4); // Room F
			WallInArea(new Point(5, 10), 2, 3); // Room G
			WallInArea(new Point(7, 10), 2, 3); // Room H
			WallInArea(new Point(1, 14), 4, 4); // Room I
			WallInArea(new Point(5, 13), 4, 5); // Room J
			WallInArea(new Point(9, 13), 3, 5); // Room K

			WallInArea(new Point(14, 1), 3, 5); // Room L
			WallInArea(new Point(17, 1), 4, 4); // Room M
			WallInArea(new Point(21, 1), 4, 4); // Room N
			WallInArea(new Point(17, 5), 4, 4); // Room O
			WallInArea(new Point(21, 5), 4, 4); // Room P

			//WallInArea(new Point(17, 10), 4, 4); // Room Q
			AddHorizontalWall(17, 10, 4);
			AddVerticalWall(17, 10, 3);

			WallInArea(new Point(21, 10), 4, 4); // Room R
			WallInArea(new Point(14, 13), 4, 5); // Room S
			WallInArea(new Point(18, 14), 3, 4); // Room T
			WallInArea(new Point(21, 14), 4, 4); // Room U

			WallInArea(new Point(10, 7), 6, 5); // Room V
		}

		public void AddTile(Point origin, MapTile tile)
		{ AddTile(origin.X, origin.Y, tile); }

		public void AddTile(int originX, int originY, MapTile tile)
		{
			List<MapTile> tiles = GetMapTilesForLocation(originX, originY);
			tiles.Add(tile);
			_mapTileLocations[tile] = new Point(originX, originY);
		}

		public void SetBaseTile(Point origin, MapTile tile)
		{ SetBaseTile(origin.X, origin.Y, tile); }

		public void SetBaseTile(int originX, int originY, MapTile tile)
		{
			List<MapTile> tiles = GetMapTilesForLocation(originX, originY);
			if (tiles.Count > 0)
			{
				_mapTileLocations.Remove(tiles[0]);
				tiles[0] = tile;
			}
			else
			{ tiles.Add(tile); }
			_mapTileLocations[tile] = new Point(originX, originY);
		}

		public void SetBaseAreaWithTile(Point origin, int tilesWide, int tilesTall, MapTile tileTemplate)
		{
			Rectangle area = new Rectangle(origin.X, origin.Y, origin.X + tilesWide, origin.Y + tilesTall);
			for (int y = area.Y; y < area.Height; y++)
			{
				for (int x = area.X; x < area.Width; x++)
				{
					MapTileList mapTiles = GetMapTilesForLocation(x, y);
					MapTile tileToAdd = (MapTile)tileTemplate.Clone();
					if (mapTiles.Count > 0)
					{
						_mapTileLocations.Remove(mapTiles[0]);
						mapTiles[0] = tileToAdd;
					}
					else
					{ mapTiles.Add(tileToAdd); }
					
					_mapTileLocations[tileToAdd] = new Point(x, y);
				}
			}
		}

		public void FillAreaWithTile(Point origin, int tilesWide, int tilesTall, MapTile tileTemplate)
		{
			Rectangle area = new Rectangle(origin.X, origin.Y, origin.X + tilesWide, origin.Y + tilesTall);
			for (int y = area.Y; y < area.Height; y++)
			{
				for (int x = area.X; x < area.Width; x++)
				{
					MapTileList mapTiles = GetMapTilesForLocation(x, y);
					MapTile tileToAdd = (MapTile)tileTemplate.Clone();
					mapTiles.Add(tileToAdd);
					_mapTileLocations[tileToAdd] = new Point(x, y);
				}
			}
		}

		public void WallInArea(Point origin, int tilesWide, int tilesTall)
		{ WallInArea(origin.X, origin.Y, tilesWide, tilesTall); }

		public void AddHorizontalWall(int originX, int originY, int wallWidth)
		{
			for (int x = originX; x < originX + wallWidth; x++)
			{
				AddTileIfMatchNotAlreadyThere(x, originY, new WallHorizontalTopTile());
				if (originY > 0)
				{ AddTileIfMatchNotAlreadyThere(x, originY - 1, new WallHorizontalBottomTile()); }
			}
		}

		public void AddVerticalWall(int originX, int originY, int wallHeight)
		{
			for (int y = originY; y < originY + wallHeight; y++)
			{
				AddTileIfMatchNotAlreadyThere(originX, y, new WallVerticalLeftTile());
				if (originX > 0)
				{ AddTileIfMatchNotAlreadyThere(originX-1, y, new WallVerticalRightTile()); }
			}
		}

		public void WallInArea(int originX, int originY, int tilesWide, int tilesTall)
		{
			Rectangle area = new Rectangle(originX, originY, originX + tilesWide, originY + tilesTall);
			for (int y = area.Y; y < area.Height; y++)
			{
				for (int x = area.X; x < area.Width; x++)
				{
					if (x == area.X) // If we're on the left edge, add left wall
					{
						AddTileIfMatchNotAlreadyThere(x, y, new WallVerticalLeftTile());
						if (x > 0)
						{ AddTileIfMatchNotAlreadyThere(x-1, y, new WallVerticalRightTile()); }
					}
					if (x == area.Width-1)
					{
						AddTileIfMatchNotAlreadyThere(x, y, new WallVerticalRightTile());
						if (x < MAP_WIDTH_IN_TILES-1)
						{ AddTileIfMatchNotAlreadyThere(x+1, y, new WallVerticalLeftTile()); }
					}
					if (y == area.Y)
					{
						AddTileIfMatchNotAlreadyThere(x, y, new WallHorizontalTopTile());
						if (y > 0)
						{ AddTileIfMatchNotAlreadyThere(x, y-1, new WallHorizontalBottomTile()); }
					}
					if (y == area.Height-1)
					{
						AddTileIfMatchNotAlreadyThere(x, y, new WallHorizontalBottomTile()); 
						if (y < MAP_HEIGHT_IN_TILES-1)
						{ AddTileIfMatchNotAlreadyThere(x, y+1, new WallHorizontalTopTile()); }
					}
				}
			}
		}

		public void RemoveBarriersBetween(Point pointA, Point pointB)
		{ RemoveBarriersBetween(pointA.X, pointA.Y, pointB.X, pointB.Y); }

		public void RemoveBarriersBetween(int aX, int aY, int bX, int bY)
		{
			double distance = Math.Sqrt(Math.Pow(aX - bX, 2) + Math.Pow(aY - bY, 2));
			if (distance != 1)
			{ throw new ArgumentException("Can only remove barriers between two adjacent tiles!"); }
			
			// Get the tile sets=
			MapTileList aTiles = GetMapTilesForLocation(aX, aY);
			MapTileList bTiles = GetMapTilesForLocation(bX, bY);

			// Remove any blocking connections that are between those two tiles.  This could use some optimization
			if (aY != bY) // It's vertical
			{
				if (aY < bY) // A is on top, B is on bottom
				{
					for (int i = aTiles.Count - 1; i >= 0; i--)
					{
						MapTile tile = aTiles[i];
						if (tile.BlocksSouth)
						{
							aTiles.RemoveAt(i);
							_mapTileLocations.Remove(tile);
						}
					}
					for (int i = bTiles.Count - 1; i >= 0; i--)
					{
						MapTile tile = bTiles[i];
						if (tile.BlocksNorth)
						{
							bTiles.RemoveAt(i);
							_mapTileLocations.Remove(tile);
						}
					}
				}
				else // B is on top, A is on bottom
				{
					for (int i = aTiles.Count - 1; i >= 0; i--)
					{
						MapTile tile = aTiles[i];
						if (tile.BlocksNorth)
						{
							aTiles.RemoveAt(i);
							_mapTileLocations.Remove(tile);
						}
					}
					for (int i = bTiles.Count - 1; i >= 0; i--)
					{
						MapTile tile = bTiles[i];
						if (tile.BlocksSouth)
						{
							bTiles.RemoveAt(i);
							_mapTileLocations.Remove(tile);
						}
					}
				}
			}
			else // It's horizontal
			{
				if (aX < bX) // A is on the left, B is on the right
				{
					for (int i = aTiles.Count - 1; i >= 0; i--)
					{
						MapTile tile = aTiles[i];
						if (tile.BlocksEast)
						{
							aTiles.RemoveAt(i);
							_mapTileLocations.Remove(tile);
						}
					}
					for (int i = bTiles.Count - 1; i >= 0; i--)
					{
						MapTile tile = bTiles[i];
						if (tile.BlocksWest)
						{
							bTiles.RemoveAt(i);
							_mapTileLocations.Remove(tile);
						}
					}
				}
				else // B is on the left, A is on the right
				{
					for (int i = aTiles.Count - 1; i >= 0; i--)
					{
						MapTile tile = aTiles[i];
						if (tile.BlocksWest)
						{
							aTiles.RemoveAt(i);
							_mapTileLocations.Remove(tile);
						}
					}
					for (int i = bTiles.Count - 1; i >= 0; i--)
					{
						MapTile tile = bTiles[i];
						if (tile.BlocksEast)
						{
							bTiles.RemoveAt(i);
							_mapTileLocations.Remove(tile);
						}
					}
				}
			}
		}

		public void AddDoorBetween(Point pointA, Point pointB)
		{ AddDoorBetween(pointA.X, pointA.Y, pointB.X, pointB.Y); }

		public void AddDoorBetween(int aX, int aY, int bX, int bY)
		{
			double distance = Math.Sqrt(Math.Pow(aX - bX, 2) + Math.Pow(aY - bY, 2));
			if (distance != 1)
			{ throw new ArgumentException("Can only put doors between two adjacent tiles!"); }

			// Get the tile sets for A
			MapTileList aTiles = GetMapTilesForLocation(aX, aY);
			MapTileList bTiles = GetMapTilesForLocation(bX, bY);

			// Remove any blocking connections that are between those two tiles.
			RemoveBarriersBetween(aX, aY, bX, bY);

			if (aY != bY) // It's a vertical door
			{
				if (aY < bY) // A is on top, B is on bottom
				{ AddMapTileSetTo(new Point(aX, aY), new DoorHorizontalTileSet()); }
				else // B is on top, A is on bottom
				{ AddMapTileSetTo(new Point(bX, bY), new DoorHorizontalTileSet()); }
			}
			else // It's a horizontal door
			{
				if (aX < bX) // A is on the left, B is on the right
				{ AddMapTileSetTo(new Point(aX, aY), new DoorVerticalTileSet()); }
				else // B is on the left, A is on the right
				{ AddMapTileSetTo(new Point(bX, bY), new DoorVerticalTileSet()); }
			}
		}

		public void AddTileIfMatchNotAlreadyThere(int x, int y, MapTile tileTemplate)
		{
			List<MapTile> mapTiles = GetMapTilesForLocation(x, y);

			// Check to see if the map tile already includes a left wall
			Boolean foundMatch = false;
			foreach (MapTile tile in mapTiles)
			{
				if (tile.Equals(tileTemplate))
				{
					foundMatch = true;
					break;
				}
			}

			if (!foundMatch)
			{
				mapTiles.Add(tileTemplate);
				_mapTileLocations[tileTemplate] = new Point(x, y);
			}
		}

		public Monster GenerateWanderingMonster()
		{ return null; }

		private String GetKeyForPoint(Point point)
		{ return GetKeyForLocation(point.X, point.Y); }

		private String GetKeyForLocation(int x, int y)
		{ return x.ToString() + "x" + y.ToString(); }

		public void CalculatePathfindingGraph()
		{
			PathfindingGraph = GeneratePathfindingGraph();
		}

		public PointList GetPointsWithinLineOfSightOf(Point origin)
		{
			PointList lineOfSight = new PointList();
			lineOfSight.Add(origin);

			// Start with the origin point, and check adjacent nodes that are "within line of sight". Probably need to modify the MapTile class to include a "blocks line of sight" property?
			PointList visitedPoints = new PointList();
			visitedPoints.Add(origin);
			PointList pointsLeftToVisit = GetVisibleAdjacentPoints(origin, visitedPoints);
			while (pointsLeftToVisit.Count > 0)
			{
				Point currentPoint = pointsLeftToVisit[0];
				pointsLeftToVisit.RemoveAt(0);
				visitedPoints.Add(currentPoint);
				if (CanSeePointFromPoint(origin, currentPoint))
				{
					lineOfSight.Add(currentPoint);

					// Check each adjacent node.  If they are within line of sight, add their adjacent nodes.  
					// Always check the line of sight from the origin, however.  Use the GetStraightPathBetweenTwoPoints method to generate the line.
					// Given the line, 
					pointsLeftToVisit.AddDistinctPointsOnly(GetVisibleAdjacentPoints(currentPoint, visitedPoints));
				}
			}

			return lineOfSight;
		}

		public Boolean CanSeePointFromPoint(Point origin, Point destination)
		{
			System.Diagnostics.Debug.WriteLine("QuestMap CanSeePointFromPoint([" + origin.X + "," + origin.Y + "], [" + destination.X + ", " + destination.Y + "])");
			List<Point> points = GetStraightPathBetweenTwoPoints(origin, destination);

			if (origin.X == 1 && origin.Y == 14 && destination.X == 0 && destination.Y == 17)
			{
				System.Diagnostics.Debug.WriteLine("here!");
			}

			Point previousPoint = null;
			for (int i = 0; i < points.Count; i++)
			{
				Point currentPoint = points[i];
				if (previousPoint != null)
				{
					if (DoAdjacentPointsObstructLineOfSight(previousPoint, currentPoint))
					{ return false; }
				}
				previousPoint = currentPoint;
			}

			return true;
		}

		public Boolean DoAdjacentPointsObstructLineOfSight(Point pointA, Point pointB)
		{
			System.Diagnostics.Debug.WriteLine("QuestMap DoAdjacentPointsObstructLineOfSight([" + pointA.X + "," + pointA.Y + "], [" + pointB.X + "," + pointB.Y + "])");
			double distance = Math.Sqrt(Math.Pow(pointA.X - pointB.X, 2) + Math.Pow(pointA.Y - pointB.Y, 2));
			if (distance > Math.Sqrt(2))
			{ throw new ArgumentException("QuestMap DoAdjacentPointsObstructLineOfSight(a, b) points were not adjacent!"); } // If the points are too far away, then we can't test.
			if (distance <= 0)
			{ throw new ArgumentException("QuestMap DoAdjacentPointsObstructLineOfSight(a, b) points were the same point!"); }

			MapTileList aTiles = GetMapTilesForPoint(pointA);
			MapTileList bTiles = GetMapTilesForPoint(pointB);
			
			if (pointA.Y > pointB.Y)
			{
				if (pointA.X > pointB.X)
				{
					if (aTiles.BlocksLineOfSight || (aTiles.BlocksLineOfSightNorth || aTiles.BlocksLineOfSightWest))
					{ return true; }
					if (bTiles.BlocksLineOfSight || (bTiles.BlocksLineOfSightSouth || bTiles.BlocksLineOfSightEast))
					{ return true; }
				}
				else if (pointA.X == pointB.X)
				{
					if (aTiles.BlocksLineOfSight || aTiles.BlocksLineOfSightNorth)
					{ return true; }
					if (bTiles.BlocksLineOfSight || bTiles.BlocksLineOfSightSouth)
					{ return true; }
				}
				else
				{
					if (aTiles.BlocksLineOfSight || (aTiles.BlocksLineOfSightNorth || aTiles.BlocksLineOfSightEast))
					{ return true; }
					if (bTiles.BlocksLineOfSight || (bTiles.BlocksLineOfSightSouth || bTiles.BlocksLineOfSightWest))
					{ return true; }
				}
			}
			if (pointA.Y < pointB.Y)
			{
				if (pointA.X > pointB.X)
				{
					if (aTiles.BlocksLineOfSight || (aTiles.BlocksLineOfSightSouth || aTiles.BlocksLineOfSightWest))
					{ return true; }
					if (bTiles.BlocksLineOfSight || (bTiles.BlocksLineOfSightNorth || bTiles.BlocksLineOfSightEast))
					{ return true; }
				}
				else if (pointA.X == pointB.X)
				{
					if (aTiles.BlocksLineOfSight || aTiles.BlocksLineOfSightSouth)
					{ return true; }
					if (bTiles.BlocksLineOfSight || bTiles.BlocksLineOfSightNorth)
					{ return true; }
				}
				else
				{
					if (aTiles.BlocksLineOfSight || (aTiles.BlocksLineOfSightSouth || aTiles.BlocksLineOfSightEast))
					{ return true; }
					if (bTiles.BlocksLineOfSight || (bTiles.BlocksLineOfSightNorth || bTiles.BlocksLineOfSightWest))
					{ return true; }
				}
			}
			else // y's are same
			{
				if (pointA.X > pointB.X)
				{
					if (aTiles.BlocksLineOfSight || aTiles.BlocksLineOfSightWest)
					{ return true; }
					if (bTiles.BlocksLineOfSight || bTiles.BlocksLineOfSightEast)
					{ return true; }
				}
				else
				{
					if (aTiles.BlocksLineOfSight || aTiles.BlocksLineOfSightEast)
					{ return true; }
					if (bTiles.BlocksLineOfSight || bTiles.BlocksLineOfSightWest)
					{ return true; }
				}
			}

			return false;
		}

		public PointList GetVisibleAdjacentPoints(Point origin, PointList skipPoints)
		{
			PointList adjacentPoints = new PointList();

			MapTileList originTiles = GetMapTilesForPoint(origin);
			Boolean originBlocksLineOfSightNorth = false;
			Boolean originBlocksLineOfSightEast = false;
			Boolean originBlocksLineOfSightSouth = false;
			Boolean originBlocksLineOfSightWest = false;
			if (originTiles.BlocksLineOfSight)
			{ return adjacentPoints; } // If the tile itself blocks all line of sight, then nothing is visible
			if (originTiles.BlocksLineOfSightNorth)
			{ originBlocksLineOfSightNorth = true; }
			if (originTiles.BlocksLineOfSightEast)
			{ originBlocksLineOfSightEast = true; }
			if (originTiles.BlocksLineOfSightSouth)
			{ originBlocksLineOfSightSouth = true; }
			if (originTiles.BlocksLineOfSightWest)
			{ originBlocksLineOfSightWest = true; }

			if (origin.X > 0 &&  !originBlocksLineOfSightWest && !skipPoints.ContainsLocation(origin.X - 1, origin.Y))
			{ adjacentPoints.Add(new Point(origin.X - 1, origin.Y)); }
			if (origin.X < MAP_WIDTH_IN_TILES - 1 && !originBlocksLineOfSightEast && !skipPoints.ContainsLocation(origin.X + 1, origin.Y))
			{ adjacentPoints.Add(new Point(origin.X + 1, origin.Y)); }
			if (origin.Y > 0 && !originBlocksLineOfSightNorth && !skipPoints.ContainsLocation(origin.X, origin.Y - 1))
			{ adjacentPoints.Add(new Point(origin.X, origin.Y - 1)); }
			if (origin.Y < MAP_HEIGHT_IN_TILES - 1 && !originBlocksLineOfSightSouth && !skipPoints.ContainsLocation(origin.X, origin.Y+1))
			{ adjacentPoints.Add(new Point(origin.X, origin.Y + 1)); }

			return adjacentPoints;
		}

		public PathfindingNode GetPathfindingNodeForTile(MapTile tile)
		{
			// Look up the location of the tile
			Point location = _mapTileLocations[tile];
			return GetPathfindingNodeForLocation(location.X, location.Y);
		}

		public PathfindingNode GetPathfindingNodeForLocation(int x, int y)
		{
			String identifier = GetKeyForLocation(x, y);
			return PathfindingGraph.FindNodeByIdentifier(identifier);
		}

		public Point GetPointForPathfindingNode(PathfindingNode node)
		{
			return  _pathfindingPoints[node];
		}

		/// <summary>
		/// This method should be called when all of the traversable locations are entered.  Note, do NOT place DOORs before calling
		/// this method.  Doors require actions to be traversable, so they would block the path.
		/// </summary>
		private PathGraph GeneratePathfindingGraph()
		{
			DateTime startTime = DateTime.Now;
			PathGraph pathfindingGraph = new PathGraph();
			//PathfindingNodes = new List<PathfindingNode>();
			//PathfindingConnections = new List<NodeConnection>();

			for (int y = 0; y < MAP_HEIGHT_IN_TILES; y++)
			{
				for (int x = 0; x < MAP_WIDTH_IN_TILES; x++)
				{
					String locationKey = GetKeyForLocation(x, y);
					MapTileList locationTiles = GetMapTilesForLocation(x, y);
					
					Boolean locationIsTraversable = true;
					Boolean isTraversableNorth = true;
					Boolean isTraversableEast = true;
					Boolean isTraversableSouth = true;
					Boolean isTraversableWest = true;
					foreach(MapTile tile in locationTiles)
					{
						// If we have already determined that all paths are blocked, then there's no sense looping any further.
						if (!locationIsTraversable || (!isTraversableNorth && !isTraversableEast && !isTraversableSouth && !isTraversableWest))
						{ break; }

						if (tile.BlocksMovement)
						{ locationIsTraversable = false; }
						if (tile.BlocksNorth)
						{ isTraversableNorth = false; }
						if (tile.BlocksEast)
						{ isTraversableEast = false; }
						if (tile.BlocksSouth)
						{ isTraversableSouth = false; }
						if (tile.BlocksWest)
						{ isTraversableWest = false; }
					}

					if (locationIsTraversable) // Only add a node if the location itself may be moved on
					{
						System.Diagnostics.Debug.WriteLine("Location " + locationKey + " is traversable!");

						PathfindingNode node = new PathfindingNode(locationKey);
						_pathfindingPoints[node] = new Point(x, y);
						pathfindingGraph.AddNode(node);

						// Look for connections in all directions: North, East, South, 
						List<MapTile> connectingTiles;
						Point connectingLocation = null;
						String connectingKey = null;
						PathfindingNode connectingNode = null;
						Boolean isConnectionTraversable = false;
						if (y > 0 && isTraversableNorth)
						{
							connectingLocation = new Point(x, y - 1);
							connectingKey = GetKeyForPoint(connectingLocation);
							connectingNode = pathfindingGraph.FindNodeByIdentifier(connectingKey);
							if (connectingNode != null && !pathfindingGraph.HasDirectConnectionBetween(node, connectingNode))
							{
								connectingTiles = GetMapTilesForPoint(connectingLocation);
								isConnectionTraversable = true;
								foreach (MapTile tile in connectingTiles)
								{
									if (tile.BlocksMovement || tile.BlocksSouth)
									{
										isConnectionTraversable = false;
										break;
									}
								}
								if (isConnectionTraversable) // If both connections are valid, then link them!
								{ pathfindingGraph.AddConnection(node, connectingNode, 1); }
							}
						}

						if (y < MAP_HEIGHT_IN_TILES - 1 && isTraversableSouth)
						{
							connectingLocation = new Point(x, y + 1);
							connectingKey = GetKeyForPoint(connectingLocation);
							connectingNode = pathfindingGraph.FindNodeByIdentifier(connectingKey);
							if (connectingNode != null && !pathfindingGraph.HasDirectConnectionBetween(node, connectingNode))
							{
								connectingTiles = GetMapTilesForPoint(connectingLocation);
								isConnectionTraversable = true;
								foreach (MapTile tile in connectingTiles)
								{
									if (tile.BlocksMovement || tile.BlocksNorth)
									{
										isConnectionTraversable = false;
										break;
									}
								}
								if (isConnectionTraversable) // If both connections are valid, then link them!
								{ pathfindingGraph.AddConnection(node, connectingNode, 1); }
							}
						}

						if (x > 0 && isTraversableWest)
						{
							connectingLocation = new Point(x - 1, y);
							connectingKey = GetKeyForPoint(connectingLocation);
							connectingNode = pathfindingGraph.FindNodeByIdentifier(connectingKey);
							if (connectingNode != null && !pathfindingGraph.HasDirectConnectionBetween(node, connectingNode))
							{
								connectingTiles = GetMapTilesForPoint(connectingLocation);
								isConnectionTraversable = true;
								foreach (MapTile tile in connectingTiles)
								{
									if (tile.BlocksMovement || tile.BlocksEast)
									{
										isConnectionTraversable = false;
										break;
									}
								}
								if (isConnectionTraversable) // If both connections are valid, then link them!
								{ pathfindingGraph.AddConnection(node, connectingNode, 1); }
							}
						}

						if (x < MAP_WIDTH_IN_TILES-1 && isTraversableEast)
						{
							connectingLocation = new Point(x + 1, y);
							connectingKey = GetKeyForPoint(connectingLocation);
							connectingNode = pathfindingGraph.FindNodeByIdentifier(connectingKey);
							if (connectingNode != null && !pathfindingGraph.HasDirectConnectionBetween(node, connectingNode))
							{
								connectingTiles = GetMapTilesForPoint(connectingLocation);
								isConnectionTraversable = true;
								foreach (MapTile tile in connectingTiles)
								{
									if (tile.BlocksMovement || tile.BlocksWest)
									{
										isConnectionTraversable = false;
										break;
									}
								}
								if (isConnectionTraversable) // If both connections are valid, then link them!
								{ pathfindingGraph.AddConnection(node, connectingNode, 1); }
							}
						}
					}
				}
			}

			DateTime endTime = DateTime.Now;
			TimeSpan duration = endTime.Subtract(startTime);
			System.Diagnostics.Debug.WriteLine("CalculateNodesAndConnections duration: "+duration.TotalMilliseconds+"ms");
			return pathfindingGraph;
		}

		public PointList GetActionableMapPoints(Faction faction)
		{
			PointList actionableLocations = new PointList();

			for (int y = 0; y < MAP_HEIGHT_IN_TILES; y++)
			{
				for (int x = 0; x < MAP_WIDTH_IN_TILES; x++)
				{
					MapTileList locationTiles = GetMapTilesForLocation(x, y);
					foreach (MapTile tile in locationTiles)
					{
						if (tile.HasActionsFor(faction))
						{ actionableLocations.Add(new Point(x, y)); }
					}
				}
			}

			return actionableLocations;
		}

		public List<AbstractTileAction> GetActionsForPoint(Point point)
		{
			MapTileList tileList = GetMapTilesForPoint(point);
			return tileList.Actions;
		}

		private List<Point> GetStraightPathBetweenTwoPoints(Point pointA, Point pointB)
		{
			List<Point> pointsBetween = new List<Point>();

			// http://en.wikipedia.org/wiki/Bresenham_algorithm
			int dx = Math.Abs(pointB.X - pointA.X);
			int dy = Math.Abs(pointB.Y - pointA.Y);
			int sx = -1;
			if (pointA.X < pointB.X)
			{ sx = 1; }

			int sy = -1;
			if (pointA.Y < pointB.Y)
			{ sy = 1; }

			int err = dx - dy;

			Boolean doBreak = false;
			int x0 = pointA.X;
			int y0 = pointA.Y;
			while (!doBreak)
			{
				pointsBetween.Add(new Point(x0, y0));
				if (x0 == pointB.X && y0 == pointB.Y)
				{ doBreak = true; }
				else
				{
					int e2 = 2 * err;
					if (e2 > -dy)
					{
						err = err - dy;
						x0 = x0 + sx;
					}

					if (e2 < dx)
					{
						err = err + dx;
						y0 = y0 + sy;
					}
				}
			}


			return pointsBetween;
		}
	}
}