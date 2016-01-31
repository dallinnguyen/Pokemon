using System.Collections;
using UnityEngine;

/// <summary>
/// Singleton pattern for a PPBehaviour, that is possibly not in the scene and has to be instanciated in the rest of the code
/// </summary>
/// <typeparam name="T">The PPBehaviour that is a singleton.</typeparam>
public abstract class PPNonPersistantBehaviourSingleton<T> : PPBehaviour where T : PPBehaviour
{
	private static T instance;

	/// <summary>
	/// Gets the singleton instance of the given PPBehaviour.
	/// </summary>
	public static T Instance
	{
		get
		{
			return instance;
		}
	}

	/// <summary>
	/// Simply return this to get the instance, nothing in addition
	/// </summary>
	protected abstract T GetInstance();

	protected abstract void Initialize();

	private void Awake()
	{
		if (instance == null)
		{
			instance = GetInstance();
			Initialize();
		}
		else
		{
			Destroy(gameObject);
		}
	}
}
