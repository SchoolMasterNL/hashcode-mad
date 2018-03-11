using System;
using System.Collections.Generic;
using System.Text;

namespace hashcode_mad
{
    internal static class Manhattan
    {
        public static int Distance(int x1, int y1, int x2, int y2)
        {
            return Math.Abs(x1 - x2) + Math.Abs(y1 - y2);
        }
    }
}
