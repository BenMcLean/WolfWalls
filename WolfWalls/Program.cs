﻿using AnimatedGif;
using SixLabors.ImageSharp;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace WolfWalls
{
	public class Program
	{
		public static void Main(string[] args)
		{
			if (!args?.Any() ?? false)
				throw new InvalidDataException();
			bool[][] map = LoadMap(args[0]);
			int[] palette = TextureMethods.LoadPalette(File.ReadAllText(@".\..\..\..\yamble.pal"));
			byte[] image = new byte[map.Length * map.Length * 4];
			for (int x = 0; x < map.Length; x++)
				for (int y = 0; y < map[x].Length; y++)
					if (!map[x][y])
						image.DrawPixel(140, 140, 140, 255, x, y, map.Length);
			SixLabors.ImageSharp.Image.LoadPixelData<SixLabors.ImageSharp.PixelFormats.Rgba32>(image, map.Length, map.Length)
				.SaveAsPng("frame0.png");
			int frames = 0;
			List<MapRect> list = MapRect.MapRects(map);
			foreach (MapRect rect in list)
			{
				frames++;
				image.DrawRectangle(palette[frames % palette.Length], rect.X, rect.Y, rect.Width, rect.Height, map.Length);
				SixLabors.ImageSharp.Image.LoadPixelData<SixLabors.ImageSharp.PixelFormats.Rgba32>(image, map.Length, map.Length)
					.SaveAsPng("frame" + frames + ".png");
			}
			using (AnimatedGifCreator gif = AnimatedGif.AnimatedGif.Create("output.gif", 256))
			{
				for (int frame = 0; frame < frames; frame++)
					gif.AddFrame(System.Drawing.Image.FromFile("frame" + frame + ".png"), delay: -1, quality: GifQuality.Bit8);
			}
		}
		public static bool[][] LoadMap(string path)
		{
			if (!File.Exists(path))
				throw new FileNotFoundException(path);
			bool[][] map;
			using (StreamReader streamReader = File.OpenText(path))
			{
				if (streamReader.ReadLine() is string firstLine)
				{
					string[] strings = firstLine.Split(',');
					map = new bool[strings.Length][];
					map[0] = GetRow(strings);
				}
				else throw new InvalidDataException("Could not parse file at \"" + path + "\"");
				bool[] GetRow(string[] strings)
				{
					bool[] bools = new bool[strings.Length];
					for (int i = 0; i < bools.Length; i++)
						bools[i] = !strings[i].Equals("0");
					return bools;
				}
				int row = 0;
				for (string line; (line = streamReader.ReadLine()) != null;)
					map[++row] = GetRow(line.Split(','));
			}
			return map;
		}
	}
}
