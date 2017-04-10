namespace ServerTest
{
    using System;
    using System.IO;
    using System.Net.Sockets;
    using System.Runtime.InteropServices;
    using System.Security.Cryptography;
    using System.Text;

    internal static class CryptoHelper
    {
        public static byte[] DecryptRijndael(byte[] data, byte[] key, byte[] iv)
        {
            int length = 0;
            MemoryStream stream = new MemoryStream(data);
            ICryptoTransform transform = new RijndaelManaged
            {
                KeySize = 0x100,
                BlockSize = 0x100,
                Padding = PaddingMode.Zeros,
                Mode = CipherMode.CBC
            }.CreateDecryptor(key, iv);
            CryptoStream stream2 = new CryptoStream(stream, transform, CryptoStreamMode.Read);
            byte[] buffer = new byte[stream.Length];
            length = stream2.Read(buffer, 0, buffer.Length);
            byte[] destinationArray = new byte[length];
            Array.Copy(buffer, 0, destinationArray, 0, length);
            return destinationArray;
        }

        public static byte[] DecryptRSA(string PrivateKey, byte[] cipher)
        {
            using (RSACryptoServiceProvider provider = new RSACryptoServiceProvider(2048))
            {
                provider.FromXmlString(PrivateKey);
                return provider.Decrypt(cipher, false);
            }
        }

        public static byte[] EncryptRijndael(byte[] data, byte[] key, out byte[] iv)
        {
            MemoryStream stream = new MemoryStream();
            RijndaelManaged managed = new RijndaelManaged
            {
                KeySize = 0x100,
                BlockSize = 0x100,
                Padding = PaddingMode.Zeros,
                Mode = CipherMode.CBC
            };
            managed.GenerateIV();
            iv = managed.IV;
            ICryptoTransform transform = managed.CreateEncryptor(key, managed.IV);
            byte[] buffer2 = data;
            CryptoStream stream2 = new CryptoStream(stream, transform, CryptoStreamMode.Write);
            stream2.Write(buffer2, 0, buffer2.Length);
            stream2.FlushFinalBlock();
            byte[] buffer = stream.ToArray();
            stream.Close();
            stream2.Close();
            return buffer;
        }

        public static byte[] EncryptRSA(string PublicKey, byte[] plain)
        {
            using (RSACryptoServiceProvider provider = new RSACryptoServiceProvider(2048))
            {
                provider.FromXmlString(PublicKey);
                return provider.Encrypt(plain, false);
            }
        }

        public static string ExchangePublicKeys(NetworkStream stream, string publicKey)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(publicKey);
            uint length = (uint)bytes.Length;
            stream.Write(BitConverter.GetBytes(length), 0, 4);
            stream.Write(bytes, 0, bytes.Length);
            stream.Flush();
            length = BitConverter.ToUInt32(readBytes(stream, 4), 0);
            if (length == 0)
            {
                throw new Exception("Exchange public keys error.");
            }
            bytes = readBytes(stream, (int)length);
            return Encoding.UTF8.GetString(bytes);
        }

        public static void GenerateRSAKeys(out string publicKey, out string privateKey)
        {
            RSACryptoServiceProvider provider = new RSACryptoServiceProvider(2048);
            publicKey = provider.ToXmlString(false);
            privateKey = provider.ToXmlString(true);
        }

        private static byte[] readBytes(Stream stream, int size)
        {
            byte[] buffer = new byte[size];
            int offset = 0;
            do
            {
                int num2 = stream.Read(buffer, offset, size - offset);
                if (num2 == 0)
                {
                    throw new Exception("Error reading stream.");
                }
                offset += num2;
            }
            while (offset < size);
            return buffer;
        }

        public static byte[] ReadRequest(NetworkStream stream, string privateKey)
        {
            uint num = BitConverter.ToUInt32(readBytes(stream, 4), 0);
            if (num == 0)
            {
                throw new Exception("Read request error.");
            }
            byte[] cipher = readBytes(stream, (int)num);
            byte[] key = DecryptRSA(privateKey, cipher);
            num = BitConverter.ToUInt32(readBytes(stream, 4), 0);
            if (num == 0)
            {
                throw new Exception("Read request error.");
            }
            cipher = readBytes(stream, (int)num);
            byte[] iv = DecryptRSA(privateKey, cipher);
            num = BitConverter.ToUInt32(readBytes(stream, 4), 0);
            if (num == 0)
            {
                throw new Exception("Read request error.");
            }
            return DecryptRijndael(readBytes(stream, (int)num), key, iv);
        }

        public static byte[] ReadResponse(NetworkStream stream, string privateKey)
        {
            uint num = BitConverter.ToUInt32(readBytes(stream, 4), 0);
            if (num == 0)
            {
                throw new Exception("Read main server response error.");
            }
            byte[] cipher = readBytes(stream, (int)num);
            byte[] key = DecryptRSA(privateKey, cipher);
            num = BitConverter.ToUInt32(readBytes(stream, 4), 0);
            if (num == 0)
            {
                throw new Exception("Read main server response error.");
            }
            cipher = readBytes(stream, (int)num);
            byte[] iv = DecryptRSA(privateKey, cipher);
            num = BitConverter.ToUInt32(readBytes(stream, 4), 0);
            if (num == 0)
            {
                throw new Exception("Read main server response error.");
            }
            return DecryptRijndael(readBytes(stream, (int)num), key, iv);
        }

        public static void WriteRequest(NetworkStream stream, byte[] data, string remotePublicKey)
        {
            byte[] buffer3;
            byte[] key = SymmetricAlgorithm.Create().Key;
            byte[] buffer4 = EncryptRijndael(data, key, out buffer3);
            byte[] buffer = EncryptRSA(remotePublicKey, key);
            uint length = (uint)buffer.Length;
            stream.Write(BitConverter.GetBytes(length), 0, 4);
            stream.Write(buffer, 0, buffer.Length);
            buffer = EncryptRSA(remotePublicKey, buffer3);
            length = (uint)buffer.Length;
            stream.Write(BitConverter.GetBytes(length), 0, 4);
            stream.Write(buffer, 0, buffer.Length);
            length = (uint)buffer4.Length;
            stream.Write(BitConverter.GetBytes(length), 0, 4);
            stream.Write(buffer4, 0, buffer4.Length);
            stream.Flush();
        }

        public static void WriteResponse(NetworkStream stream, byte[] data, string remotePublicKey)
        {
            byte[] buffer3;
            byte[] key = SymmetricAlgorithm.Create().Key;
            byte[] buffer4 = EncryptRijndael(data, key, out buffer3);
            byte[] buffer = EncryptRSA(remotePublicKey, key);
            uint length = (uint)buffer.Length;
            stream.Write(BitConverter.GetBytes(length), 0, 4);
            stream.Write(buffer, 0, buffer.Length);
            buffer = EncryptRSA(remotePublicKey, buffer3);
            length = (uint)buffer.Length;
            stream.Write(BitConverter.GetBytes(length), 0, 4);
            stream.Write(buffer, 0, buffer.Length);
            length = (uint)buffer4.Length;
            stream.Write(BitConverter.GetBytes(length), 0, 4);
            stream.Write(buffer4, 0, buffer4.Length);
            stream.Flush();
        }
    }
}
