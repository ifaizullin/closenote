using System;
using System.Diagnostics;
using System.Threading;

namespace CloseNote
{    
    class Program
    {
        static void Main(string[] args)
        {
            if (!TryParse(args)) return;
            Sheduler sheduler = new Sheduler(args[0], Int32.Parse(args[1]), Int32.Parse(args[2]));            
            Thread monitor = new Thread(sheduler.Startcycle);
            monitor.IsBackground = true;
            monitor.Start();
            Console.ReadLine();
        }
        public class Sheduler
        {
            private string processname;
            private int timelive;
            private int timeout;

            public Sheduler(string _processname, int _timelive, int _timeout)
            {
                this.processname = _processname;
                this.timelive = _timelive;
                this.timeout = _timeout;
            }
            public void Startcycle()
            {                
                while (true)
                {
                    Console.WriteLine($"NameofService: {processname} timelive: {timelive} timeout: {timeout}");
                    Console.WriteLine("To stop programm click enter");
                    Process[] processes = Process.GetProcessesByName(processname);
                    foreach (Process proc in processes)
                    {
                        Console.WriteLine($"ID: {proc.Id} Name: {proc.ProcessName} StartTime: {proc.StartTime}");
                        double min = (DateTime.Now-proc.StartTime).TotalMinutes;
                        if ((int)min > timelive)
                        {
                            proc.Kill();
                            Console.WriteLine($"Kill: {proc.Id}");
                        }
                    }                    
                    Thread.Sleep(timeout*60000);
                    Console.Clear();
                }
                
            }

        }
        

        private static bool TryParse(string[] args)
        {
            if (args.Length != 3)
            {
                Console.WriteLine("Please enter name of service(string), timelive max 1000 min and timeout max 1000 min");
                Console.WriteLine("Example: CloseNote.exe notepad 5 1");
                return false;
            }
            else
            {
                try
                {
                    string programname = args[0];
                    if (programname.Length > 15) return Error();
                    int timelive = Int32.Parse(args[1]);
                    if (timelive > 1000) return Error();
                    int timeout = Int32.Parse(args[2]);
                    if (timeout > 1000) return Error();
                    return true;
                }
                catch
                {                    
                    return Error();
                }

            }
        }

        private static bool Error()
        {
            Console.WriteLine("Error data is not correct. Please use example");
            Console.WriteLine("Example: CloseNote.exe notepad 5 1");
            return false;
        }
    }
}
