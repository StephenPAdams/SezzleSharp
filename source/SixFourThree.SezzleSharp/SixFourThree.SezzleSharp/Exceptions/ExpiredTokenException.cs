using System;
using System.Collections.Generic;
using System.Text;

namespace SixFourThree.SezzleSharp.Exceptions
{
    public class ExpiredTokenException : Exception
    {
        public ExpiredTokenException() { }

        public ExpiredTokenException(string message) : base(message) { }

        public ExpiredTokenException(string message, Exception inner) : base(message, inner) { }
    }
}
