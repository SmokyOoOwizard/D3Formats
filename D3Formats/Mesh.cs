namespace D3Formats
{
	public class Mesh
	{
		public string? Name { get; set; }
		public Vector[] Vertices { get; set; } = Array.Empty<Vector>();
		public Face[] Faces { get; set; } = Array.Empty<Face>();
		public Vector[] Normals { get; set; } = Array.Empty<Vector>();
	}
}
