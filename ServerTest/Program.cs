using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using System.IO;
using ProtoBuf;
using ProtoBuf.Serializers;
using ProtoBuf.Meta;
using ProtoBuf.Compiler;

namespace ServerTest
{
    public enum ResponseResult : short
    {
        AlreadyLoggedInError = -1,
        ClientVersionError = -2,
        Error = -1,
        OwnershipIssue = -5,
        Success = 1,
        WrongPassword = -4
    };

    public enum ServerTag
    {
        Official,
        Community,
        Private
    }


    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
    public class ServerData
    {
        public string Address;
        public int GamePort;
        public uint Hash;
        public long Id;
        public bool Locked;
        public string Name;
        public int StatusPort;
        public ServerTag Tag;
    }

    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
    public class VesselData
    {
        public float[] CollidersCenterOffset;
        public long Id;
        public string Name;
        public GameScenes.SceneID SceneID;
        public long ServerID;
        public string Tag;
    }

    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
    public class IPAddressRange
    {
        public string EndAddress;
        public string StartAddress;

        public IPAddressRange() { }
    };

    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
    public class MainServerGenericResponse : NetworkData
    {
        public string Message = "";
        public ResponseResult Response = ResponseResult.Success;
    };

    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
    public class CheckInMessage : NetworkData
    {
        public long ServerID;
    }

    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
    public class CheckInRequest : NetworkData
    {
        public bool CleanStart;
        public int GamePort;
        public bool Private;
        public uint ServerHash;
        public long ServerID;
        public string ServerName;
        public int StatusPort;
    };

    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
    public class CheckInResponse : NetworkData
    {
        public IPAddressRange[] AdminIpAddressRanges;
        public string Message = "";
        public ResponseResult Response = ResponseResult.Success;
        public long ServerID;
    };

    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
    public class ServerStatusRequest : NetworkData
    {
    };

    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
    public class CheckConnectionMessage : NetworkData
    {
    };

    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
    public class ServerStatusResponse : NetworkData
    {
        public short CurrentPlayers;
        public short MaxPlayers;
        public ResponseResult Response = ResponseResult.Success;
    };

    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
    public class SignInRequest : NetworkData
    {
        public uint ClientHash;
        public string ClientVersion;
        public string SteamId;
    };

    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
    public class SignInResponse : NetworkData
    {
        public int BanPoints = 0;
        public long CharacterInUse = 0L;
        public Dictionary<long, CharacterData> Characters = new Dictionary<long, CharacterData>();
        public string LastSignInTime = string.Empty;
        public string Message = string.Empty;
        public ResponseResult Response = ResponseResult.Success;
        public Dictionary<long, ServerData> Servers = new Dictionary<long, ServerData>();
        public Dictionary<long, VesselData> Ships = new Dictionary<long, VesselData>();
    };

    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
    public class CharacterData
    {
        public long Id;
        public string Name;
        public long ServerId;
        public string SteamId;
    };

    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
    public class LogInRequest : NetworkData
    {
        public uint Clienthash;
        public long GUID;
        public string Password;
        public long ServerID;
    }

    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
    public class LogOutRequest : NetworkData
    {
    }

    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
    public class CreateCharacterRequest : NetworkData
    {
        public string Name;
        public string SteamId;
    }

    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
    public class ResetServer : NetworkData
    {
    }

    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
    public class CharacterDataResponse : NetworkData
    {
        public string CharacterData = string.Empty;
        public long CharacterId;
        public string CharacterName = string.Empty;
        public string Message = string.Empty;
        public ResponseResult Response = ResponseResult.Success;
        public string SteamId;
    }

    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
    public class CreateCharacterResponse : NetworkData
    {
        public long CharacterID;
        public string Message = string.Empty;
        public ResponseResult Response = ResponseResult.Success;
    }

    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
    public class CharacterListResponse : NetworkData
    {
        public Dictionary<long, CharacterData> Characters = new Dictionary<long, CharacterData>();
        public string Message = string.Empty;
        public ResponseResult Response = ResponseResult.Success;
    }

    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
    public class LogOutResponse : NetworkData
    {
        public ResponseResult Response = ResponseResult.Success;

        public LogOutResponse()
        {
            Response = ResponseResult.Success;
        }
    }

    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
    public class LatencyTestMessage : NetworkData
    {
        public byte Index;
    }

    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
    public class DeleteCharacterResponse : NetworkData
    {
        public string Message = string.Empty;
        public ResponseResult Response = ResponseResult.Success;
    }

    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
    public class ServerUpdateMessage : NetworkData
    {
        public bool CleanStart;
    }

    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
    public class DeleteCharacterRequest : NetworkData
    {
        public long ServerId;
        public string SteamId;
    }

    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
    public class MarkAsLoggedOutRequest : NetworkData
    {
        public long CharacterId;
        public long ServerId;
    }

    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
    public class MarkAsLoggedInRequest : NetworkData
    {
        public long CharacterId;
        public long ServerId;
    }

    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
    public class CharacterDataRequest : NetworkData
    {
        public long CharacterId;
        public long ServerId;
    }

    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
    public class GetDeletedCharactersRequest : NetworkData
    {
        public string Message = string.Empty;
        public ResponseResult Response = ResponseResult.Success;
        public long ServerId;
    }

    [ProtoContract(ImplicitFields=ImplicitFields.AllPublic)]
    [ProtoInclude(202, typeof(ServerStatusResponse))]
    [ProtoInclude(101, typeof(LogOutRequest))]
    [ProtoInclude(100, typeof(LogInRequest))]
    [ProtoInclude(300, typeof(CheckConnectionMessage))]
    [ProtoInclude(504, typeof(CheckInResponse))]
    [ProtoInclude(102, typeof(ServerStatusRequest))]
    [ProtoInclude(405, typeof(CreateCharacterRequest))]
    [ProtoInclude(319, typeof(ResetServer))]
    [ProtoInclude(503, typeof(CharacterDataResponse))]
    [ProtoInclude(505, typeof(CreateCharacterResponse))]
    [ProtoInclude(408, typeof(CheckInRequest))]
    [ProtoInclude(501, typeof(CharacterListResponse))]
    [ProtoInclude(500, typeof(MainServerGenericResponse))]
    [ProtoInclude(201, typeof(LogOutResponse))]
    [ProtoInclude(331, typeof(CheckInMessage))]
    [ProtoInclude(333, typeof(LatencyTestMessage))]
    [ProtoInclude(502, typeof(SignInResponse))]
    [ProtoInclude(506, typeof(DeleteCharacterResponse))]
    [ProtoInclude(334, typeof(ServerUpdateMessage))]
    [ProtoInclude(406, typeof(DeleteCharacterRequest))]
    [ProtoInclude(401, typeof(MarkAsLoggedOutRequest))]
    [ProtoInclude(400, typeof(MarkAsLoggedInRequest))]
    [ProtoInclude(0x194, typeof(SignInRequest))]
    [ProtoInclude(407, typeof(CharacterDataRequest))]
    [ProtoInclude(403, typeof(GetDeletedCharactersRequest))]
    public abstract class NetworkData
    {
        public long Sender;

        protected NetworkData()
        {
        }

        public override string ToString()
        {
            return Json.Serialize(this, Json.Formatting.Indented);
        }
    };

    [Serializable, ProtoContract(ImplicitFields=ImplicitFields.AllPublic)]
    public class NetworkDataTransportWrapper
    {
        public NetworkData data;
    };

    public static class ServerListClass
    {
        public static Dictionary<long, ServerData> Servers = new Dictionary<long, ServerData>();
    };

    public static class CharacterListClass
    {
        public static Dictionary<long, CharacterData> Characters = new Dictionary<long, CharacterData>();
        public static int Counter = 0;
    }

    class Program
    {
        public static T Deserialize<T>(Stream stream)
        {
            return ((T)RuntimeTypeModel.Default.Deserialize(stream, null, typeof(T)));
        }

        
        static void Main(string[] args)
        {
            ServerListenThread SLInstance = new ServerListenThread();
            GameClientListenThread GCLInstance = new GameClientListenThread();

            Thread GCLThread = new Thread(new ThreadStart(GCLInstance.ServerListener));
            GCLThread.Start();

            Thread SLThread = new Thread(new ThreadStart(SLInstance.ServerListener));
            SLThread.Start();

            while(GCLThread.IsAlive && SLThread.IsAlive)
            {
                Thread.Sleep(1000);
            }
        }
    }
}
