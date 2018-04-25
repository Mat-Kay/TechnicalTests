<Query Kind="Program">
  <NuGetReference>morelinq</NuGetReference>
  <Namespace>MoreLinq</Namespace>
  <Namespace>System</Namespace>
  <Namespace>System.Collections.Generic</Namespace>
  <Namespace>System.ComponentModel</Namespace>
  <Namespace>System.Data</Namespace>
  <Namespace>System.Drawing</Namespace>
  <Namespace>System.Drawing</Namespace>
  <Namespace>System.Drawing.Imaging</Namespace>
  <Namespace>System.Drawing.Imaging</Namespace>
  <Namespace>System.IO</Namespace>
  <Namespace>System.Windows.Forms</Namespace>
</Query>

void Main()
{
	using (var image = new Bitmap("..\\Description\\sample-teal.png"))
	using (var scrambledImage = new Bitmap(image.Width, image.Height))
	{
		Work(image, scrambledImage);
		
		scrambledImage.Save("..\\Description\\scrambled-sample-teal.png");
	}
}

public struct Coord
{
	public int x, y;
}

// https://codegolf.stackexchange.com/a/35034
private void Work(Bitmap srcb, Bitmap outb)
{
	int w = srcb.Width, h = srcb.Height;
	Coord[] coord = new Coord[w * h];

	FastBitmap fsb = new FastBitmap(srcb);
	FastBitmap fob = new FastBitmap(outb);
	fsb.LockImage();
	fob.LockImage();
	ulong seed = 0;
	int numpix = 0;
	for (int y = 0; y < h; y++)
		for (int x = 0; x < w; numpix++, x++)
		{
			coord[numpix].x = x;
			coord[numpix].y = y;
			uint color = fsb.GetPixel(x, y);
			seed += color;
			fob.SetPixel(x, y, color);
		}
	fsb.UnlockImage();
	fob.UnlockImage();

	int half = numpix / 2;
	int limit = half;
	XorShift rng = new XorShift(seed);


	fob.LockImage();
	while (limit > 0)
	{
		int p = (int)(rng.next() % (uint)limit);
		int q = (int)(rng.next() % (uint)limit);
		uint color = fob.GetPixel(coord[p].x, coord[p].y);
		fob.SetPixel(coord[p].x, coord[p].y, fob.GetPixel(coord[half + q].x, coord[half + q].y));
		fob.SetPixel(coord[half + q].x, coord[half + q].y, color);
		limit--;
		if (p < limit)
		{
			coord[p] = coord[limit];
		}
		if (q < limit)
		{
			coord[half + q] = coord[half + limit];
		}
		if ((limit & 0xfff) == 0)
		{
			fob.UnlockImage();
			fob.LockImage();
		}
	}
	fob.UnlockImage();
}


//Adapted from Visual C# Kicks - http://www.vcskicks.com/
unsafe public class FastBitmap
{
	private Bitmap workingBitmap = null;
	private int width = 0;
	private BitmapData bitmapData = null;
	private Byte* pBase = null;

	public FastBitmap(Bitmap inputBitmap)
	{
		workingBitmap = inputBitmap;
	}

	public BitmapData LockImage()
	{
		Rectangle bounds = new Rectangle(Point.Empty, workingBitmap.Size);

		width = (int)(bounds.Width * 4 + 3) & ~3;

		//Lock Image
		bitmapData = workingBitmap.LockBits(bounds, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
		pBase = (Byte*)bitmapData.Scan0.ToPointer();
		return bitmapData;
	}

	private uint* pixelData = null;

	public uint GetPixel(int x, int y)
	{
		pixelData = (uint*)(pBase + y * width + x * 4);
		return *pixelData;
	}

	public uint GetNextPixel()
	{
		return *++pixelData;
	}

	public void GetPixelArray(int x, int y, uint[] Values, int offset, int count)
	{
		pixelData = (uint*)(pBase + y * width + x * 4);
		while (count-- > 0)
		{
			Values[offset++] = *pixelData++;
		}
	}

	public void SetPixel(int x, int y, uint color)
	{
		pixelData = (uint*)(pBase + y * width + x * 4);
		*pixelData = color;
	}

	public void SetNextPixel(uint color)
	{
		*++pixelData = color;
	}

	public void UnlockImage()
	{
		workingBitmap.UnlockBits(bitmapData);
		bitmapData = null;
		pBase = null;
	}
}

public class XorShift
{
	private ulong x; /* The state must be seeded with a nonzero value. */

	public XorShift(ulong seed)
	{
		x = seed;
	}

	public ulong next()
	{
		x ^= x >> 12; // a
		x ^= x << 25; // b
		x ^= x >> 27; // c
		return x * 2685821657736338717L;
	}
}