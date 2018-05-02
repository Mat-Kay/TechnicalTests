<Query Kind="Program">
  <NuGetReference>morelinq</NuGetReference>
  <Namespace>MoreLinq</Namespace>
  <Namespace>System.Drawing</Namespace>
  <Namespace>System.Drawing.Imaging</Namespace>
</Query>

public class ReferenceColor
{
	public ReferenceColor(string name, Color color)
	{
		Name = name;
		Color = color;
	}
	
	public string Name {get;}
	
	public Color Color {get;}
}

void Main()
{
	var referenceColors = new List<ReferenceColor>() {
		{ new ReferenceColor("black", Color.Black) },
		{ new ReferenceColor("grey", Color.Gray) },
		{ new ReferenceColor("navy", Color.Navy) },
		{ new ReferenceColor("teal", Color.Teal) },
	};
	
	var image = new Bitmap("..\\Description\\scrambled-sample-grey.jpg");
	
	var dominantColor = GetDominantColor(image).Dump();
	
	referenceColors
		.Select(r => (ReferenceColor: r, Distance: GetEuclideanDistanceBetweenColors(r.Color, dominantColor)))
		.OrderBy(r => r.Distance)
		.Dump();
		
	// TODO: Some king of confidence score. 
	// TODO: Not sure where the reference colours come from. sample-grey.jpg is very close to both Color.Gray and Color.Teal
}

// https://github.com/THEjoezack/ColorMine for CIE distances
private int GetEuclideanDistanceBetweenColors(Color a, Color b)
{
	var redComponent = Math.Abs(a.R - b.R);
	var greenComponent = Math.Abs(a.G - b.G);
	var blueComponent = Math.Abs(a.B - b.B);
	
	return (int)Math.Sqrt(redComponent * redComponent + greenComponent * greenComponent + blueComponent * blueComponent);
}

// https://codereview.stackexchange.com/a/157725
private System.Drawing.Color GetDominantColor(Bitmap bmp)
{
	BitmapData srcData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, bmp.PixelFormat);

	int bytesPerPixel = Image.GetPixelFormatSize(srcData.PixelFormat) / 8;

	int stride = srcData.Stride;

	IntPtr scan0 = srcData.Scan0;

	long[] totals = new long[] { 0, 0, 0 };

	int width = bmp.Width * bytesPerPixel;
	int height = bmp.Height;

	unsafe
	{
		byte* p = (byte*)(void*)scan0;

		for (int y = 0; y < height; y++)
		{
			for (int x = 0; x < width; x += bytesPerPixel)
			{
				totals[0] += p[x + 0];
				totals[1] += p[x + 1];
				totals[2] += p[x + 2];
			}

			p += stride;
		}
	}

	long pixelCount = bmp.Width * height;

	int avgB = Convert.ToInt32(totals[0] / pixelCount);
	int avgG = Convert.ToInt32(totals[1] / pixelCount);
	int avgR = Convert.ToInt32(totals[2] / pixelCount);

	bmp.UnlockBits(srcData);

	return Color.FromArgb(avgR, avgG, avgB);

}

// Define other methods and classes here