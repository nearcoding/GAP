using System.Collections.Generic;
using System.Linq;

namespace GAP.Helpers
{
    public static class CsvConverter
    {
        public static string ToCSV<T>(this IEnumerable<T> list)
        {
            var stringValues = list.Select(item =>
                (item == null ? string.Empty : item.ToString()));
            return string.Join(",", stringValues.ToArray());
        }
    }

}
