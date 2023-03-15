using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Code.Converters
{
    internal static class DigitConverter
    {
        public static string Convert(this float value)
        {
            StringBuilder buffer = new StringBuilder();

            if (value < 1000)
                return value.ToString();

            if (value < 1000000)
            {
                int thousands = (int)(value / 1000);
                buffer.AppendFormat("{0}K", thousands);
            }
            else 
            {
                int mills = (int)(value / 1000000);
                buffer.AppendFormat("{0}M", mills);
            }

            return buffer.ToString();
        }
    }
}
