namespace D3Formats
{
	public struct Vector
	{
		public double X;
		public double Y;
		public double Z;
		public double W;

		public Vector(double x, double y, double z)
		{
			X = x;
			Y = y;
			Z = z;
			W = 1;
		}

		public Vector(double x, double y, double z, double w)
		{
			X = x;
			Y = y;
			Z = z;
			W = w;
		}

		public static Vector operator *(Vector v, double x)
		{
			return new Vector(v.X * x, v.Y * x, v.Z * x, v.W);
		}
	}
}