using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace ServerTest
{
    class GameClientListenThread
    {
        public void TellGameServerToConnectToClient(IPAddress ip, int port, CharacterData Data)
        {
            IPEndPoint remoteEP = new IPEndPoint(ip, port);

            Socket sender = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                sender.Connect(remoteEP);

                NetworkStream stream = new NetworkStream(sender);

                CharacterDataResponse cData = new CharacterDataResponse
                {
                    //CharacterData = d.ToString(),
                    CharacterId = Data.Id,
                    CharacterName = Data.Name,
                    SteamId = Data.SteamId
                };

                byte[] buffer = Serializer.Serialize(cData);

                stream.Write(buffer, 0, buffer.Length);

                stream.Close();
                sender.Close();
            } catch(Exception e)
            {

            }
        }

        

        public void ServerListener()
        {
            //---listen at the specified IP and port no.---
            IPAddress localAdd = IPAddress.Parse("127.0.0.1");
            TcpListener listener = new TcpListener(localAdd, 6000);
            Console.WriteLine("Listening...");
            listener.Start();

            string publicKey = "";
            string privateKey = "";

            CryptoHelper.GenerateRSAKeys(out publicKey, out privateKey);

            while (true)
            {

                //---incoming client connected---
                TcpClient client = listener.AcceptTcpClient();

                Console.WriteLine("Accepted...");
                NetworkStream nwStream = client.GetStream();
                //---get the incoming data through a network stream---


                string RemoteKey = CryptoHelper.ExchangePublicKeys(nwStream, publicKey);
                byte[] buffer = CryptoHelper.ReadResponse(nwStream, privateKey);

                int totalLength = ((int)buffer[0]) + 4;
                byte[] buffer2 = new byte[totalLength];

                for (int i = 0; i < totalLength; i++)
                {
                    buffer2[i] = buffer[i];
                    Console.Write(buffer[i] + " ");
                }

                MemoryStream str = new MemoryStream();
                NetworkData data1 = Serializer.ReceiveData(new MemoryStream(buffer2));

                if (data1 != null)
                {
                    if (data1.GetType() == typeof(SignInRequest))
                    {
                        SignInRequest Request = (SignInRequest)data1;

                        Console.WriteLine(Request.ClientHash);
                        Console.WriteLine(Request.ClientVersion);
                        Console.WriteLine(Request.SteamId);

                        SignInResponse Res = new SignInResponse
                        {
                            BanPoints = 0,
                            CharacterInUse = 0,
                            Servers = ServerListClass.Servers,
                            Characters = CharacterListClass.Characters
                        };

                        byte[] data = Serializer.Serialize(Res);

                        CryptoHelper.WriteResponse(nwStream, data, RemoteKey);
                    }
                    if(data1.GetType() == typeof(CreateCharacterRequest))
                    {
                        CreateCharacterRequest Request = (CreateCharacterRequest)data1;

                        Console.WriteLine(Request.Name);
                        Console.WriteLine(Request.SteamId);

                        CharacterData dat = new CharacterData
                        {
                            Id = CharacterListClass.Counter++,
                            Name = Request.Name,
                            ServerId = 0,
                            SteamId = Request.SteamId
                        };

                        CharacterListClass.Characters.Add(dat.Id, dat);

                        SaveLoadClass.SaveCharacterList();

                        CreateCharacterResponse Res = new CreateCharacterResponse
                        {
                            CharacterID = dat.Id
                        };

                        byte[] data = Serializer.Serialize(Res);

                        CryptoHelper.WriteResponse(nwStream, data, RemoteKey);

                        // Connect to 

                        ServerData server;
                        ServerListClass.Servers.TryGetValue(0, out server);

                        IPAddress ip = IPAddress.Parse(server.Address);
                        TellGameServerToConnectToClient(ip, server.GamePort, dat);
                    }
                    if(data1.GetType() == typeof(CharacterDataRequest))
                    {
                        CharacterDataRequest Request = (CharacterDataRequest)data1;
                        Console.WriteLine(Request.CharacterId);
                        Console.WriteLine(Request.ServerId);

                        CharacterData Data;
                        CharacterListClass.Characters.TryGetValue(Request.CharacterId, out Data);

                        CharacterDataResponse Response = new CharacterDataResponse
                        {
                            CharacterId = Data.Id,
                            CharacterName = Data.Name,
                            SteamId = Data.SteamId
                        };

                        byte[] data = Serializer.Serialize(Response);

                        CryptoHelper.WriteResponse(nwStream, data, RemoteKey);
                    }
                }
                else
                {
                    Console.WriteLine("Nulldata from game");
                }

                client.Close();
            }
        }
    }
}
