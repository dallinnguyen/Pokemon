using System;
using System.Collections;
using Lidgren.Network;
using UnityEngine;

/// <summary>
/// The client connection with a game server.
/// </summary>
public class Admin : Singleton<Admin>, IInitializeable
{
    private const float NetworkDelay = 0.015f;
    private NetClient admin;
    private string password;

    /// <summary>
    /// Handles an error event.
    /// </summary>
    /// <param name="message">The error message.</param>
    public delegate void ErrorHandler(string message);

    /// <summary>
    /// Handles a successful GetServers event.
    /// </summary>
    /// <param name="servers">The servers returned by the GetServers request.</param>
    public delegate void GetServersHandler(Server[] servers);

    /// <summary>
    /// Handles receiving the account after connecting to the masterserver.
    /// </summary>
    /// <param name="account">The account received from the master server.</param>
    public delegate void GetAccountHandler(Account account);

    /// <summary>
    /// Handles a standard action event with no return value and no parameters.
    /// </summary>
    public delegate void ActionHandler();

    /// <summary>
    /// Handles a successful LoadChunk event.
    /// </summary>
    /// <param name="chunkInfo">The network information returned by the LoadChunk request.</param>
    public delegate void LoadChunkHandler(MapChunk.NetworkInfo chunkInfo);

    /* Redundant since connecting equals logging in 
    /// <summary>
    /// Shot when Login is successful.
    /// </summary>
    public event ActionHandler OnLogin;
    */

    /// <summary>
    /// Shot when Login fails (i.e. when the user is banned/not found/not authorized).
    /// </summary>
    public event ErrorHandler OnLoginFailure;

    /// <summary>
    /// Shot when the admin is disconnected.
    /// </summary>
    public event ErrorHandler OnDisconnect;

    /// <summary>
    /// Shot when GetServers is successful.
    /// </summary>
    public event GetServersHandler OnGetServers;

    /// <summary>
    /// Shot when GetServers is successful.
    /// </summary>
    //public event GetAccountHandler OnAccount;

    /// <summary>
    /// Shot when Connect is successful.
    /// </summary>
    public event ActionHandler OnConnect;

    /// <summary>
    /// Shot when the client is disconnecting.
    /// </summary>
    public event ActionHandler OnDisconnecting;

    /// <summary>
    /// Shot when the masterserver replies to our RequestRedirect.
    /// </summary>
    public event ActionHandler OnReceiveRedirect;

    /// <summary>
    /// Shot when Connect fails.
    /// </summary>
    public event ErrorHandler OnConnectFailure;

    /// <summary>
    /// Shot when LoadChunk is successful.
    /// </summary>
    public event LoadChunkHandler OnLoadChunk;

    /// <summary>
    /// Shot when LoadChunk fails.
    /// </summary>
    public event ErrorHandler OnLoadChunkFailure;

    /// <summary>
    /// Gets a value indicating whether the client is currently connected to a server.
    /// </summary>
    public bool Connected
    {
        get;
        private set;
    }

    /// <summary>
    /// Gets the server the masterserver sent to us.
    /// </summary>
    public Server RedirectTarget
    {
        get;
        private set;
    }

    /// <summary>
    /// Gets the token needed to connect to the <see cref="RedirectTarget"/>
    /// </summary>
    public string Token
    {
        get;
        private set;
    }

    /// <summary>
    /// Gets the username of the admin.
    /// </summary>
    public string Username
    {
        get;
        private set;
    }

    /// <summary>
    /// Gets the ID of the selecetd character we want to play.
    /// </summary>
    public uint CharacterId
    {
        get;
        private set;
    }

    /// <summary>
    /// Initializes the client object.
    /// </summary>
    public void Initialize()
    {
        NetPeerConfiguration config = new NetPeerConfiguration("PlanetPokemon");
        config.ConnectionTimeout = 8;
        admin = new NetClient(config);
        admin.Start();
        
    }

    /// <summary>
    /// Attempts to retrieve a user's password salt from the masterserver using the given credentials
    /// </summary>
    /// <param name="user">The username of the user.</param>
    /// <param name="password">The password of the user.</param>
    public void Login(string username, string password)
    {
        NetOutgoingMessage message = admin.CreateMessage();
        message.Write((ushort)AccessLevel.Admin);
        message.Write(username);
        
        admin.Connect(Server.Master.IPAddress, Server.Master.Port, message);

        Username = username;
        this.password = password;
        Game.Instance.StartCoroutine(NetworkLoop());
    }



    /// <summary>
    /// Attempts to retrieve a user's password salt from the masterserver using the given credentials
    /// </summary>
    /// <param name="user">The username of the user.</param>
    /// <param name="password">The password of the user.</param>


    public void testMessage()
    {
        NetOutgoingMessage message = admin.CreateMessage();
        message.Write("I love you");
        //admin.SendMessage(message, NetDeliveryMethod.ReliableOrdered);
        admin.Connect(Server.Master.IPAddress, Server.Master.Port, message);
    }

    /// <summary>
    /// Attempts to log a user into the master server after having retrieved the password salt.
    /// </summary>
    /// <param name="passwordSalt">The password salt from the masterserver.</param>
    public void Login(string passwordSalt)
    {
        NetOutgoingMessage message = admin.CreateMessage();
        message.Write((ushort)AccessLevel.Admin);
        message.Write(Username);
        int timestamp = Utility.UnixTimestamp();
        message.Write(timestamp);
        message.Write(passwordSalt);
        string passwordHash = Utility.HashBCrypt(Utility.HashSHA512(Username + timestamp + passwordSalt + Utility.HashBCrypt(password, passwordSalt)), passwordSalt);
        message.Write(passwordHash);
        message.Write(Fingerprint.UniqueIdentifier);
        admin.Connect(Server.Master.IPAddress, Server.Master.Port, message);
    }

    /// <summary>
    /// Retrieves the available game servers from the master server.
    /// </summary>
    public void GetServers()
    {
        NetOutgoingMessage message = admin.CreateMessage();
        message.Write((int)AdminAction.GetServers);
        admin.SendMessage(message, NetDeliveryMethod.ReliableOrdered);
    }

    /// <summary>
    /// Requests a server to connect to from the masterserver using a given Character.
    /// </summary>
    /// <param name="characterId">The id of the character that the user wants to play with.</param>
    public void RequestRedirect(uint characterId, int realmId)
    {
        CharacterId = characterId;
        NetOutgoingMessage message = admin.CreateMessage();
        message.Write(false); // not requesting public information
        message.Write((ushort)AdminAction.RequestRedirect);
        message.Write((uint)characterId);
        message.Write((uint)realmId);
        admin.SendMessage(message, NetDeliveryMethod.ReliableOrdered);
    }

    /// <summary>
    /// Opens a connection to a game server. Make sure the client is initialized properly beforehand.
    /// </summary>
    /// <param name="server">The game server to connect to.</param>
    /// <param name="token">A secret token from the masterserver that expires after a few seconds.</param>
    /// <param name="username">The username of the admin</param>
    /// <param name="characterId">The id of the character that the user wants to play with.</param>
    public void Connect(Server server, string token, string username, uint characterId)
    {
        Debug.Log("Connecting to server at " + server.IPAddress + ":" + server.Port);
        Initialize();
        NetOutgoingMessage hailMessage = admin.CreateMessage();
        hailMessage.Write((ushort)AccessLevel.Admin);
        hailMessage.Write(username);
        hailMessage.Write((uint)characterId);
        hailMessage.Write(token);
        admin.Connect(server.IPAddress, server.Port, hailMessage);
    }

    /// <summary>
    /// Disconnects the client from either the masterserver or a game server.
    /// </summary>
    /// <param name="reason">The reason sent to the server when disconnecting.</param>
    public void Disconnect(string reason)
    {
        Debug.Log("Manual Disconnect");
        admin.Shutdown(reason);
    }

    /// <summary>
    /// Loads network information on the map chunk located at the given location.
    /// </summary>
    /// <param name="x">The x coordinate of the map chunk.</param>
    /// <param name="y">The y coordinate of the map chunk.</param>
    /// <param name="layer">The map layer of the map chunk.</param>
    public void LoadChunk(int x, int y, int layer)
    {
    }

    private IEnumerator NetworkLoop()
    {
        Connected = true;
        while (Connected)
        {
            NetIncomingMessage incoming;
            while ((incoming = admin.ReadMessage()) != null)
            {
                try
                {
                    switch (incoming.MessageType)
                    {
                        case NetIncomingMessageType.Data:
                            AdminAction action = (AdminAction)incoming.ReadUInt16();
                            switch (action)
                            {
                                case AdminAction.GetServers:
                                    int amountOfServers = incoming.ReadInt32();
                                    Server[] serverList = new Server[amountOfServers];
                                    for (int i = 0; i < amountOfServers; i++)
                                    {
                                        serverList[i] = new Server(incoming.ReadString(), incoming.ReadUInt16());
                                    }
                                    OnGetServers(serverList);
                                    break;
                                case AdminAction.GetAccount:
                                    string username = incoming.ReadString();
                                    string email = incoming.ReadString();

                                    int amountOfCharacters = incoming.ReadInt32();
                                    Character[] characterList = new Character[amountOfCharacters];
                                    for (int i = 0; i < amountOfCharacters; i++)
                                    {
                                        int numberOfBytes = incoming.ReadInt32();
                                        byte[] serializedCharacter = incoming.ReadBytes(numberOfBytes);
                                        Serialization.Deserialize(serializedCharacter, out characterList[i]);
                                    }

                                    Account account = new Account().Recreate(username, email, characterList);
                                    Account.Main = account;

                                    //if (OnAccount != null)
                                    //{
                                    //    OnAccount(account);
                                    //}
                                    break;
                                case AdminAction.RequestRedirect:
                                    OnReceiveRedirect();

                                    string serverIp = incoming.ReadString();
                                    ushort serverPort = incoming.ReadUInt16();
                                    string token = incoming.ReadString();

                                    RedirectTarget = new Server(serverIp, serverPort);
                                    Token = token;

                                    Disconnect("Received redirect target.");
                                    break;
                                default:
                                    Debug.Log("Received unknown message type: " + (AdminAction)action);
                                    break;
                            }
                            break;
                        case NetIncomingMessageType.StatusChanged:
                            NetConnectionStatus status = (NetConnectionStatus)incoming.ReadByte();
                            Debug.Log("NetConnection Status changed: " + status.ToString());
                            switch (status)
                            {
                                case NetConnectionStatus.Disconnected:
                                    int reasonInt;
                                    Debug.Log("status changed disconnected");
                                    string reason = incoming.ReadString();
                                    if (!string.IsNullOrEmpty(reason))
                                    {
                                        if (reason.Length.Equals(29)) // server replied with password hash
                                        {
                                            Login(reason); // login with the hash
                                        }
                                        else if (reason.Equals("401"))
                                        {
                                            if (OnLoginFailure != null)
                                            {
                                                OnLoginFailure(Localization.Instance.Localize("login.error.401"));
                                            }
                                        }
                                        else if (reason.Equals("404"))
                                        {
                                            if (OnLoginFailure != null)
                                            {
                                                OnLoginFailure(Localization.Instance.Localize("login.error.404"));
                                            }
                                        }
                                        else if (int.TryParse(reason, out reasonInt))
                                        {
                                            if (OnLoginFailure != null)
                                            {
                                                OnLoginFailure(reason); // banned until <reason> (unix timestamp)
                                            }
                                        }
                                        else
                                        {
                                            if (OnDisconnect != null)
                                            {
                                                OnDisconnect(reason);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (OnDisconnect != null) OnDisconnect("Unknown error");
                                    }
                                    break;
                                case NetConnectionStatus.Connected:
                                    if (OnConnect != null)
                                    {
                                        OnConnect();
                                    }
                                    break;
                                case NetConnectionStatus.Disconnecting:
                                    if (OnDisconnecting != null)
                                    {
                                        OnDisconnecting();
                                    }
                                    break;
                                default:
                                    break;
                            }
                            break;
                        case NetIncomingMessageType.Error:
                            Debug.Log("Error: " + incoming.ReadString());
                            break;
                        case NetIncomingMessageType.WarningMessage:
                            Debug.Log("Warning: " + incoming.ReadString());
                            break;
                        case NetIncomingMessageType.ErrorMessage:
                            Debug.Log("Error: " + incoming.ReadString());
                            break;
                    }
                }
                catch (Exception e)
                {
                    Game.Instance.Log(e.StackTrace + ": " + e.Message + "; NetMessage: " + incoming.ToString());
                }
                finally
                {
                    admin.Recycle(incoming);
                }
            }
            yield return new WaitForSeconds(NetworkDelay);
        }
    }
}
