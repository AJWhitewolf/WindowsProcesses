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
        static void Main(string[] args)
        {
            while (true)
            {
                try
                {
                    Console.Write("Type Process Name. Use \"all\" for all processes, \"quit\" to quit: ");
                    //Console.TreatControlCAsInput = true;
                    string pname = Console.ReadLine();
                    Dictionary<int, Process> dict = new Dictionary<int, Process>();
                    ConsoleKeyInfo cki;
                    int count = 0;
                    if (pname == "quit")
                    {
                        Environment.Exit(1);
                    }
                    if (pname == "all")
                    {
                        foreach (Process process in Process.GetProcesses())
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
                            Console.WriteLine(space+"[{0}]: {1}", count, process.ProcessName);
                            count++;
                            if (count%20 == 0)
                            {
                                Console.WriteLine("Displaying Entries {0} to {1}, press Escape to stop, or any other key to continue.", count-20, count-1);
                                cki = Console.ReadKey();
                                if (cki.Key == ConsoleKey.Escape)
                                {
                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        foreach (Process process in Process.GetProcessesByName(pname))
                        {
                            dict.Add(count, process);
                            Console.WriteLine("[{0}]: {1}", count, process.ProcessName);
                            count++;
                        }
                    }
                    if (count > 0)
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
                        int whichInt = int.Parse(which);
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
                        Console.WriteLine("Invalid entry.  Press any key to return.");
                        Console.Clear();
                        Console.ReadKey();
                        continue;
                    }
                    else
                    {
                        Console.WriteLine("No processes found that match name \"{0}\".", pname);
                        Console.WriteLine("Press any key to return.");
                        Console.Clear();
                        Console.ReadKey();
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
