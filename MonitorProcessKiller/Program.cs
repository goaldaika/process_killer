using MonitorProcessKiller;

public class Program
{
    public static int MinuteCount { get; set; }
    public static void Main(string[] args)
    {
        string name = args[0];
        int maxLifeTime = Convert.ToInt32(args[1]);
        int freq = Convert.ToInt32(args[2]);

        if (!string.IsNullOrWhiteSpace(name))
        {
            var killer = new ProcessKiller(name, maxLifeTime, freq);

            Console.WriteLine("Press any key to stop the program ...");

            var time = freq * 60000;

            var timer = new Timer(state => killer.Monitoring(), null, 0, time);

            Console.ReadKey();
        }
        else
        {
            Console.WriteLine("Please require a process for monitoring ... !!");
        }
    }

}