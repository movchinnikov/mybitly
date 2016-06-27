namespace MyBitly.BLL.Utils
{
	using System;

	public class Helper
	{
		public static void ShallowExceptions(Action @delegate)
		{
			try
			{
				@delegate();
			}
			catch
			{
				// ignored
			}
		}
	}
}