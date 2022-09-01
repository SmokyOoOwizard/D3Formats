namespace D3Formats
{
	public class Mesh
	{
		public string? Name { get; set; }
		public Vector3[] Vertices { get; set; } = Array.Empty<Vector3>();
		public Face[] Faces { get; set; } = Array.Empty<Face>();
		public Vector3[] Normals { get; set; } = Array.Empty<Vector3>();
	}
}
