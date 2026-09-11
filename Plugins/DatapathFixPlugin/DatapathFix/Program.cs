using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace DatapathFix
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string currentPath = Assembly.GetExecutingAssembly().Location;
            string origPath = currentPath.Replace(".exe", ".orig.exe");

            if (File.Exists("tmp") && File.Exists(origPath))
            {
                string dataPathArg = File.ReadAllText("tmp");

                if (args.Length == 0)
                {
                    try
                    {
                        File.Move(currentPath, currentPath.Replace(".exe", ".old"));
                        File.Move(origPath, currentPath);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Error While Launching: Unable to Move Files");
                        Console.WriteLine(e);
                        Console.WriteLine("Restarting as Administrator...");
                        AnyKeyToContinue();

                        Process.Start(new ProcessStartInfo
                        {
                            FileName = currentPath,
                            UseShellExecute = true,
                            Verb = "runas"
                        });
                        return;
                    }

                    string parPath = origPath.Replace(".exe", ".par");
                    if (File.Exists(parPath))
                    {
                        try
                        {
                            File.Delete(parPath);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Error While Launching: Unable to Delete File");
                            Console.WriteLine(e);
                            Console.WriteLine("Restarting as Administrator...");
                            AnyKeyToContinue();

                            Process.Start(new ProcessStartInfo
                            {
                                FileName = currentPath,
                                UseShellExecute = true,
                                Verb = "runas"
                            });
                            return;
                        }
                    }

                    Console.WriteLine($"Starting '{Path.GetFileName(currentPath)}' with mods");
                    AnyKeyToContinue();

                    try
                    {
                        ProcessStartInfo startInfo = new ProcessStartInfo
                        {
                            FileName = currentPath,
                            WorkingDirectory = Environment.CurrentDirectory,
                            Arguments = dataPathArg,
                            UseShellExecute = false
                        };

                        string dataDirPath = "tmp_datadir";
                        if (File.Exists(dataDirPath))
                            startInfo.EnvironmentVariables["GAME_DATA_DIR"] = File.ReadAllText(dataDirPath);

                        Process.Start(startInfo);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Error While Launching:");
                        Console.WriteLine(e);
                        AnyKeyToContinue();
                    }
                }

                else
                {
                    Console.WriteLine("Arguments present, assuming it was Frosty attempting to launch");
                    Console.WriteLine($"Starting '{Path.GetFileName(origPath)}' to prompt the platform to launch the game");
                    AnyKeyToContinue();

                    try
                    {
                        Process.Start(new ProcessStartInfo
                        {
                            FileName = origPath,
                            WorkingDirectory = Environment.CurrentDirectory,
                            UseShellExecute = false
                        });
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Error While Launching:");
                        Console.WriteLine(e);
                        AnyKeyToContinue();
                    }
                }
            }
            else
            {
                if (!File.Exists("tmp"))
                    Console.WriteLine("Error: 'tmp' does not exist");
                if (!File.Exists(origPath))
                    Console.WriteLine($"Error: '{Path.GetFileName(origPath)}' does not exist");
                AnyKeyToContinue();
            }

            void AnyKeyToContinue()
            {
                #if DEBUG
                    Console.WriteLine("");
                    Console.Write("Press Any Key to Continue...");
                    Console.ReadKey();
                #endif
            }
        }
    }
}
