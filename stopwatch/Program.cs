namespace StopWatch
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                System.Console.WriteLine("Usage: stopwatch.exe <start|stop> <id>");
                System.Environment.ExitCode = 1;
                return;
            }
            string cmd = args[0];
            string id = args[1];

            if (!Regex.IsMatch(id, "[A–Za–z0–9 _-]+"))
            {
                System.Environment.ExitCode = (int)FM.Win32.ErrorCode.ERROR_INVALID_PARAMETER;
                Console.Error.WriteLine("Parameter id is invalid. Allowed chars are: A-Za-z0-9_-");
                return;
            }
            
            string file = string.Format("{0}\\~StopWatch-{1}.tmp", Path.GetTempPath(), id);

            if (args[0] == "start")
            {
                File.WriteAllText(file, System.DateTime.UtcNow.ToString(), Encoding.UTF8);
            }
            else if (args[0] == "stop")
            {
                string text;
                try
                {
                    text = File.ReadAllText(file, Encoding.UTF8);
                }
                catch (System.IO.FileNotFoundException)
                {
                    Console.Error.WriteLine("stopwatch does not seem to have been started for [{0}]", id);
                    System.Environment.ExitCode = (int)FM.Win32.ErrorCode.ERROR_FILE_NOT_FOUND;
                    return;
                }

                var timeStarted = DateTime.Parse(text);
                timeStarted = DateTime.SpecifyKind(timeStarted, DateTimeKind.Utc);
                var duration = DateTime.UtcNow - timeStarted;
                string timeStr;
                if (duration.TotalMinutes >= 60)
                {
                    timeStr = string.Format("{0}h {1}m {2}s", (int)duration.TotalHours, (int)duration.TotalMinutes, duration.Seconds);

                }
                else if (duration.TotalSeconds >= 60)
                {
                    timeStr = string.Format("{0}m {1}s", (int)duration.TotalMinutes, duration.Seconds);

                }
                else
                {
                    timeStr = string.Format("{0}s", duration.Seconds);
                }

                Console.WriteLine("{0} took {1}", id, timeStr);
                File.Delete(file);
            }
            else
            {

            }
        }
    }
}
