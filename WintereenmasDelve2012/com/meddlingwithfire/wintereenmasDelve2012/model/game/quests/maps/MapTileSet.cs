using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.model.game.quests.maps
{
	/// <summary>
	/// A collection of map tiles that organize into a *single-layered* multi-part object.  Examples
	/// are Tables, Bookcases, etc that require more than one tile location to render.
	/// </summary>
	public class MapTileSet
	{
		private List<MapTile> _mapTiles;
		private Dictionary<MapTile, Point> _mapTileRelativeLocations;

		public MapTileSet()
			: base()
		{
			_mapTiles = new List<MapTile>();
			_mapTileRelativeLocations = new Dictionary<MapTile, Point>();
		}

		public void AddTile(MapTile mapTile, Point toRelativePoint)
		{
			_mapTiles.Add(mapTile);
			_mapTileRelativeLocations[mapTile] = toRelativePoint;
		}

		public List<MapTile> MapTiles
		{
			get { return _mapTiles; }
		}

		public Point GetRelativeLocationFor(MapTile tile)
		{ return _mapTileRelativeLocations[tile]; }
	}
}
