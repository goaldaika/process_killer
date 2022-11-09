using MonitorProcessKiller;

public class Program
{
    public static int MinuteCount { get; set; }
    public static void Main(string[] args)
    {
        string name = args[0];
        int maxLifeTime = Convert.ToInt32(args[1]);
        int freq = Convert.ToInt32(args[2]);
        var killer = new ProcessKiller(name, maxLifeTime, freq);

        //Rerun the function after every "intervalTime" minute
        var timer = new System.Timers.Timer(freq * 60000);
        timer.Elapsed += (s, e) =>
        {
            killer.KillProcess();
            MinuteCount++;
            if (MinuteCount == 1)
            {
                Console.WriteLine("One minute passed !");
                MinuteCount = 0;
            }
        };
        //timer.Elapsed += Timer_Elapsed;
        timer.Enabled = true;
        timer.AutoReset = true;
        timer.Start();
        Console.WriteLine("Press any key to stop the program");
        Console.ReadKey();
    }

}