using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PathFinding;
using WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.model;

namespace TestHarness
{
	class Program
	{
		static void Main(string[] args)
		{
			//TestPathFinding test = new TestPathFinding();
			TestPathDetermination test = new TestPathDetermination();
			test.Run();
			
			Console.ReadKey();
		}
	}
}