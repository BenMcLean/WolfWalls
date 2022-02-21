using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Linq;

namespace WolfWallsTest
{
	[TestClass]
	public class ComparisonTest
	{
		[TestMethod]
		public void Averages()
		{
			bool[][][] maps = Directory.GetFiles(@".\..\..\..\", "*.csv").Select(path => WolfWalls.Program.LoadMap(path)).ToArray();
			Console.WriteLine("Without overlap, average is " + maps.Select(map => WolfWalls.MapRectsNoOverlap.MapRects(map).Count()).Average() + ".");
			Console.WriteLine("With overlap, average is " + maps.Select(map => WolfWalls.MapRectsOverlap.MapRects(map).Count()).Average() + ".");
		}
	}
}
