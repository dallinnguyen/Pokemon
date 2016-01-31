/// <summary>
/// Holds account information and all characters corresponding to this account. Object pooled.
/// </summary>
public class Account
{
	/// <summary>
	/// Gets or sets the account of the current player.
	/// </summary>
	public static Account Main
	{
		get;
		set;
	}

	/// <summary>
	/// Gets the user's username.
	/// </summary>
	public string Username
	{ 
		get; 
		private set;
	}

	/// <summary>
	/// Gets the user's email.
	/// </summary>
	public string Email
	{
		get;
		private set;
	}

	/// <summary>
	/// Gets or sets the user's characters.
	/// </summary>
	public Character[] Characters
	{
		get;
		set; 
	}

	/// <summary>
	/// Recreates this object with new values.
	/// </summary>
	/// <param name="username">The username</param>
	/// <param name="email">The email</param>
	/// <param name="characters">An array of character objects</param>
	/// <returns>The new object.</returns>
	public Account Recreate(string username, string email, Character[] characters)
	{
		Username = username;
		Email = email;
		Characters = characters;
		return this;
	}
}
