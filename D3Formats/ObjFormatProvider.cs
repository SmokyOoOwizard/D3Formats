using D3Formats.Exceptions;
using System.Text;

namespace D3Formats
{
	internal class ObjFormatProvider : IFormatProvider
	{
		public async Task<Mesh[]> ReadAsync(Stream stream, bool leafOpen = true)
		{
			using var reader = new StreamReader(stream, leaveOpen: leafOpen);

			var meshes = new List<Mesh>();

			var vertices = new List<Vector3>();
			var faces = new List<Face>();
			var normals = new List<Vector3>();

			string? objName = null;
			while (!reader.EndOfStream)
			{
				var str = await reader.ReadLineAsync();

				if (string.IsNullOrWhiteSpace(str))
				{
					continue;
				}

				if (str.StartsWith("v ")) // vertex
				{
					ParseVertex(str, vertices);
				}
				else if (str.StartsWith("f ")) // face
				{
					ParseFace(str, faces);
				}
				else if (str.StartsWith("vn ")) // normal
				{
					ParseNormal(str, normals);
				}
				else if (str.StartsWith("o "))
				{
					if (!string.IsNullOrWhiteSpace(objName) || vertices.Count > 0)
					{
						meshes.Add(new Mesh
						{
							Name = objName,
							Vertices = vertices.ToArray(),
							Normals = normals.ToArray(),
							Faces = faces.ToArray(),
						});

						vertices.Clear();
						normals.Clear();
						faces.Clear();
					}

					objName = str[2..];
				}
			}

			if (!string.IsNullOrWhiteSpace(objName) || vertices.Count > 0)
			{
				meshes.Add(new Mesh
				{
					Name = objName,
					Vertices = vertices.ToArray(),
					Normals = normals.ToArray(),
					Faces = faces.ToArray(),
				});

				vertices.Clear();
				normals.Clear();
				faces.Clear();
			}

			return meshes.ToArray();
		}
		public async Task WriteAsync(Stream stream, params Mesh[] meshes)
		{
			const int WRITE_BUFFER_SIZE = 700;

			var sb = new StringBuilder();
			using var writer = new StreamWriter(stream, leaveOpen: true);

			foreach (var model in meshes)
			{
				sb.AppendLine($"o {model.Name}");

				#region Vertex
				for (int i = 0; i < model.Vertices.Length; i++)
				{
					var vertex = model.Vertices[i];

					sb.AppendLine($"v {vertex.X} {vertex.Y} {vertex.Z}");

					if (i % WRITE_BUFFER_SIZE == WRITE_BUFFER_SIZE - 1)
					{
						await writer.WriteAsync(sb.ToString());
						sb.Clear();
					}
				}

				if (sb.Length > 0)
				{
					await writer.WriteAsync(sb.ToString());
					sb.Clear();
				}
				#endregion

				#region Normals
				for (int i = 0; i < model.Normals.Length; i++)
				{
					var normal = model.Normals[i];
					sb.AppendLine($"vn {normal.X} {normal.Y} {normal.Z}");

					if (i % WRITE_BUFFER_SIZE == WRITE_BUFFER_SIZE - 1)
					{
						await writer.WriteAsync(sb.ToString());
						sb.Clear();
					}
				}

				if (sb.Length > 0)
				{
					await writer.WriteAsync(sb.ToString());
					sb.Clear();
				}
				#endregion

				for (int i = 0; i < model.Faces.Length; i++)
				{
					var face = model.Faces[i];

					sb.Append("f ");
					for (int y = 0; y < face.Indices.Length; y++)
					{
						var faceVertex = face.Indices[y];
						sb.Append(faceVertex.VertexIndex);
						if (faceVertex.UVIndex == null || faceVertex.NormalIndex == null)
						{
							sb.Append('/');
							if (faceVertex.UVIndex != null)
							{
								sb.Append(faceVertex.UVIndex);
							}

							if (faceVertex.NormalIndex != null)
							{
								sb.Append($"/{faceVertex.NormalIndex}");
							}
						}

						if (y + 1 < face.Indices.Length)
						{
							sb.Append(' ');
						}
						else
						{
							sb.AppendLine();
						}
					}

					if (i % WRITE_BUFFER_SIZE == WRITE_BUFFER_SIZE - 1)
					{
						await writer.WriteAsync(sb.ToString());
						sb.Clear();
					}
				}

				if (sb.Length > 0)
				{
					await writer.WriteAsync(sb.ToString());
					sb.Clear();
				}
			}
		}

		private static void ParseVertex(string str, ICollection<Vector3> vertices)
		{
			var parts = str.Split(' ');
			if (parts.Length < 4)
			{
				throw new InvalidFormatException();
			}

			if (double.TryParse(parts[1], out var x) && double.TryParse(parts[2], out var y) && double.TryParse(parts[3], out var z))
			{
				vertices.Add(new Vector3(x, y, z));
			}
			else
			{
				throw new InvalidFormatException();
			}
		}
		private static void ParseFace(string str, ICollection<Face> faces)
		{
			var parts = str.Split(' ');
			if (parts.Length < 4)
			{
				throw new InvalidFormatException();
			}

			var indices = new List<FaceVertex>();

			for (int i = 1; i < parts.Length; i++)
			{
				var f = parts[i].Split('/');
				var indexRaw = f[0];
				var uvRaw = f.Length >= 2 ? f[1] : null;
				var normalRaw = f.Length == 3 ? f[2] : null;

				var faceVertex = new FaceVertex();

				if (int.TryParse(indexRaw, out var index))
				{
					faceVertex.VertexIndex = index;
				}
				else
				{
					throw new InvalidFormatException();
				}

				if (!string.IsNullOrWhiteSpace(uvRaw))
				{
					if (int.TryParse(uvRaw, out var uv))
					{
						faceVertex.UVIndex = uv;
					}
					else
					{
						throw new InvalidFormatException();
					}
				}

				if (!string.IsNullOrWhiteSpace(normalRaw))
				{
					if (int.TryParse(normalRaw, out var normal))
					{
						faceVertex.NormalIndex = normal;
					}
					else
					{
						throw new InvalidFormatException();
					}
				}

				indices.Add(faceVertex);
			}

			faces.Add(new Face() { Indices = indices.ToArray() });
		}
		private static void ParseNormal(string str, ICollection<Vector3> normals)
		{
			var parts = str.Split(' ');
			if (parts.Length < 4)
			{
				throw new InvalidFormatException();
			}

			if (double.TryParse(parts[1], out var x) && double.TryParse(parts[2], out var y) && double.TryParse(parts[3], out var z))
			{
				normals.Add(new Vector3(x, y, z));
			}
			else
			{
				throw new InvalidFormatException();
			}
		}
	}
}
