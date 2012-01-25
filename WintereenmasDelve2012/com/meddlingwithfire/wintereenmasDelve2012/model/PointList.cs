using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.model
{
	public class PointList : List<Point>
	{
		public PointList()
		{

		}

		public Boolean ContainsLocation(int x, int y)
		{
			foreach (Point point in this)
			{
				if (point.X == x && point.Y == y)
				{ return true; }
			}
			return false;
		}

		public void RemoveLocation(int x, int y)
		{
			for (int i = Count - 1; i >= 0; i--)
			{
				Point point = this[i];
				if (point.X == x && point.Y == y)
				{ RemoveAt(i); }
			}
		}

		public void AddPoints(List<Point> points)
		{
			for (int i = 0; i < points.Count; i++)
			{ Add(points[i]); }
		}

		public void AddDistinctPointsOnly(List<Point> points)
		{
			for (int i = 0; i < points.Count; i++)
			{
				Point point = points[i];
				if (!ContainsLocation(point.X, point.Y))
				{ Add(point); }
			}
		}

		public PointList Intersects(List<Point> points)
		{
			PointList intersects = new PointList();
			foreach (Point point in points)
			{
				if (ContainsLocation(point.X, point.Y))
				{ intersects.Add(point); }
			}
			return intersects;
		}

	}
}
