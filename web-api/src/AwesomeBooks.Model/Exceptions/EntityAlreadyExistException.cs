﻿using System;
using System.Collections.Generic;
using System.Text;

namespace AwesomeBooks.Model.Exceptions
{
    public class EntityAlreadyExistsException : Exception
    {
        public EntityAlreadyExistsException(string message) : base(message) { }
        public EntityAlreadyExistsException(string message, Exception innerException) : base(message, innerException) { }
    }
}
