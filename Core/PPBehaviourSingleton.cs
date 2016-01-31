using System.Collections;
using UnityEngine;

/// <summary>
/// Singleton pattern for a PPBehaviour.
/// </summary>
/// <typeparam name="T">The PPBehaviour that is a singleton.</typeparam>
public abstract class PPBehaviourSingleton<T> : PPBehaviour where T : PPBehaviour, IInitializeable
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
				GameObject persistent = GameObject.Find("Persistent");
				if (persistent == null)
				{
					persistent = new GameObject("Persistent");
					DontDestroyOnLoad(persistent);
				}

				instance = persistent.GetComponent<T>();
				if (instance == null)
				{
					instance = persistent.AddComponent<T>();
				}
				instance.Initialize();
			}
			return instance;
		}
	}
}
