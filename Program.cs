class Program
{
    static void ManageLocker(ILocker locker)
    {
        int sn = locker.Mod;
        int door = 1;
        if (locker.OpenDoor(door))
        {
            Console.WriteLine($"Locker: {sn}, Door: {door} Door Opened");

            if (locker.IsDoorOpen(door))
            {
                Console.WriteLine($"Locker: {sn}, Door: {door} opened");

                bool filled = locker.isFiled(door);
                if (filled)
                {
                    Console.WriteLine($"Locker: {sn}, Door: {door} is filled");
                }
                else
                {
                    Console.WriteLine($"Locker: {sn}, Door: {door} is not filled");
                }
            }
            else
            {
                Console.WriteLine($"Locker: {sn}, Door: {door} not opened");

            }
        }
        else
        {
            Console.WriteLine($"Locker: {sn}, Door: {door} not opened");
        }
    }
    static void Main(string[] args)
    {
        int board = 1;
        UDPLocker locker = new UDPLocker(Code.Direct, board);
        ManageLocker(locker);
    }
}