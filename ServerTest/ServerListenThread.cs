using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace ServerTest
{
    class ServerListenThread
    {
        public void ServerListener()
        {
            //---listen at the specified IP and port no.---
            IPAddress localAdd = IPAddress.Parse("127.0.0.1");
            TcpListener listener = new TcpListener(localAdd, 6001);
            Console.WriteLine("Listening...");
            listener.Start();

            while (true)
            {

                //---incoming client connected---
                TcpClient client = listener.AcceptTcpClient();

                Console.WriteLine("Accepted...");
                NetworkStream nwStream = client.GetStream();

                //---get the incoming data through a network stream---

                try
                {
                    NetworkData data1 = Serializer.ReceiveData(nwStream);
                    if (data1 != null)
                    {
                        Console.WriteLine(data1.GetType().ToString());

                        if(data1.GetType() == typeof(GetDeletedCharactersRequest))
                        {
                            GetDeletedCharactersRequest Req = (GetDeletedCharactersRequest)data1;

                            Console.WriteLine(Req.Message);
                            Console.WriteLine(Req.Response);
                            Console.WriteLine(Req.Sender);
                            Console.WriteLine(Req.ServerId);

                            CharacterListResponse Response = new CharacterListResponse
                            {
                                Response = ResponseResult.Success
                                //Characters = CharacterListClass.Characters
                            };

                            byte[] SendData = Serializer.Serialize(Response);

                            nwStream.Write(SendData, 0, SendData.Length);
                        }

                        if (data1.GetType() == typeof(CheckInRequest))
                        {
                            CheckInRequest Req = (CheckInRequest)data1;
                            // Do something with the data you recieved...
                            Console.WriteLine(Req.ServerID);
                            Console.WriteLine(Req.ServerName);
                            Console.WriteLine(Req.GamePort);
                            Console.WriteLine(Req.StatusPort);
                            Console.WriteLine(Req.Private);
                            Console.WriteLine(Req.ServerHash);
                            Console.WriteLine(Req.CleanStart);

                            ServerData CurrentServer = new ServerData
                            {
                                Address = "127.0.0.1",
                                GamePort = Req.GamePort,
                                StatusPort = Req.StatusPort,
                                Locked = false,
                                Hash = Req.ServerHash,
                                Id = Req.ServerID,
                                Name = Req.ServerName,
                                Tag = ServerTag.Official
                            };

                            ServerListClass.Servers.Add(0, CurrentServer);

                            CheckInResponse Response = new CheckInResponse {
                                Response = ResponseResult.Success,
                                ServerID = Req.ServerID,
                                Message = string.Empty
                            };

                            byte[] SendData = Serializer.Serialize(Response);

                            nwStream.Write(SendData, 0, SendData.Length);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Nulldata");
                    }
                }
                catch (SocketException exception)
                {
                    Console.WriteLine(exception.Message);
                }
                catch (Serializer.ZeroDataException)
                {
                }

                client.Close();
            }
        }
    }
}
