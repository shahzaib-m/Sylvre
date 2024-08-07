﻿using System;

namespace Sylvre.WebAPI.Services.Exceptions
{
    internal class UserServiceException : Exception
    {
        public UserServiceException()
        {
        }

        public UserServiceException(string message) : base(message)
        {
        }

        public UserServiceException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}