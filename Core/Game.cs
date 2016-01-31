using UnityEngine;

/// <summary>
/// Container for overall game data and coroutines.
/// </summary>
public class Game : PPBehaviourSingleton<Game>, IInitializeable
{
	/// <summary>
	/// Records the given message in the log file.
	/// </summary>
	/// <param name="message">The message to record.</param>
	public void Log(string message)
	{
	}

	/// <summary>
	/// Not used
	/// </summary>
	public void Initialize()
	{
	}
}