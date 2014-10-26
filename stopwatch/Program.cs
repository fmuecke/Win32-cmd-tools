namespace StopWatch
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;

    using System.Threading.Tasks;

    internal class Program
    {
        private static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                System.Console.WriteLine("Usage: stopwatch.exe <start|stop> <id> <format>\n");
                System.Console.WriteLine("       id may consist of 'a-zA-Z0-9_-'; \\ will be replaced with '_'. ");
                System.Console.WriteLine("       default format is \"{0}: Time Elapsed {1}\"");
                System.Console.WriteLine("\nSource code available from: http://bitbucket.org/fmuecke/win32-cmd-tools");
                System.Environment.ExitCode = 1;
                return;
            }
            string cmd = args[0];
            string id = args[1];
            string format = "{0}: Time Elapsed {1}";
            if (args.Length >= 3)
            {
                format = args[2].Trim('\"');
            }
            id = id.Replace('\\', '_');

            var rx = new Regex("[a-zA-Z0-9_-]+");
            if (!rx.IsMatch(id))
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
                var durationStr = (DateTime.UtcNow - timeStarted).ToString("hh\\:mm\\:ss");
                //Console.WriteLine("{0}: Time Elapsed {1:hh\\:mm\\:ss}", id, duration);
                try
                {
                    Console.WriteLine(format, id, durationStr);
                }
                catch (System.FormatException e)
                {
                    Console.Error.WriteLine(e.Message);
                    System.Environment.Exit((int)FM.Win32.ErrorCode.ERROR_INVALID_PARAMETER);
                }

                File.Delete(file);
            }
            else
            {
            }
        }
    }
}