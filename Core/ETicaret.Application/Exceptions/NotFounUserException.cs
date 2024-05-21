using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret.Application.Exceptions
{
    public class NotFounUserException : Exception
    {
        public NotFounUserException() : base("kullanıcı adı veya şifre hatalıs")
        {

        }
        public NotFounUserException(string? message): base(message)
        {

        }
        public NotFounUserException(string? message,Exception? innerException) : base(message,innerException)
        {

        }
    }
}
