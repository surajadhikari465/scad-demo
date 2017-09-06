using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebSupport.Helpers
{
	public static class ValidationHelper
	{
		/// <summary>
		/// Parses newline-delimited text into an enumerable collection of item scan codes
		/// </summary>
		/// <param name="textWithScanCodes">text representing a list of scan codes,
		///     each on its own line</param>
		/// <returns>collection of strings representng unique scan codes</returns>
		public static IEnumerable<string> ParseScanCodes(string textWithScanCodes)
		{
			var parsedScanCodes = textWithScanCodes.Split(
				new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

			return parsedScanCodes.Select(s => s.Trim()).Distinct();
		}
	}
}