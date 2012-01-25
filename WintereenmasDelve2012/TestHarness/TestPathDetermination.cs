using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.model;

namespace TestHarness
{
	public class TestPathDetermination
	{
		public void Run()
		{
			List<Point> points = GetStraightPathBetweenTwoPoints(new Point(1, 14), new Point(3, 17));
			//List<Point> points = GetStraightPathBetweenTwoPoints(new Point(2, 14), new Point(4, 15));
			//List<Point> points = GetStraightPathBetweenTwoPoints(new Point(5, 15), new Point(10, 15));
			//List<Point> points = GetStraightPathBetweenTwoPoints(new Point(5, 5), new Point(5, 10));
			//List<Point> points = GetStraightPathBetweenTwoPoints(new Point(1, 15), new Point(3, 14));

			double distance = 0;
			Point previousPoint = null;
			for (int i = 0; i < points.Count; i++)
			{
				Point point = points[i];
				if (previousPoint != null)
				{ distance = Math.Sqrt(Math.Pow(point.X - previousPoint.X, 2) + Math.Pow(point.Y - previousPoint.Y, 2)); }

				previousPoint = point;
				Console.WriteLine(i + ": (" + point.X + ", " + point.Y + ") distance: "+distance);
			}
		}

		public List<Point> GetStraightPathBetweenTwoPoints(Point pointA, Point pointB)
		{
			List<Point> pointsBetween = new List<Point>();

			// http://en.wikipedia.org/wiki/Bresenham_algorithm
			int dx = Math.Abs(pointB.X-pointA.X);
			int dy = Math.Abs(pointB.Y-pointA.Y) ;
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
			while(!doBreak)
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
					
					if (e2 <  dx)
					{
						err = err + dx;
						y0 = y0 + sy ;
					}
				}
			}

			
			return pointsBetween;
		}
	}
}
