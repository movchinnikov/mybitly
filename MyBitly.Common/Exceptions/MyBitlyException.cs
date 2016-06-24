namespace MyBitly.Common.Exceptions
{
    using System;

    public class MyBitlyException : Exception
    {
        public string Code { get; set; }

        public byte StatusCode { get; set; }

        public MyBitlyException() : base() {}
        
        public MyBitlyException(string message) : base(message) { }
        
        public MyBitlyException(string message, Exception innerException) : base(message, innerException) { }
    }
}