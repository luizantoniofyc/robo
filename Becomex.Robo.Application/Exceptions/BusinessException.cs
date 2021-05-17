using System;
using System.Collections.Generic;
using System.Text;

namespace Becomex.Robo.Application.Exceptions
{
    public class BusinessException : Exception
    {
        public BusinessException(string message) : base(message)
        {
        }

        public BusinessException(string message, string code) : base(message)
        {
            this.Source = code;
        }
    }
}
