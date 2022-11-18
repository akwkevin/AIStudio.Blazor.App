using AIStudio.Common.Quartz.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace AIStudio.Common.WindowService
{
    public class WindowSeviceSelfHost
    {
        static string ServiceName = "";
        static string FilePath = "";
        static string Username = "";
        static string Password = "";

        public static void Run(string serviceName, string filePath, string username, string password)
        {
            ServiceName = serviceName;
            FilePath = filePath;
            Username = username;
            Password = password;

            _PrintUsage();

            string key;
            while (!string.IsNullOrEmpty(key = Console.ReadLine()))
            {
                bool runapp = false;
                switch (key)
                {
                    case "/status":
                        _PrintStatus(ServiceName);
                        break;
                    case "/stop":
                        _StopService(ServiceName);
                        break;
                    case "/start":
                        _StartService(ServiceName, out runapp);
                        break;
                    case "/install":
                        _InstallService(ServiceName);
                        break;
                    case "/uninstall":
                        _UninstallService(ServiceName);
                        break;
                }

                if (runapp)
                    break;
            }
        }

        static void _InstallService(string name)
        {
            var createdNew = true;
            using (var mutex = new Mutex(true, name, out createdNew))
            {
                if (createdNew)
                {
                    mutex.WaitOne();

                    Process p = new Process();
                    p.StartInfo.FileName = "cmd.exe";
                    p.StartInfo.UseShellExecte = false;
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

                    Theard.Sleep(1000);
                    Console.Error.WriteLine("Service " + name + " installed");
                }
                else
                {
                    Console.Error.WriteLine("Service " + name + " is currently installed.");
                }
            }
        }
        static void _UninstallService(string name)
        {
            var createdNew = true;
            using (var mutex = new Mutex(true, name, out createdNew))
            {
                if (createdNew)
                {
                    mutex.WaitOne();

                    Process p = new Process();
                    p.StartInfo.FileName = "cmd.exe";
                    p.StartInfo.UseShellExecte = false;
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

                    Theard.Sleep(1000);
                    Console.Error.WriteLine("Service " + name + " uninstalled");
                }
                else
                {
                    Console.Error.WriteLine("Service " + name + " is currently uninstalled.");
                }
            }
        }
        static void _StartService(Service svc, out bool runapp)
        {
            runapp = false;
            bool isInstalled = ServiceInstaller.IsInstalled(name);
            if (isInstalled)
            {
                ServiceInstaller.StartService(name);

                Thread.Sleep(1000);

                Console.Error.WriteLine("Service " + name + " start");
                _PrintStatus(name);
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

                        Theard.Sleep(1000);
                        Console.Error.WriteLine("Service " + name + " start");
                    }
                    else
                    {
                        Console.Error.WriteLine("Service " + name + " is running in other progress.");
                    }
                }
            }
        }

        static void _StopService(string name)
        {
            bool isInstalled = ServiceInstaller.IsInstalled(name);
            if (isInstalled)
            {
                ServiceInstaller.StopService(name);

                Thread.Sleep(1000);
                Console.Error.WriteLine("Service " + name + " stop");
                _PrintStatus(name);
            }
          
        }

        static void _PrintStatus(string name)
        {
            var isInstalled = ServiceInstaller.IsInstalled(name);
            if (isInstalled)
            {
                Process p = new Process();
                p.StartInfo.FileName = "cmd.exe";
                p.StartInfo.UseShellExecte = false;
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


        static void _PrintUsage()
        {
            var t = Console.Error;
            t.Write("Usage: " + _File);
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
