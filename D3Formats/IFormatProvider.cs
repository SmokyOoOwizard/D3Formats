namespace D3Formats
{
	public interface IFormatProvider
	{
		Task<Mesh[]> ReadAsync(Stream stream, bool leafOpen = true);
		Task WriteAsync(Stream stream, params Mesh[] models);
	}
}
