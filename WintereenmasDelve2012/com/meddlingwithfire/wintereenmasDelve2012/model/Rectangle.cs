using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.model
{
	public class Rectangle
	{
		public int X;
		public int Y;
		public int Width;
		public int Height;

		public Rectangle(int x, int y, int width, int height)
			: base()
		{
			X = x;
			Y = y;
			Width = width;
			Height = height;
		}

		public static Rectangle FromTwoPoints(Point pointA, Point pointB)
		{
			return new Rectangle(
				Math.Min(pointA.X, pointB.X),
				Math.Min(pointA.Y, pointB.Y),
				Math.Max(pointA.X, pointB.X) - Math.Min(pointA.X, pointB.X),
				Math.Max(pointA.Y, pointB.Y) - Math.Min(pointA.Y, pointB.Y)
			);
		}

		public Boolean Contains(Point point)
		{
			if (point.X < X)
			{ return false; }
			if (point.X > X + Width)
			{ return false; }
			if (point.Y < Y)
			{ return false; }
			if (point.Y > Y + Height)
			{ return false; }
			return true;
		}
	}
}
