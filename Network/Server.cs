/// <summary>
/// A game server that can be connected to.
/// </summary>
public class Server
{
	/// <summary>
	/// Initializes a new instance of the <see cref="Server" /> class with the given information.
	/// </summary>
	/// <param name="ipAddress">The IP address of the server.</param>
	/// <param name="port">The port of the server.</param>
	public Server(string ipAddress, ushort port)
	{
		this.IPAddress = ipAddress;
		this.Port = port;
	}

	/// <summary>
	/// Gets the master server.
	/// </summary>
	public static Server Master
	{
		get
		{
			// return new Server("master.planetpokemon.net", 44444);
			//return new Server("188.193.154.119", 44444);
			return new Server("127.0.0.1", 52843);
		}
	}

	/// <summary>
	/// Gets or sets the available game servers.
	/// </summary>
	public static Server[] Servers
	{
		get;
		set;
	}

	/// <summary>
	/// Gets the IP address of the server.
	/// </summary>
	public string IPAddress
	{
		get;
		private set;
	}

	/// <summary>
	/// Gets the port of the server.
	/// </summary>
	public ushort Port
	{
		get;
		private set;
	}
}
