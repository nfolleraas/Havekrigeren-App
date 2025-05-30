using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HavekrigerenApp.Exceptions
{
    public class TooManyAttemptsException : Exception
    {
        public TooManyAttemptsException(string message) 
            : base(message) { }

        public TooManyAttemptsException(string message, Exception innerException) 
            : base(message, innerException) { }
    }
}
