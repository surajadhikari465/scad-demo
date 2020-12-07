using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LoggerMiddleware
{
    public static class UtilsExtensions
    {
        public static string BetweenConcat(this IEnumerable<string> args, string separator) 
            => args.SelectMany((a, i) => i == 0 ? new[] { a } : new[] { separator, a })
                   .Aggregate(new StringBuilder(), (builder, line) => builder.Append(line))
                   .ToString();
    }
}
