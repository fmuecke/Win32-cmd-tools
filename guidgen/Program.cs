using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace guidgen
{
    internal class Program
    {
        [STAThreadAttribute]
        private static void Main(string[] args)
        {
            Guid guid = Guid.NewGuid();
            string s = guid.ToString().ToUpperInvariant();
            Console.Write(s);
            Clipboard.SetText(s);
        }
    }
}