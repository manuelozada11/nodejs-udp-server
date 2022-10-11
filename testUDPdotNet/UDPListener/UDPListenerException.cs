[Serializable]
public class UDPListenerException : Exception
{
    public UDPListenerException() : base() { }
    public UDPListenerException(string message) : base(message) { }
    public UDPListenerException(string message, int errorCode) : base(message) { }
    public UDPListenerException(string message, Exception inner) : base(message, inner) { }
}