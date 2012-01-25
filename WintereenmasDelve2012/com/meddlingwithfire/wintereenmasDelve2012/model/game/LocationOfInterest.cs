using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PathFinding;

namespace WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.model.game
{
	public class LocationOfInterest
	{
		public List<Point> StepsToLocation;

		public LocationOfInterest(List<Point> stepsToLocation)
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
