namespace D3Formats
{
	public static class FormatProviderFactory
	{
		public static IFormatProvider? GetProvider(Formats format)
		{
			switch (format)
			{
				case Formats.Obj:
					return new ObjFormatProvider();
				default:
					return default;
			}
		}
	}
}
