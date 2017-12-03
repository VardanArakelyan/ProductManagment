using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.Services
{
    public class DuplicateElementException : Exception
    {
        public DuplicateElementException()
        {
        }

        public DuplicateElementException(string message)
        : base(message)
        {
        }

        public DuplicateElementException(string message, Exception inner)
        : base(message, inner)
        {
        }
    }
}
