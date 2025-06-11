using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FloraApp.Model.Exceptions
{
    public class UserExceptions : Exception
    {
        public UserExceptions(string message) : base(message) { }
        public UserExceptions(string message, Exception inner) : base(message, inner) { }
    }
}
