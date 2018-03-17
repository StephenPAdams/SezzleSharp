using System;
using System.Collections.Generic;
using System.Text;

namespace SixFourThree.SezzleSharp.Models.Responses
{
    /// <summary>
    /// Response Base class
    /// </summary>
    public abstract class Response : ErrorResponse
    {
        // TODO: See if Sezzle supports rate limit metadata

        /// <summary>
        /// The total number of calls allowed within the 1-hour window
        /// </summary>
        public int RateLimitLimit { get; set; }

        /// <summary>
        /// The remaining number of calls available to your app within the 1-hour window
        /// </summary>
        public int RateLimitRemaining { get; set; }
    }
}
