﻿namespace Shared.Exceptions
{
    public class BadRequestException : Exception
    {
        public BadRequestException(string message = "Bad Request!") : base(message)
        {

        }
    }
}
