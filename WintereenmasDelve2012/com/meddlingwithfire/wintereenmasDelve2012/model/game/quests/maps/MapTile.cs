using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.game;
using WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.model.game.quests.maps.tileActions;

namespace WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.model.game.quests.maps
{
	public class MapTile
	{
		public String ImagePath;
		public Boolean BlocksMovement;
		public Boolean BlocksWest;
		public Boolean BlocksNorth;
		public Boolean BlocksEast;
		public Boolean BlocksSouth;

		public Boolean BlocksLineOfSight;
		public Boolean BlocksLineOfSightNorth;
		public Boolean BlocksLineOfSightWest;
		public Boolean BlocksLineOfSightSouth;
		public Boolean BlocksLineOfSightEast;

		private List<AbstractTileAction> _actions;
		private List<Faction> _allowedFactionsToMoveThrough;

		public MapTile(String tileImagePath, Boolean blocksMovement, Boolean blocksWest, Boolean blocksNorth, Boolean blocksEast, Boolean blocksSouth)
			: this(tileImagePath, blocksMovement, blocksWest, blocksNorth, blocksEast, blocksSouth, false, false, false, false, false)
		{ }

		public MapTile(String tileImagePath, Boolean blocksMovement, Boolean blocksWest, Boolean blocksNorth, Boolean blocksEast, Boolean blocksSouth, Boolean blocksLineOfSight, Boolean blocksLineOfSightNorth, Boolean blocksLineOfSightEast, Boolean blocksLineOfSightSouth, Boolean blocksLineOfSightWest)
		{
			ImagePath = tileImagePath;
			BlocksMovement = blocksMovement;
			BlocksWest = blocksWest;
			BlocksNorth = blocksNorth;
			BlocksEast = blocksEast;
			BlocksSouth = blocksSouth;

			BlocksLineOfSight = blocksLineOfSight;
			BlocksLineOfSightNorth = blocksLineOfSightNorth;
			BlocksLineOfSightWest = blocksLineOfSightWest;
			BlocksLineOfSightSouth = blocksLineOfSightSouth;
			BlocksLineOfSightEast = blocksLineOfSightEast;

			_allowedFactionsToMoveThrough = new List<Faction>();

			_actions = new List<AbstractTileAction>();
		}

		public Boolean HasActionsFor(Faction faction)
		{
			foreach (AbstractTileAction action in _actions)
			{
				if (action.IsAvailableTo(faction))
				{ return true; }
			}
			return false;
		}

		public void AddAction(AbstractTileAction action)
		{
			_actions.Add(action);
		}

		public void AllowFactionToPassThrough(Faction faction)
		{
			_allowedFactionsToMoveThrough.Add(faction);
		}

		public List<AbstractTileAction> Actions
		{
			get { return _actions; }
		}

		/// <summary>
		/// Whether or not this tile can be walked on.
		/// </summary>
		/// <returns></returns>
		public Boolean AllowsMovementForFaction(Faction faction)
		{ return (!BlocksMovement || _allowedFactionsToMoveThrough.Contains(faction)); }

		virtual public Object Clone()
		{ return new MapTile(ImagePath, BlocksMovement, BlocksWest, BlocksNorth, BlocksEast, BlocksSouth, BlocksLineOfSight, BlocksLineOfSightNorth, BlocksLineOfSightEast, BlocksLineOfSightSouth, BlocksLineOfSightWest); }

		virtual public Boolean Equals(MapTile otherTile)
		{
			// We probably only need to check the image path, since we are not re-using images across multiple tiles, but added the other checks for safety
			return (ImagePath.Equals(otherTile.ImagePath) /*&& BlocksMovement == otherTile.BlocksMovement*/ && BlocksWest == otherTile.BlocksWest && BlocksNorth == otherTile.BlocksNorth && BlocksEast == otherTile.BlocksEast && BlocksSouth == otherTile.BlocksSouth);
		}
	}
}
