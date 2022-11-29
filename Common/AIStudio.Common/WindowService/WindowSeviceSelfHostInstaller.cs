using AIStudio.Common.Quartz.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace AIStudio.Common.WindowService
{
    /// <summary>
    /// 
    /// </summary>
    public class WindowSeviceSelfHostInstaller
    {
        /// <summary>
        /// The service name
        /// </summary>
        static string ServiceName = "";
        /// <summary>
        /// The file path
        /// </summary>
        static string FilePath = "";
        /// <summary>
        /// The username
        /// </summary>
        static string Username = "";
        /// <summary>
        /// The password
        /// </summary>
        static string Password = "";

        /// <summary>
        /// Runs the specified service name.
        /// </summary>
        /// <param name="serviceName">Name of the service.</param>
        /// <param name="filePath">The file path.</param>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        public static void Run(string serviceName, string filePath, string username, string password)
        {
            ServiceName = serviceName;
            FilePath = filePath;
            Username = username;
            Password = password;

            PrintUsage();

            string key;
            while (!string.IsNullOrEmpty(key = Console.ReadLine()))
            {
                bool runapp = false;
                switch (key)
                {
                    case "/status":
                        PrintStatus(ServiceName);
                        break;
                    case "/stop":
                        StopService(ServiceName);
                        break;
                    case "/start":
                        StartService(ServiceName, out runapp);
                        break;
                    case "/install":
                        InstallService(ServiceName);
                        break;
                    case "/uninstall":
                        UninstallService(ServiceName);
                        break;
                }

                if (runapp)
                    break;
            }
        }

        /// <summary>
        /// Installs the service.
        /// </summary>
        /// <param name="name">The name.</param>
        static void InstallService(string name)
        {
            var createdNew = true;
            using (var mutex = new Mutex(true, name, out createdNew))
            {
                if (createdNew)
                {
                    mutex.WaitOne();

                    Process p = new Process();
                    p.StartInfo.FileName = "cmd.exe";
                    p.StartInfo.UseShellExecute = false;
                    p.StartInfo.RedirectStandardInput = true;
                    p.StartInfo.RedirectStandardOutput = true;
                    p.StartInfo.RedirectStandardError = true;
                    p.StartInfo.CreateNoWindow = true;

                    p.Start();

                    p.StandardInput.WriteLine($@"sc create {name} BinPath={FilePath}");
                    p.StandardInput.WriteLine($@"sc config {name} start=auto  ");
                    if (!string.IsNullOrEmpty(Username))
                    {
                        p.StandardInput.WriteLine($"sc config {name} obj= {Username} password= {Password}");
                    }
                    p.StandardInput.WriteLine($"sc description {name}  服务描述");
                    p.StandardInput.WriteLine($"sc start {name}");
                    p.StandardInput.WriteLine($"exit");
                    p.StandardInput.AutoFlush = true;

                   StreamReader sc = p.StandardOutput;
                    string output = p.StandardOutput.ReadToEnd();

                    Console.WriteLine(output);
                    p.Close();

                    Thread.Sleep(1000);
                    Console.Error.WriteLine("Service " + name + " installed");
                }
                else
                {
                    Console.Error.WriteLine("Service " + name + " is currently installed.");
                }
            }
        }
        /// <summary>
        /// Uninstalls the service.
        /// </summary>
        /// <param name="name">The name.</param>
        static void UninstallService(string name)
        {
            var createdNew = true;
            using (var mutex = new Mutex(true, name, out createdNew))
            {
                if (createdNew)
                {
                    mutex.WaitOne();

                    Process p = new Process();
                    p.StartInfo.FileName = "cmd.exe";
                    p.StartInfo.UseShellExecute = false;
                    p.StartInfo.RedirectStandardInput = true;
                    p.StartInfo.RedirectStandardOutput = true;
                    p.StartInfo.RedirectStandardError = true;
                    p.StartInfo.CreateNoWindow = true;

                    p.Start();

                    p.StandardInput.WriteLine($@"sc stop {name}");
                    p.StandardInput.WriteLine($@"sc delete {name}");
                    p.StandardInput.WriteLine($"exit");
                    p.StandardInput.AutoFlush = true;

                    StreamReader sc = p.StandardOutput;
                    string output = p.StandardOutput.ReadToEnd();

                    Console.WriteLine(output);
                    p.Close();

                    Thread.Sleep(1000);
                    Console.Error.WriteLine("Service " + name + " uninstalled");
                }
                else
                {
                    Console.Error.WriteLine("Service " + name + " is currently uninstalled.");
                }
            }
        }
        /// <summary>
        /// Starts the service.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="runapp">if set to <c>true</c> [runapp].</param>
        static void StartService(string name, out bool runapp)
        {
            runapp = false;
            bool isInstalled = ServiceInstaller.IsInstalled(name);
            if (isInstalled)
            {
                ServiceInstaller.StartService(name);

                Thread.Sleep(1000);

                Console.Error.WriteLine("Service " + name + " start");
                PrintStatus(name);
            }
            else
            {
                var createdNew = true;
                using (var mutex = new Mutex(true, name, out createdNew))
                {
                    if (createdNew)
                    {
                        mutex.WaitOne();

                        runapp = true;

                        Thread.Sleep(1000);
                        Console.Error.WriteLine("Service " + name + " start");
                    }
                    else
                    {
                        Console.Error.WriteLine("Service " + name + " is running in other progress.");
                    }
                }
            }
        }

        /// <summary>
        /// Stops the service.
        /// </summary>
        /// <param name="name">The name.</param>
        static void StopService(string name)
        {
            bool isInstalled = ServiceInstaller.IsInstalled(name);
            if (isInstalled)
            {
                ServiceInstaller.StopService(name);

                Thread.Sleep(1000);
                Console.Error.WriteLine("Service " + name + " stop");
                PrintStatus(name);
            }
          
        }

        /// <summary>
        /// Prints the status.
        /// </summary>
        /// <param name="name">The name.</param>
        static void PrintStatus(string name)
        {
            var isInstalled = ServiceInstaller.IsInstalled(name);
            if (isInstalled)
            {
                Process p = new Process();
                p.StartInfo.FileName = "cmd.exe";
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.RedirectStandardInput = true;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.RedirectStandardError = true;
                p.StartInfo.CreateNoWindow = true;

                p.Start();

                p.StandardInput.WriteLine($@"sc query {name} &exit");
                p.StandardInput.AutoFlush = true;

                StreamReader sc = p.StandardOutput;
                string output = p.StandardOutput.ReadToEnd();

                Console.WriteLine(output);
                p.Close();
            }
        }


        /// <summary>
        /// Prints the usage.
        /// </summary>
        static void PrintUsage()
        {
            var t = Console.Error;
            t.Write("Usage: " + FilePath);
            t.WriteLine(" /start | /stop | /install | /uninstall | /status");
            t.WriteLine();
            t.WriteLine("   /start      Starts the service, if it's not already running. When not installed, this runs in console mode and exit install mode.");
            t.WriteLine("   /stop       Stops the service, if it's running. This will stop the installed service");
            t.WriteLine("   /install    Installs the service, if not installed so that it may run in Windows service mode.");
            t.WriteLine("   /uninstall  Uninstalls the service, if installed, so that it will not run in Windows service mode.");
            t.WriteLine("   /status     Reports if the service is installed and/or running.");
            t.WriteLine();

        }
    }
}
