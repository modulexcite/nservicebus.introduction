﻿using NServiceBus.Persistence;

namespace SiriusCyberneticsCorp.Facility
{
    using System;
    using System.Runtime.InteropServices;

    using NServiceBus;

    /// <summary>
    /// AsA_Publisher extends AsA_Server and also indicates to the infrastructure that a storage for subscription requests is to be set up.
    /// </summary>
    public class EndpointConfig : IConfigureThisEndpoint, AsA_Server
    {
        const int SwpNosize = 0x0001;

        [DllImport("kernel32.dll", ExactSpelling = true)]
        private static extern IntPtr GetConsoleWindow();

        private static readonly IntPtr ConsolePtr = GetConsoleWindow();

        [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
        public static extern IntPtr SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int y, int cx, int cy, int wFlags);

        public void Customize(BusConfiguration configuration)
        {
            Console.Title = "Facility";

            Console.SetWindowSize(80, 30);
            SetWindowPos(ConsolePtr, 0, 650, 10, 0, 0, SwpNosize);

            configuration.Conventions()
                .DefiningCommandsAs(t => t.Namespace != null && t.Namespace.StartsWith("SiriusCyberneticsCorp.InternalMessages"))
                .DefiningEventsAs(t => t.Namespace != null && t.Namespace.StartsWith("SiriusCyberneticsCorp.Contract"));

            configuration.UseSerialization<JsonSerializer>();
            configuration.UsePersistence<RavenDBPersistence>();
        }
    }
}