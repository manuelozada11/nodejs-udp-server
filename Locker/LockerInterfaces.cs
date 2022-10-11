public enum Code : byte
{
    Direct = 0x04,
    Broadcast = 0x03
}
public enum Func : byte
{
    Func01 = 0x01,
    Open = 0x02,
    Func03 = 0x02,
    IsFilled = 0x04,
    Close = 0x05,
}
public enum DoorState : byte
{
    Open = 0x01,
    Close = 0x00
}
public enum ContentState : byte
{
    Filled = 0x01,
    Empty = 0x00
}


public interface ILockerResponse
{
    public Code Code { get; set; }
    public Func Func { get; set; }
    public int Mod { get; set; }
    public int Res { get; set; }
    public int IDK { get; set; }
}
public interface IBroadcastResponse
{
    public ILockerResponse[] list { get; set; }
}

public interface ILocker
{
    public Code Cod { get; }
    public int Mod { get; }
    public bool OpenDoor(int door);
    public bool IsDoorOpen(int door);
    public bool isFiled(int door);
    public byte GenerateIDK();

}

public interface ILockerConnectionDriver
{
    public bool Connect();
    public byte[] StartListener();
    public void SendMessage(byte[] message);
}