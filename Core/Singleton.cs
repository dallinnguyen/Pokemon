using UnityEngine;

/// <summary>
/// Singleton pattern for any type.
/// </summary>
/// <typeparam name="T">The type that is a singleton.</typeparam>
public abstract class Singleton<T> where T : IInitializeable, new()
{
	private static T instance;

	/// <summary>
	/// Gets the singleton instance of the given PPBehaviour.
	/// </summary>
	public static T Instance
	{
		get
		{
			if (instance == null)
			{
				instance = new T();
				instance.Initialize();
			}
			return instance;
		}
	}
}
