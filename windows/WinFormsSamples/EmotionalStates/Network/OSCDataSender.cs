using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Neuro;

namespace EmotionalStates.Network
{
    public class OSCDataSender
    {
        public static void SendEmotionData(EmotionalState state, IPEndPoint destination)
        {
            using (var udpSender = new UdpClient())
            {
                var relaxPacket = CreatePacket("Relax", state.Relax);
                udpSender.Send(relaxPacket, relaxPacket.Length, destination);

                var meditationPacket = CreatePacket("Meditation", state.Meditation);
                udpSender.Send(meditationPacket, meditationPacket.Length, destination);

                var attentionPacket = CreatePacket("Attention", state.Attention);
                udpSender.Send(attentionPacket, attentionPacket.Length, destination);

                var stressPacket = CreatePacket("Stress", state.Stress);
                udpSender.Send(stressPacket, stressPacket.Length, destination);
            }
        }

        private static byte[] CreatePacket(string index, float value)
        {
            using (var packetMemoryStream = new MemoryStream())
            {
                var packetAddress = $"/{index}";
                var addressBytes = Encoding.ASCII.GetBytes(packetAddress);
                var multipleOf4Size = NearestMultipleOf4(addressBytes.Length + 1);
                try
                {
                    packetMemoryStream.Write(addressBytes, 0, addressBytes.Length);
                    packetMemoryStream.WriteByte(0);
                    while (multipleOf4Size > addressBytes.Length + 1)
                    {
                        packetMemoryStream.WriteByte(0);
                        --multipleOf4Size;
                    }

                    var valueHeader = ",f";
                    var valueHeaderBytes = Encoding.ASCII.GetBytes(valueHeader);
                    packetMemoryStream.Write(valueHeaderBytes, 0, valueHeaderBytes.Length);
                    packetMemoryStream.WriteByte(0);
                    packetMemoryStream.WriteByte(0);
                    var valueBytes = BitConverter.GetBytes(value);
                    Array.Reverse(valueBytes);
                    packetMemoryStream.Write(valueBytes, 0, valueBytes.Length);
                }
                catch (IOException e)
                {
                    throw new IOException(e.Message);
                }

                return packetMemoryStream.ToArray();
            }
        }

        private static int NearestMultipleOf4(int value)
        {
            if (value % 4 == 0)
                return value;

            return (value / 4 + 1) * 4;
        }
    }
}