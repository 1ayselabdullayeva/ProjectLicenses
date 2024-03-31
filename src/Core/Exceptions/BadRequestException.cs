using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Core.Exceptions
{
    public class BadRequestException : BaseException<BadRequestException>
    {
        public BadRequestException() : base()
        {

        }
        public BadRequestException(string errorMessage) : base(errorMessage)
        {

        }
        public BadRequestException(string errorMessage, Dictionary<string, IEnumerable<string>> errors) : base(errorMessage)
        {
            Errors = errors;
        }
        public BadRequestException(string errorMessage, Exception innerException) : base(errorMessage, innerException)
        {

        }

        public Dictionary<string, IEnumerable<string>> Errors { get; set; }
    }
}
