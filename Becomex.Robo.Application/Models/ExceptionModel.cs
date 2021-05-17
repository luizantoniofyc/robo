using System;
using System.Collections.Generic;
using System.Text;

namespace Becomex.Robo.Application.Models
{
    /// <summary>
	/// Represents a set of properties for exception handling
	/// </summary>
    public class ExceptionModel
    {
        /// <summary>
        /// Exception type (e.g. Business)
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Exception code (e.g. BN001)
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Exception message (e.g. Robot not found.)
        /// </summary>
        public string Message { get; set; }
    }
}
