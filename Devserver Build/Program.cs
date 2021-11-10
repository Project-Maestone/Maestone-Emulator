using System;
using System.Reflection;
using System.Diagnostics;

using DevServer.Network;
using DevServer.Packets;
using DevServer.Handlers;

namespace DevServer
{
    internal static class Program
    {
        private static void Main()
        {
            Console.Title = "Project Maestone";




            //ToDO is to change the Charbase data to new one - Check Size here and in Ida Pro .. and see the difference
            var cUse = new DPKUZ_USER_RS_CHAR_BASE_DATA();

            var size = System.Runtime.InteropServices.Marshal.SizeOf(cUse);
            var sizename = cUse.GetType().Name;

            AppDomain.CurrentDomain.UnhandledException += UnhandledException;

              string title = @"
                               ╔═╗╔═╗              ╔╗             
                               ║║╚╝║║             ╔╝╚╗            
                               ║╔╗╔╗║╔══╗ ╔══╗╔══╗╚╗╔╝╔══╗╔═╗ ╔══╗
                               ║║║║║║╚ ╗║ ║╔╗║║══╣ ║║ ║╔╗║║╔╗╗║╔╗║
                               ║║║║║║║╚╝╚╗║║═╣╠══║ ║╚╗║╚╝║║║║║║║═╣
                               ╚╝╚╝╚╝╚═══╝╚══╝╚══╝ ╚═╝╚══╝╚╝╚╝╚══╝                              
                               copyright by Project-Maestone.info
            
                            ";

            Console.WriteLine(title + "\n\n");

            //Log.WriteInfo(title);
            Log.WriteInfo("Starting Maestone Emulator...");

            Log.WriteInfo("Starting servers...");

            Log.WriteWarning("The Size of Packet " + sizename + " is: " + size.ToString() + " Bytes");

            var login = new Server(21001, HandlerType.Login);
            var zone = new Server(21002, HandlerType.Zone);

            foreach (var assemblyType in Assembly.GetExecutingAssembly().GetTypes())
            {
                foreach (var typeMethod in assemblyType.GetMethods())
                {
                    foreach (var packetAttribute in typeMethod.GetCustomAttributes<PacketAttribute>())
                    {
                        switch (packetAttribute.HandlerType)
                        {
                            case HandlerType.Login:
                                login.AddPacketHandler(typeMethod, packetAttribute.PacketType);
                                break;

                            case HandlerType.Zone:
                                zone.AddPacketHandler(typeMethod, packetAttribute.PacketType);
                                break;

                            default:
                                Log.WriteWarning("Packet handler isn't fully implemented yet.");
                                break;
                        }
                    }
                }
            }

            login.Initialize();
            zone.Initialize();

            Log.WriteSuccess("Servers successfully started.");

            Process.GetCurrentProcess().WaitForExit();
        }

        private static void UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (!(e.ExceptionObject is Exception ex))
                return;

            Log.WriteError(ex.Message);

            Debug.WriteLine("[{0}] {1}", DateTime.Now.ToLongTimeString(), ex.ToString().Replace("\n", "\n\t\t"));
        }
    }
}