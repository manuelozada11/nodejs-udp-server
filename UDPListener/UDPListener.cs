using System.Net;
using System.Net.Sockets;
using System.Text;

public class UDPListener : ILockerConnectionDriver
{
    private int listenPort;
    private int sendPort;
    private string serverIp;
    private int timeout;
    private UdpClient listener;
    public delegate void ReceiveCallBack(string[] response);

    public UDPListener(string serverIp, int listenPort, int sendPort, int timeout)
    {
        this.serverIp = serverIp;
        this.listenPort = listenPort;
        this.sendPort = sendPort;
        this.timeout = timeout;
        listener = new UdpClient(listenPort);
        listener.Client.ReceiveTimeout = timeout;
    }

    public bool Connect()
    {
        try
        {
            listener.Connect(serverIp, sendPort);
            return true;
        }
        catch (SocketException e)
        {
            Console.WriteLine(e);
            return false;
        }

    }

    public byte[] StartListener()
    {
        try
        {
            while (true)
            {
                Console.WriteLine("Waiting for broadcast");
                IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
                byte[] bytes = listener.Receive(ref RemoteIpEndPoint);
                if (bytes != null)
                {
                    Console.WriteLine("Broadcast Received");
                    Console.WriteLine("Received: " +
                                       BitConverter.ToString(bytes));
                    Console.WriteLine("Server: " +
                                                RemoteIpEndPoint.Address.ToString() +
                                                " Port: " +
                                                RemoteIpEndPoint.Port.ToString());
                    return bytes;
                }
                Thread.Sleep(100);
            }
        }
        catch (SocketException e)
        {
            if (e.ErrorCode == (int)SocketError.TimedOut)
            {
                Console.WriteLine("Tempo Esgotado");
                throw new UDPListenerException($"TimedOut ({e.ErrorCode})", e.ErrorCode);
            }
            else
            {
                throw e;
            }
        }
        finally
        {
            listener.Close();
        }
    }

    public void SendMessage(byte[] message)
    {
        try
        {
            listener.Send(message, message.Length);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
    }
}