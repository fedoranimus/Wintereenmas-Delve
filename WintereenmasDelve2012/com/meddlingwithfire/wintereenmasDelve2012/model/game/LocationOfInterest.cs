using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PathFinding;

namespace WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.model.game
{
	public class LocationOfInterest
	{
		public PointList StepsToLocation;

		public LocationOfInterest(PointList stepsToLocation)
			: base()
		{
			StepsToLocation = stepsToLocation;
		}

		public int NumberOfMovementsRequired
		{
			get { return StepsToLocation.Count; }
		}
	}
}
