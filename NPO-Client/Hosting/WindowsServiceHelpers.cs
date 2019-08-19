// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Source on https://github.com/aspnet/Extensions/blob/v3.0.0-preview7.19352.13/src/Hosting/WindowsServices/src/WindowsServiceHelpers.cs

using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;

namespace NPO_Client.Hosting
{
    /// <summary>
    /// Helper methods for Windows Services.
    /// </summary>
    public static class WindowsServiceHelpers
    {
        /// <summary>
        /// Check if the current process is hosted as a Windows Service. 
        /// </summary>
        /// <returns><c>True</c> if the current process is hosted as a Windows Service, otherwise <c>false</c>.</returns>
        public static bool IsWindowsService(string[] args)
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return false;
            }

            // Use of own check because official isn't available in .NET Core 2.2
            return !(Debugger.IsAttached || args.Contains("--console"));
        }
    }
}
