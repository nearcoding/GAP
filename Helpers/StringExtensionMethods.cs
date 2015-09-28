using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAP.Helpers
{
    public static class StringExtensionMethods
    {
        public static bool IsInteger(this String str)
        {
            int result = 0;
            return Int32.TryParse(str, out result);
        }

        public static bool IsDecimal(this string str)
        {
            decimal dec = 0;
            return Decimal.TryParse(str, out dec);
        }

        public static bool IsInteger(this List<string> lst)
        {
            int result = 0;
            bool isDirty = false;
            Parallel.ForEach(lst, currentItem =>
                {
                    if (!Int32.TryParse(currentItem, out result))
                    {
                        isDirty = true;
                        return;
                    }
                });
            return !isDirty;
        }       
    }//end class
}//end namespace