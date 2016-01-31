using UnityEngine;

/// <summary>
/// Planet Pokemon wrapper for BetterBehaviour that provides custom methods.
/// </summary>
public abstract class PPBehaviour : MonoBehaviour
{
	/// <summary>
	/// Retrieves the component of the given type from this object, throwing an
	/// an exception if it does not exist.
	/// </summary>
	/// <typeparam name="T">The type of component.</typeparam>
	/// <returns>The component of the given type.</returns>
	public T GetSafeComponent<T>() where T : PPBehaviour
	{
		T component = this.GetComponent<T>();

		if (component == null)
		{
			Debug.LogError("Expected to find component of type " + typeof(T) + " but found none", this.gameObject);
		}

		return component;
	}

	/// <summary>
	/// Finds the child of this GameObject with the given name.
	/// </summary>
	/// <param name="name">The name the child.</param>
	/// <returns>The child.</returns>
	public GameObject FindChild(string name)
	{
		return gameObject.FindChild(name);
	}
}
