namespace D3Formats.Extensions
{
	public static class FormatsExtensions
	{
		public static string ToExtension(this Formats format)
		{
			switch (format)
			{
				case Formats.Obj:
					return ".obj";
				default:
					return string.Empty;
			}
		}

		public static Formats GetFormat(this string rawFormat)
		{
			if (string.IsNullOrWhiteSpace(rawFormat))
			{
				return Formats.Unknown;
			}

			switch (rawFormat.ToLower())
			{
				case "obj":
					return Formats.Obj;
				default:
					return Formats.Unknown;
			}
		}
	}
}
