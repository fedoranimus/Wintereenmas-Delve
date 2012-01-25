using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.model
{
	public class TwoLinkedPoints
	{
		public Point PointA;
		public Point PointB;

		public TwoLinkedPoints(int aX, int aY, int bX, int bY)
		{
			PointA = new Point(aX, aY);
			PointB = new Point(bX, bY);
		}
	}
}
