using System;

using DevServer.Handlers;

namespace DevServer.Packets
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    internal sealed class PacketAttribute : Attribute
    {
        public HandlerType HandlerType { get; }
        public PacketType PacketType { get; }

        public PacketAttribute(HandlerType handlerType, PacketType packetType)
        {
            HandlerType = handlerType;
            PacketType = packetType;
        }
    }
}