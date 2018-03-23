using System;
using System.Collections.Generic;
using System.Text;

namespace SixFourThree.SezzleSharp.Extensions
{
    public static class BooleanExtensions
    {
        public static string ToLowerString(this bool value)
        {
            return value.ToString().ToLower();
        }
    }
}
