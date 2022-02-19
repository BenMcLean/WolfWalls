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
		public static IEnumerable<MapRect> MapRects(bool[][] input)
		{
			bool[][] map = new bool[input.Length][];
			for (int i = 0; i < input.Length; i++)
			{
				map[i] = new bool[input[i].Length];
				Array.Copy(input[i], map[i], input[i].Length);
			}
			while (NextRect(map) is MapRect rect)
			{
				for (int x = rect.X; x < rect.X + rect.Width; x++)
					for (int y = rect.Y; y < rect.Y + rect.Height; y++)
						map[x][y] = false;
				yield return rect;
			}
		}
		private static MapRect? NextRect(bool[][] map)
		{
			if (!NextEmpty(map, out int x, out int y))
				return null;
			int width = 1;
			for (; x + width < map.Length; width++)
				if (!map[x + width][y])
					break;
			int height = 1;
			bool done = false;
			for (; y + height < map[x].Length; height++)
			{
				for (int i = x; i < x + width; i++)
					if (!map[i][y + height])
					{
						done = true;
						break;
					}
				if (done)
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
				for (y = 0; y < map[x].Length; y++)
					if (map[x][y])
						return true;
			return false;
		}
	}
}
