using System;
using System.Collections.Generic;
using System.Text;

namespace SixFourThree.SezzleSharp.Models.Responses
{
    /// <summary>
    /// Unless otherwise specified in our documentation, Sezzle returns a standard API error object.
    /// We attempt to keep these errors as consistent as possible, and will announce any changes in advance if they are required.
    /// </summary>
    public class ErrorResponse
    {
        /// <summary>
        /// Matches the HTTP Status code of the response
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// A programmatic identifier for the error. These rarely (if at all) change
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        //  A human-friendly string. These may change, and are intended to assist in debugging rather than program logic.
        /// </summary>
        public string Message { get; set; }
    }
}
