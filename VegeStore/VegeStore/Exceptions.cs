using System;

namespace VegeStore
{
    [Serializable]
    public class OverloadException : Exception
    {
        public OverloadException() { }
        public OverloadException(string message) : base(message) { }
        public OverloadException(string message, Exception inner) : base(message, inner) { }
        protected OverloadException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

    [Serializable]
    public class OverweightException : Exception
    {
        public OverweightException() { }
        public OverweightException(string message) : base(message) { }
        public OverweightException(string message, Exception inner) : base(message, inner) { }
        protected OverweightException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
