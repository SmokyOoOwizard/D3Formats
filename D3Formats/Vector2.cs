using System.Runtime.InteropServices;

namespace D3Formats
{
	[StructLayout(LayoutKind.Sequential)]
	public struct Vector2
	{
		public double X;
		public double Y;

		public Vector2(double x, double y)
		{
			X = x;
			Y = y;
		}
	}
}