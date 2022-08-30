using D3Formats.Exceptions;
using D3Formats.Extensions;

namespace D3Formats
{
	public static class D3Formats
	{
		public const string SupportedFormats = ".Obj";

		public static Task<Mesh[]> ReadAsync(string path)
		{
			var fileFormat = Path.GetExtension(path).GetFormat();

			return ReadAsync(path, fileFormat);
		}
		public static async Task<Mesh[]> ReadAsync(string path, Formats format)
		{
			if (format == Formats.Unknown)
			{
				throw new UnknownFormatException();
			}

			var formatProvider = FormatProviderFactory.GetProvider(format);
			if (formatProvider == null)
			{
				throw new UnknownFormatProviderException();
			}

			using var fileStream = File.OpenRead(path);

			return await formatProvider.ReadAsync(fileStream);
		}
		public static async Task<Mesh[]> ReadAsync(Stream stream, Formats format)
		{
			if (format == Formats.Unknown)
			{
				throw new UnknownFormatException();
			}

			if (!stream.CanRead)
			{
				throw new IOException($"{nameof(stream)}.{nameof(stream.CanRead)} = false");
			}

			var formatProvider = FormatProviderFactory.GetProvider(format);
			if (formatProvider == null)
			{
				throw new UnknownFormatProviderException();
			}

			return await formatProvider.ReadAsync(stream);
		}

		public static async Task WriteAsync(Mesh[] meshes, string path, Formats format)
		{
			if (format == Formats.Unknown)
			{
				throw new UnknownFormatException();
			}

			var formatProvider = FormatProviderFactory.GetProvider(format);
			if (formatProvider == null)
			{
				throw new UnknownFormatProviderException();
			}

			if (string.IsNullOrWhiteSpace(Path.GetExtension(path)))
			{
				path += format.ToExtension();
			}

			using var fileStream = File.OpenWrite(path);

			await formatProvider.WriteAsync(fileStream, meshes);
		}
		public static async Task WriteAsync(Mesh[] meshes, Stream stream, Formats format)
		{
			if (format == Formats.Unknown)
			{
				throw new UnknownFormatException();
			}

			if (!stream.CanWrite)
			{
				throw new IOException($"{nameof(stream)}.{nameof(stream.CanWrite)} = false");
			}

			var formatProvider = FormatProviderFactory.GetProvider(format);
			if (formatProvider == null)
			{
				throw new UnknownFormatProviderException();
			}

			await formatProvider.WriteAsync(stream, meshes);
		}
	}
}
