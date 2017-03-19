using System;

namespace Kontur.GameStats.Server.Infrastructure
{
    public class ValidationException : Exception
    {
        public ValidationException(string property, string message) : base(message)
        {
            Property = property;
        }

        public string Property { get; }
    }
}