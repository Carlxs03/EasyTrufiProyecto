using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyTrufi.Core.Exceptions
{
    public class BussinesException : Exception
    {
        public int StatusCode { get; set; } = 500;
        public string ErrorCode { get; set; }

        public BussinesException()
        {
        }

        public BussinesException(string message) : base(message)
        {
        }

        public BussinesException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public BussinesException(string message, int statusCode) : base(message)
        {
            StatusCode = statusCode;
        }

        public BussinesException(string message, string errorCode, int statusCode) : base(message)
        {
            ErrorCode = errorCode;
            StatusCode = statusCode;
        }

    }
}
