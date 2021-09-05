using System;

namespace Domain.Exceptions
{
    public class RegisterException : Exception
    {
        public RegisterException() : base("Error occured while registering user.") { }
    }
}