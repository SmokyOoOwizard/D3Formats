using System.Runtime.InteropServices;

namespace D3Formats
{
	[StructLayout(LayoutKind.Sequential)]
	public struct Vector3
	{
		public double X;
		public double Y;
		public double Z;

		public Vector3(double x, double y, double z)
		{
			X = x;
			Y = y;
			Z = z;
		}
	}
}