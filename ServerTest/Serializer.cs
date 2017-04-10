namespace ServerTest
{
    using ProtoBuf;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net.Sockets;

    public static class Serializer
    {
        private static DateTime lastStatisticUpdateTime;
        public const int SizeOfMessageLength = 4;
        private static Dictionary<Type, StatisticsHelper> statistics = new Dictionary<Type, StatisticsHelper>();
        private static double statisticsLogUpdateTime = -1.0;

        public static NetworkData Deserialize(MemoryStream ms)
        {
            NetworkData data = null;
            ms.Position = 0L;
            try
            {
                data = ProtoBuf.Serializer.Deserialize<NetworkDataTransportWrapper>(ms).data;
            }
            catch (Exception exception)
            {
                object[] values = new object[] { "Failed to deserialize communication data", exception.Message, exception.StackTrace };
                Console.WriteLine(exception.Message + "\n" + exception.StackTrace);
            }
            return data;
        }

        public static NetworkData ReceiveData(Stream str)
        {
            byte[] buffer = new byte[4];
            int num = 0;
            int offset = 0;
            do
            {
                num = str.Read(buffer, offset, buffer.Length - offset);
                if (num == 0)
                {
                    throw new ZeroDataException("Received zero data message.");
                }
                offset += num;
            }
            while (offset < buffer.Length);
            byte[] buffer2 = new byte[BitConverter.ToUInt32(buffer, 0)];
            offset = 0;
            do
            {
                num = str.Read(buffer2, offset, buffer2.Length - offset);
                if (num == 0)
                {
                    throw new ZeroDataException("Received zero data message.");
                }
                offset += num;
            }
            while (offset < buffer2.Length);
            using (MemoryStream stream = new MemoryStream())
            {
                stream.Write(buffer2, 0, buffer2.Length);
                return Deserialize(stream);
            }
        }

        public static NetworkData ReceiveData(Socket soc)
        {
            if ((soc == null) || !soc.Connected)
            {
                return null;
            }
            return ReceiveData(new NetworkStream(soc));
        }

        public static byte[] Serialize(NetworkData data)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                using (MemoryStream stream2 = new MemoryStream())
                {
                    try
                    {
                        NetworkDataTransportWrapper instance = new NetworkDataTransportWrapper
                        {
                            data = data
                        };
                        ProtoBuf.Serializer.Serialize<NetworkDataTransportWrapper>(stream2, instance);
                    }
                    catch (Exception exception)
                    {
                        return null;
                    }
                    if (statisticsLogUpdateTime > 0.0)
                    {
                        Type key = data.GetType();
                        if (statistics.ContainsKey(key))
                        {
                            StatisticsHelper local1 = statistics[key];
                            local1.ByteSum += stream2.Length;
                            StatisticsHelper local2 = statistics[key];
                            local2.PacketNubmer++;
                            StatisticsHelper local3 = statistics[key];
                            local3.BytesSinceLastCheck += stream2.Length;
                        }
                        else
                        {
                            statistics[key] = new StatisticsHelper(stream2.Length);
                        }
                        if (DateTime.UtcNow.Subtract(lastStatisticUpdateTime).TotalSeconds >= statisticsLogUpdateTime)
                        {
                            string str = "Serialize packet statistics: \n";
                            foreach (KeyValuePair<Type, StatisticsHelper> pair in statistics)
                            {
                                double num = ((double)(((float)pair.Value.BytesSinceLastCheck) / 1024f)) / DateTime.UtcNow.Subtract(lastStatisticUpdateTime).TotalSeconds;
                                object[] objArray2 = new object[] { str, pair.Key, ": ", num.ToString("0.0"), " (", (int)(((float)pair.Value.ByteSum) / 1024f), "), \n" };
                                str = string.Concat(objArray2);
                                pair.Value.BytesSinceLastCheck = 0L;
                            }
                            lastStatisticUpdateTime = DateTime.UtcNow;
                        }
                    }
                    stream.Write(BitConverter.GetBytes((uint)stream2.Length), 0, 4);
                    stream.Write(stream2.ToArray(), 0, (int)stream2.Length);
                    stream.Flush();
                    return stream.ToArray();
                }
            }
        }

        public class CorruptedPackageException : Exception
        {
            public CorruptedPackageException(string message)
                : base(message)
            {
            }
        }

        public class StatisticsHelper
        {
            public long BytesSinceLastCheck;
            public long ByteSum;
            public int PacketNubmer;

            public StatisticsHelper(long bytes)
            {
                this.ByteSum = bytes;
                this.PacketNubmer = 1;
                this.BytesSinceLastCheck = bytes;
            }
        }

        public class ZeroDataException : Exception
        {
            public ZeroDataException(string message)
                : base(message)
            {
            }
        }
    }
}
