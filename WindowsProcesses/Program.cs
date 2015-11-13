using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Mime;
using System.Text;

namespace WindowsProcesses
{
    class Program
    {
        public static Dictionary<int, Process> dict { get; set; }
        public static List<Process> pList { get; set; }

        static void writeHeader()
        {
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write("  [#]: Process Name");
            Console.SetCursorPosition(40, Console.CursorTop);
            Console.Write("Memory Usage" + Environment.NewLine);
            Console.WriteLine("-----------------------------------------------------------------------------");
        }

        static void displayResults(List<Process> pList)
        {
            int count = 0;
            dict = new Dictionary<int, Process>();
            ConsoleKeyInfo cki;
            Console.Clear();
            writeHeader();

            foreach (Process process in pList)
            {
                string space = "";
                if (count < 10)
                {
                    space = "  ";
                }
                else if (count > 9 && count < 100)
                {
                    space = " ";
                }
                dict.Add(count, process);
                Console.Write(space + "[{0}]: {1}", count, process.ProcessName);
                Console.SetCursorPosition(40, Console.CursorTop);
                if ((process.PrivateMemorySize64 / 1000) > 10000)
                {
                    Console.Write(process.PrivateMemorySize64 / 1000000 + "MB" + Environment.NewLine);
                }
                else
                {
                    Console.Write(process.PrivateMemorySize64 / 1000 + "KB" + Environment.NewLine);
                }
                count++;
                if (count % 20 == 0)
                {
                    Console.WriteLine("Displaying Entries {0} to {1}, press Escape to stop, or any other key to continue.", count - 20, count - 1);
                    cki = Console.ReadKey();
                    if (cki.Key == ConsoleKey.Escape)
                    {
                        break;
                    }
                    writeHeader();
                }
            }
        }
        static void Main(string[] args)
        {
            pList = new List<Process>();
            while (true)
            {
                try
                {
                    pList.Clear();
                    Console.Write("Type Process Name. Use \"all\" for all processes, \"quit\" to quit: ");
                    //Console.TreatControlCAsInput = true;
                    string pname = Console.ReadLine();
                    
                    if (pname == "quit")
                    {
                        Environment.Exit(1);
                    }
                    if (pname == "all")
                    {
                        foreach (Process p in Process.GetProcesses())
                        {
                            pList.Add(p);
                        }
                        displayResults(pList);
                    }
                    else
                    {
                        if (Process.GetProcessesByName(pname).Any())
                        {
                            Console.Clear();
                            writeHeader();
                            foreach (Process p in Process.GetProcessesByName(pname))
                            {
                                pList.Add(p);
                            }
                            if (pList.Count > 0)
                            {
                                displayResults(pList);
                            }
                            
                            
                        }
                    }
                    if (pList.Any())
                    {
                        string op = "Choose Process Number to End or send \"c\" to Cancel ";
                        foreach (KeyValuePair<int, Process> entry in dict)
                        {
                            op += "[" + entry.Key + "] ";
                        }
                        op += "[c] : ";
                        Console.Write(op);
                        string which = Console.ReadLine();
                        if (which == "c" || which == "C")
                        {
                            Console.WriteLine("Canceled. Press any key to return.");
                            Console.ReadKey();
                            Console.Clear();
                            continue;
                        }
                        int whichInt;
                        if (int.TryParse(which, out whichInt))
                        {
                            foreach (KeyValuePair<int, Process> entry in dict)
                            {
                                if (entry.Key == whichInt)
                                {
                                    entry.Value.Kill();
                                    Console.WriteLine("Process Ended: {0}", entry.Value.ProcessName);
                                    Console.WriteLine("Press any key to return.");
                                    Console.ReadKey();
                                    Console.Clear();
                                    continue;
                                }
                            }
                        }
                        Console.WriteLine("Invalid entry.  Press any key to return.");
                        Console.Clear();
                        Console.ReadKey();
                        continue;
                    }
                    else
                    {
                        Console.WriteLine("No processes found that match name \"{0}\".", pname);
                        Console.WriteLine("Press any key to return.");
                        Console.ReadKey();
                        Console.Clear();
                        continue;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.ReadLine();
                }
            }
        }
    }
}
