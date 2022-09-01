namespace D3Formats.Extensions
{
	public static class VectorExtensions
	{
		public static IEnumerable<Vector3> ScaleUpTo(this IEnumerable<Vector3> source, Vector3 to)
		{
			double mX = source.Max(v => Math.Max(v.X, Math.Max(v.Y, v.Z)));
			double mY = source.Max(v => Math.Max(v.X, Math.Max(v.Y, v.Z)));
			double mZ = source.Max(v => Math.Max(v.X, Math.Max(v.Y, v.Z)));

			double sX = to.X / mX;
			double sY = to.Y / mY;
			double sZ = to.Z / mZ;

			return source.Select(v => new Vector3(v.X * sX, v.Y * sY, v.Z * sZ));
		}
	}
}