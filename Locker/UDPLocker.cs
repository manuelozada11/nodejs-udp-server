
public class UDPLocker : ILocker
{
    private UDPListener listener;

    public UDPLocker()
    {
        UDPConfigs config = GetUDPConfiguration();
        listener = new UDPListener(config.serverIp, config.listenPort, config.sendPort, config.timeout);
    }
    private Code _Cod;
    public Code Cod
    {
        get => _Cod;
    }
    private int _Mod;
    public int Mod
    {
        get => _Mod;
    }

    public bool OpenDoor(int door)
    {
        byte[] bytes = new byte[5];
        bytes[0] = (byte)_Cod;
        bytes[1] = (byte)Func.Open;
        bytes[2] = Convert.ToByte(_Mod);
        bytes[3] = Convert.ToByte(door);
        bytes[4] = GenerateIDK();
        listener.SendMessage(bytes);
        byte[] response = listener.StartListener();
        if (response != null && response.Length > 4)
        {
            if (response[1] == Convert.ToByte(_Mod) && response[2] == Convert.ToByte(door) && response[3] == Convert.ToByte(1))
            {
                return true;
            }
        }
        return false;
    }
    public bool IsDoorOpen(int door)
    {
        //TODO
        return false;

    }
    public bool isFiled(int door)
    {
        //TODO
        return false;
    }
    public byte GenerateIDK()
    {
        Random rand = new Random();
        return (byte)rand.Next(0, 256);
    }
    private struct UDPConfigs
    {
        public string serverIp;
        public int listenPort, sendPort, timeout;
        public UDPConfigs(string serverIp, int listenPort, int sendPort, int timeoutSeconds)
        {
            this.serverIp = serverIp;
            this.listenPort = listenPort;
            this.sendPort = sendPort;
            this.timeout = timeoutSeconds * 1000;
        }
    }
    private UDPConfigs GetUDPConfiguration()
    {
        //TODO: Get configs from config file
        return new UDPConfigs("192.168.0.4", 5000, 3000, 5);
    }

    public UDPLocker(Code Cod, int Mod)
    {
        this._Mod = Mod;
        this._Cod = Cod;
        UDPConfigs udpConfig = GetUDPConfiguration();
        listener = new UDPListener(udpConfig.serverIp, udpConfig.listenPort, udpConfig.sendPort, udpConfig.timeout);
        listener.Connect();
    }
}