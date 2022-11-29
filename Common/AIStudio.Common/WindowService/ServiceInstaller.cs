using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AIStudio.Common.WindowService
{
    #region ServiceInstaller and support (adapted from https://stackoverflow.com/questions/358700/how-to-install-a-windows-service-programmatically-in-c)
    /// <summary>
    /// 
    /// </summary>
    public static class ServiceInstaller
    {
        /// <summary>
        /// The standard rights required
        /// </summary>
        private const int STANDARD_RIGHTS_REQUIRED = 0xF0000;
        /// <summary>
        /// The service wi N32 own process
        /// </summary>
        private const int SERVICE_WIN32_OWN_PROCESS = 0x00000010;

        /// <summary>
        /// 
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        private class SERVICE_STATUS
        {
            /// <summary>
            /// The dw service type
            /// </summary>
            public int dwServiceType = 0;
            /// <summary>
            /// The dw current state
            /// </summary>
            public _ServiceState dwCurrentState = 0;
            /// <summary>
            /// The dw controls accepted
            /// </summary>
            public int dwControlsAccepted = 0;
            /// <summary>
            /// The dw win32 exit code
            /// </summary>
            public int dwWin32ExitCode = 0;
            /// <summary>
            /// The dw service specific exit code
            /// </summary>
            public int dwServiceSpecificExitCode = 0;
            /// <summary>
            /// The dw check point
            /// </summary>
            public int dwCheckPoint = 0;
            /// <summary>
            /// The dw wait hint
            /// </summary>
            public int dwWaitHint = 0;
        }

        #region OpenSCManager
        /// <summary>
        /// Opens the sc manager.
        /// </summary>
        /// <param name="machineName">Name of the machine.</param>
        /// <param name="databaseName">Name of the database.</param>
        /// <param name="dwDesiredAccess">The dw desired access.</param>
        /// <returns></returns>
        [DllImport("advapi32.dll", EntryPoint = "OpenSCManagerW", ExactSpelling = true, CharSet = CharSet.Unicode, SetLastError = true)]
        static extern IntPtr OpenSCManager(string machineName, string databaseName, _ScmAccessRights dwDesiredAccess);
        #endregion

        #region OpenService
        /// <summary>
        /// Opens the service.
        /// </summary>
        /// <param name="hSCManager">The h sc manager.</param>
        /// <param name="lpServiceName">Name of the lp service.</param>
        /// <param name="dwDesiredAccess">The dw desired access.</param>
        /// <returns></returns>
        [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern IntPtr OpenService(IntPtr hSCManager, string lpServiceName, _ServiceAccessRights dwDesiredAccess);
        #endregion

        #region CreateService
        /// <summary>
        /// Creates the service.
        /// </summary>
        /// <param name="hSCManager">The h sc manager.</param>
        /// <param name="lpServiceName">Name of the lp service.</param>
        /// <param name="lpDisplayName">Display name of the lp.</param>
        /// <param name="dwDesiredAccess">The dw desired access.</param>
        /// <param name="dwServiceType">Type of the dw service.</param>
        /// <param name="dwStartType">Start type of the dw.</param>
        /// <param name="dwErrorControl">The dw error control.</param>
        /// <param name="lpBinaryPathName">Name of the lp binary path.</param>
        /// <param name="lpLoadOrderGroup">The lp load order group.</param>
        /// <param name="lpdwTagId">The LPDW tag identifier.</param>
        /// <param name="lpDependencies">The lp dependencies.</param>
        /// <param name="lp">The lp.</param>
        /// <param name="lpPassword">The lp password.</param>
        /// <returns></returns>
        [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern IntPtr CreateService(IntPtr hSCManager, string lpServiceName, string lpDisplayName, _ServiceAccessRights dwDesiredAccess, int dwServiceType, _ServiceBootFlag dwStartType, _ServiceError dwErrorControl, string lpBinaryPathName, string lpLoadOrderGroup, IntPtr lpdwTagId, string lpDependencies, string lp, string lpPassword);
        #endregion

        #region CloseServiceHandle
        /// <summary>
        /// Closes the service handle.
        /// </summary>
        /// <param name="hSCObject">The h sc object.</param>
        /// <returns></returns>
        [DllImport("advapi32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool CloseServiceHandle(IntPtr hSCObject);
        #endregion

        #region QueryServiceStatus
        /// <summary>
        /// Queries the service status.
        /// </summary>
        /// <param name="hService">The h service.</param>
        /// <param name="lpServiceStatus">The lp service status.</param>
        /// <returns></returns>
        [DllImport("advapi32.dll")]
        private static extern int QueryServiceStatus(IntPtr hService, SERVICE_STATUS lpServiceStatus);
        #endregion

        #region DeleteService
        /// <summary>
        /// Deletes the service.
        /// </summary>
        /// <param name="hService">The h service.</param>
        /// <returns></returns>
        [DllImport("advapi32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool DeleteService(IntPtr hService);
        #endregion

        #region ControlService
        /// <summary>
        /// Controls the service.
        /// </summary>
        /// <param name="hService">The h service.</param>
        /// <param name="dwControl">The dw control.</param>
        /// <param name="lpServiceStatus">The lp service status.</param>
        /// <returns></returns>
        [DllImport("advapi32.dll")]
        private static extern int ControlService(IntPtr hService, _ServiceControl dwControl, SERVICE_STATUS lpServiceStatus);
        #endregion

        #region StartService
        /// <summary>
        /// Starts the service.
        /// </summary>
        /// <param name="hService">The h service.</param>
        /// <param name="dwNumServiceArgs">The dw number service arguments.</param>
        /// <param name="lpServiceArgVectors">The lp service argument vectors.</param>
        /// <returns></returns>
        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern int StartService(IntPtr hService, int dwNumServiceArgs, int lpServiceArgVectors);
        #endregion

        /// <summary>
        /// Uninstalls the specified service name.
        /// </summary>
        /// <param name="serviceName">Name of the service.</param>
        /// <exception cref="System.ApplicationException">
        /// Service not installed.
        /// or
        /// Could not delete service " + Marshal.GetLastWin32Error()
        /// </exception>
        public static void Uninstall(string serviceName)
        {
            IntPtr scm = _OpenSCManager(_ScmAccessRights.AllAccess);

            try
            {
                IntPtr service = OpenService(scm, serviceName, _ServiceAccessRights.AllAccess);
                if (service == IntPtr.Zero)
                    throw new ApplicationException("Service not installed.");

                try
                {
                    _StopService(service);
                    if (!DeleteService(service))
                        throw new ApplicationException("Could not delete service " + Marshal.GetLastWin32Error());
                }
                finally
                {
                    CloseServiceHandle(service);
                }
            }
            finally
            {
                CloseServiceHandle(scm);
            }
        }

        /// <summary>
        /// Determines whether the specified service name is installed.
        /// </summary>
        /// <param name="serviceName">Name of the service.</param>
        /// <returns>
        ///   <c>true</c> if the specified service name is installed; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsInstalled(string serviceName)
        {
            IntPtr scm = _OpenSCManager(_ScmAccessRights.Connect);

            try
            {
                IntPtr service = OpenService(scm, serviceName, _ServiceAccessRights.QueryStatus);

                if (service == IntPtr.Zero)
                    return false;

                CloseServiceHandle(service);
                return true;
            }
            finally
            {
                CloseServiceHandle(scm);
            }
        }

        /// <summary>
        /// Installs the specified service name.
        /// </summary>
        /// <param name="serviceName">Name of the service.</param>
        /// <param name="displayName">The display name.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <exception cref="System.ApplicationException">Failed to install service.</exception>
        public static void Install(string serviceName, string displayName, string fileName)
        {
            IntPtr scm = _OpenSCManager(_ScmAccessRights.AllAccess);

            try
            {
                IntPtr service = OpenService(scm, serviceName, _ServiceAccessRights.AllAccess);

                if (service == IntPtr.Zero)
                    service = CreateService(scm, serviceName, displayName, _ServiceAccessRights.AllAccess, SERVICE_WIN32_OWN_PROCESS, _ServiceBootFlag.AutoStart, _ServiceError.Normal, fileName, null, IntPtr.Zero, null, null, null);

                if (service == IntPtr.Zero)
                    throw new ApplicationException("Failed to install service.");

                /*try
                {
                    StartService(service);
                }
                finally
                {
                    CloseServiceHandle(service);
                }*/
            }
            finally
            {
                CloseServiceHandle(scm);
            }
        }

        /// <summary>
        /// Starts the service.
        /// </summary>
        /// <param name="serviceName">Name of the service.</param>
        /// <exception cref="System.ApplicationException">Could not open service.</exception>
        public static void StartService(string serviceName)
        {
            IntPtr scm = _OpenSCManager(_ScmAccessRights.Connect);

            try
            {
                IntPtr service = OpenService(scm, serviceName, _ServiceAccessRights.QueryStatus | _ServiceAccessRights.Start);
                if (service == IntPtr.Zero)
                    throw new ApplicationException("Could not open service.");

                try
                {
                    _StartService(service);
                }
                finally
                {
                    CloseServiceHandle(service);
                }
            }
            finally
            {
                CloseServiceHandle(scm);
            }
        }

        /// <summary>
        /// Stops the service.
        /// </summary>
        /// <param name="serviceName">Name of the service.</param>
        /// <exception cref="System.ApplicationException">Could not open service.</exception>
        public static void StopService(string serviceName)
        {
            IntPtr scm = _OpenSCManager(_ScmAccessRights.Connect);

            try
            {
                IntPtr service = OpenService(scm, serviceName, _ServiceAccessRights.QueryStatus | _ServiceAccessRights.Stop);
                if (service == IntPtr.Zero)
                    throw new ApplicationException("Could not open service.");

                try
                {
                    _StopService(service);
                }
                finally
                {
                    CloseServiceHandle(service);
                }
            }
            finally
            {
                CloseServiceHandle(scm);
            }
        }

        /// <summary>
        /// Starts the service.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <exception cref="System.ApplicationException">Unable to start service</exception>
        static void _StartService(IntPtr service)
        {
            SERVICE_STATUS status = new SERVICE_STATUS();
            StartService(service, 0, 0);
            var changedStatus = _WaitForServiceStatus(service, _ServiceState.StartPending, _ServiceState.Running);
            if (!changedStatus)
                throw new ApplicationException("Unable to start service");
        }

        /// <summary>
        /// Stops the service.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <exception cref="System.ApplicationException">Unable to stop service</exception>
        static void _StopService(IntPtr service)
        {
            SERVICE_STATUS status = new SERVICE_STATUS();
            ControlService(service, _ServiceControl.Stop, status);
            var changedStatus = _WaitForServiceStatus(service, _ServiceState.StopPending, _ServiceState.Stopped);
            if (!changedStatus)
                throw new ApplicationException("Unable to stop service");
        }

        /// <summary>
        /// Gets the service status.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <returns></returns>
        /// <exception cref="System.ApplicationException">Failed to query service status.</exception>
        static _ServiceState _GetServiceStatus(IntPtr service)
        {
            SERVICE_STATUS status = new SERVICE_STATUS();

            if (QueryServiceStatus(service, status) == 0)
                throw new ApplicationException("Failed to query service status.");

            return status.dwCurrentState;
        }

        /// <summary>
        /// Waits for service status.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="waitStatus">The wait status.</param>
        /// <param name="desiredStatus">The desired status.</param>
        /// <returns></returns>
        static bool _WaitForServiceStatus(IntPtr service, _ServiceState waitStatus, _ServiceState desiredStatus)
        {
            SERVICE_STATUS status = new SERVICE_STATUS();

            QueryServiceStatus(service, status);
            if (status.dwCurrentState == desiredStatus) return true;

            int dwStartTickCount = Environment.TickCount;
            int dwOldCheckPoint = status.dwCheckPoint;

            while (status.dwCurrentState == waitStatus)
            {
                // Do not wait longer than the wait hint. A good interval is
                // one tenth the wait hint, but no less than 1 second and no
                // more than 10 seconds.

                int dwWaitTime = status.dwWaitHint / 10;

                if (dwWaitTime < 1000) dwWaitTime = 1000;
                else if (dwWaitTime > 10000) dwWaitTime = 10000;

                Thread.Sleep(dwWaitTime);

                // Check the status again.

                if (QueryServiceStatus(service, status) == 0) break;

                if (status.dwCheckPoint > dwOldCheckPoint)
                {
                    // The service is making progress.
                    dwStartTickCount = Environment.TickCount;
                    dwOldCheckPoint = status.dwCheckPoint;
                }
                else
                {
                    if (Environment.TickCount - dwStartTickCount > status.dwWaitHint)
                    {
                        // No progress made within the wait hint
                        break;
                    }
                }
            }
            return (status.dwCurrentState == desiredStatus);
        }

        /// <summary>
        /// Opens the sc manager.
        /// </summary>
        /// <param name="rights">The rights.</param>
        /// <returns></returns>
        /// <exception cref="System.ApplicationException">Could not connect to service control manager.</exception>
        static IntPtr _OpenSCManager(_ScmAccessRights rights)
        {
            IntPtr scm = OpenSCManager(null, null, rights);
            if (scm == IntPtr.Zero)
                throw new ApplicationException("Could not connect to service control manager.");

            return scm;
        }



        /// <summary>
        /// 
        /// </summary>
        private enum _ServiceState
        {
            /// <summary>
            /// The unknown
            /// </summary>
            Unknown = -1, // The state cannot be (has not been) retrieved.
            /// <summary>
            /// The not found
            /// </summary>
            NotFound = 0, // The service is not known on the host server.
            /// <summary>
            /// The stopped
            /// </summary>
            Stopped = 1,
            /// <summary>
            /// The start pending
            /// </summary>
            StartPending = 2,
            /// <summary>
            /// The stop pending
            /// </summary>
            StopPending = 3,
            /// <summary>
            /// The running
            /// </summary>
            Running = 4,
            /// <summary>
            /// The continue pending
            /// </summary>
            ContinuePending = 5,
            /// <summary>
            /// The pause pending
            /// </summary>
            PausePending = 6,
            /// <summary>
            /// The paused
            /// </summary>
            Paused = 7
        }

        /// <summary>
        /// 
        /// </summary>
        [Flags]
        private enum _ScmAccessRights
        {
            /// <summary>
            /// The connect
            /// </summary>
            Connect = 0x0001,
            /// <summary>
            /// The create service
            /// </summary>
            CreateService = 0x0002,
            /// <summary>
            /// The enumerate service
            /// </summary>
            EnumerateService = 0x0004,
            /// <summary>
            /// The lock
            /// </summary>
            Lock = 0x0008,
            /// <summary>
            /// The query lock status
            /// </summary>
            QueryLockStatus = 0x0010,
            /// <summary>
            /// The modify boot configuration
            /// </summary>
            ModifyBootConfig = 0x0020,
            /// <summary>
            /// The standard rights required
            /// </summary>
            StandardRightsRequired = 0xF0000,
            /// <summary>
            /// All access
            /// </summary>
            AllAccess = (StandardRightsRequired | Connect | CreateService |
                         EnumerateService | Lock | QueryLockStatus | ModifyBootConfig)
        }

        /// <summary>
        /// 
        /// </summary>
        [Flags]
        private enum _ServiceAccessRights
        {
            /// <summary>
            /// The query configuration
            /// </summary>
            QueryConfig = 0x1,
            /// <summary>
            /// The change configuration
            /// </summary>
            ChangeConfig = 0x2,
            /// <summary>
            /// The query status
            /// </summary>
            QueryStatus = 0x4,
            /// <summary>
            /// The enumerate dependants
            /// </summary>
            EnumerateDependants = 0x8,
            /// <summary>
            /// The start
            /// </summary>
            Start = 0x10,
            /// <summary>
            /// The stop
            /// </summary>
            Stop = 0x20,
            /// <summary>
            /// The pause continue
            /// </summary>
            PauseContinue = 0x40,
            /// <summary>
            /// The interrogate
            /// </summary>
            Interrogate = 0x80,
            /// <summary>
            /// The user defined control
            /// </summary>
            UserDefinedControl = 0x100,
            /// <summary>
            /// The delete
            /// </summary>
            Delete = 0x00010000,
            /// <summary>
            /// The standard rights required
            /// </summary>
            StandardRightsRequired = 0xF0000,
            /// <summary>
            /// All access
            /// </summary>
            AllAccess = (StandardRightsRequired | QueryConfig | ChangeConfig |
                         QueryStatus | EnumerateDependants | Start | Stop | PauseContinue |
                         Interrogate | UserDefinedControl)
        }

        /// <summary>
        /// 
        /// </summary>
        private enum _ServiceBootFlag
        {
            /// <summary>
            /// The start
            /// </summary>
            Start = 0x00000000,
            /// <summary>
            /// The system start
            /// </summary>
            SystemStart = 0x00000001,
            /// <summary>
            /// The automatic start
            /// </summary>
            AutoStart = 0x00000002,
            /// <summary>
            /// The demand start
            /// </summary>
            DemandStart = 0x00000003,
            /// <summary>
            /// The disabled
            /// </summary>
            Disabled = 0x00000004
        }

        /// <summary>
        /// 
        /// </summary>
        private enum _ServiceControl
        {
            /// <summary>
            /// The stop
            /// </summary>
            Stop = 0x00000001,
            /// <summary>
            /// The pause
            /// </summary>
            Pause = 0x00000002,
            /// <summary>
            /// The continue
            /// </summary>
            Continue = 0x00000003,
            /// <summary>
            /// The interrogate
            /// </summary>
            Interrogate = 0x00000004,
            /// <summary>
            /// Shuts down this instance.
            /// </summary>
            Shutdown = 0x00000005,
            /// <summary>
            /// The parameter change
            /// </summary>
            ParamChange = 0x00000006,
            /// <summary>
            /// The net bind add
            /// </summary>
            NetBindAdd = 0x00000007,
            /// <summary>
            /// The net bind remove
            /// </summary>
            NetBindRemove = 0x00000008,
            /// <summary>
            /// The net bind enable
            /// </summary>
            NetBindEnable = 0x00000009,
            /// <summary>
            /// The net bind disable
            /// </summary>
            NetBindDisable = 0x0000000A
        }

        /// <summary>
        /// 
        /// </summary>
        private enum _ServiceError
        {
            /// <summary>
            /// The ignore
            /// </summary>
            Ignore = 0x00000000,
            /// <summary>
            /// The normal
            /// </summary>
            Normal = 0x00000001,
            /// <summary>
            /// The severe
            /// </summary>
            Severe = 0x00000002,
            /// <summary>
            /// The critical
            /// </summary>
            Critical = 0x00000003
        }
        #endregion
    }
}
