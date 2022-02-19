using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WolfWalls
{
	public struct MapRect
	{
		public int X;
		public int Y;
		public int Width;
		public int Height;
		public static List<MapRect> MapRects(bool[][] input)
		{
			bool[][] map = new bool[input.Length][];
			for (int i = 0; i < input.Length; i++)
			{
				map[i] = new bool[input[i].Length];
				Array.Copy(input[i], map[i], input[i].Length);
			}
			List<MapRect> list = new List<MapRect>();
			while (NextRect(map) is MapRect rect)
			{
				rect.FillIn(map);
				list.Add(rect);
			}
			return list;
		}
		private static MapRect? NextRect(bool[][] map)
		{
			if (!NextEmpty(map, out int x, out int y))
				return null;
			int width = 1;
			for (; x + width < map.Length; width++)
				if (!map[x + width][y])
				{
					width--;
					break;
				}
			int height = 1;
			for (; y + height < map[x].Length; height++)
				for (int i = x; i < x + width; i++)
					if (!map[i][y + height])
					{
						height--;
						break;
					}
			return new MapRect
			{
				X = x,
				Y = y,
				Width = width,
				Height = height,
			};
		}
		private static bool NextEmpty(bool[][] map, out int x, out int y)
		{
			y = 0;
			for (x = 0; x < map.Length; x++)
				for (; y < map[x].Length; y++)
					if (!map[x][y])
						return true;
			return false;
		}
		private bool[][] FillIn(bool[][] map) => FillIn(map, this);
		private static bool[][] FillIn(bool[][] map, MapRect mapRect) => FillIn(map, mapRect.X, mapRect.Y, mapRect.Width, mapRect.Height);
		private static bool[][] FillIn(bool[][] map, int x, int y, int width, int height)
		{
			for (int x2 = x; x2 < width; x2++)
				for (int y2 = y; y2 < height; y2++)
					map[x2][y2] = false;
			return map;
		}
	}
}
