using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Used for keeping track of, instancing from, and destroying objects in various object pools.
/// </summary>
public class ObjectPool : Singleton<ObjectPool>, IInitializeable
{
	private Dictionary<System.Type, IObjectPool> pools;

	/// <summary>
	/// Container for generic object pools.
	/// </summary>
	private interface IObjectPool
	{
	}

	/// <summary>
	/// Initializes the object pool manager.
	/// </summary>
	public void Initialize()
	{
		pools = new Dictionary<System.Type, IObjectPool>();

		CreatePool<Account>(100);
		CreatePool<Character>(100);
		CreatePool<ClothingTexture>(20);
		CreatePool<ClothingMesh>(20);
		CreatePool<PokeBall>(20);
		CreatePool<Restorative>(20);
		CreatePool<InflictedStatus>(50);
		CreatePool<Machine>(20);
		CreatePool<Unique>(20);
		CreatePool<Recipe>(20);
		CreatePool<Ingredient>(50);
		CreatePool<Dye>(20);
		CreatePool<Common>(20);
		CreatePool<ColoredTexture>(100);
	}

	/// <summary>
	/// Resizes the object pool of the given type.
	/// </summary>
	/// <typeparam name="T">The type of objects that the object pool keeps track of.</typeparam>
	/// <param name="capacity">The new amount of inactive objects the pool will store.</param>
	public void Resize<T>(int capacity) where T : new()
	{
		IndividualObjectPool<T> pool = null;
		try
		{
			pool = (IndividualObjectPool<T>)pools[typeof(T)];
		}
		catch (KeyNotFoundException)
		{
			throw new KeyNotFoundException("Object pool of type " + typeof(T).ToString() + " has not been created");
		}

		if (pool != null)
		{
			pool.Capacity = capacity;
		}
	}

	/// <summary>
	/// Retrieves a new object from the appropriate object pool.
	/// </summary>
	/// <typeparam name="T">The type of object to retrieve.</typeparam>
	/// <returns>A new object of the given type.</returns>
	public T New<T>() where T : new()
	{
		IndividualObjectPool<T> pool = null;
		try
		{
			pool = (IndividualObjectPool<T>)pools[typeof(T)];
		}
		catch (KeyNotFoundException)
		{
			throw new KeyNotFoundException("Object pool of type " + typeof(T).ToString() + " has not been created");
		}

		if (pool != null)
		{
			return pool.New();
		}
		else
		{
			return default(T);
		}
	}

	/// <summary>
	/// Marks the given object as inactive in it's appropriate object pool.
	/// </summary>
	/// <typeparam name="T">The type of object to mark inactive.</typeparam>
	/// <param name="objectToDestroy">The object to mark inactive.</param>
	public void Destroy<T>(ref T objectToDestroy) where T : new()
	{
		IndividualObjectPool<T> pool = null;
		try
		{
			pool = (IndividualObjectPool<T>)pools[typeof(T)];
		}
		catch (KeyNotFoundException)
		{
			throw new KeyNotFoundException("Object pool of type " + typeof(T).ToString() + " has not been created");
		}

		if (pool != null)
		{
			pool.AddInactive(ref objectToDestroy);
		}

		objectToDestroy = default(T);
	}

	private void CreatePool<T>(int capacity) where T : new()
	{
		pools.Add(typeof(T), new IndividualObjectPool<T>(capacity));
	}

	/// <summary>
	/// Keeps track of inactive objects for recycling.
	/// </summary>
	/// <typeparam name="T">The type of object to recycle.</typeparam>
	private class IndividualObjectPool<T> : IObjectPool where T : new()
	{
		private List<T> pool;
		private int capacity;

		/// <summary>
		/// Initializes a new instance of the <see cref="IndividualObjectPool{T}" /> class.
		/// </summary>
		/// <param name="capacity">The initial amount of inactive objects the object pool will store.</param>
		public IndividualObjectPool(int capacity)
		{
			this.pool = new List<T>();
			this.capacity = capacity;
		}

		/// <summary>
		/// Gets the amount of inactive objects in the pool.
		/// </summary>
		public int Size
		{
			get
			{
				return pool.Count;
			}
		}

		/// <summary>
		/// Gets or sets the amount of inactive objects that will be stored in the pool.
		/// </summary>
		public int Capacity
		{
			get
			{
				return capacity;
			}
			set
			{
				if (value != capacity)
				{
					capacity = value;
					if (pool.Count > capacity)
					{
						pool.RemoveRange(0, pool.Count - capacity);
					}
				}
			}
		}

		/// <summary>
		/// Retrieves a new object from the pool.
		/// </summary>
		/// <returns>The new object.</returns>
		public T New()
		{
			if (pool.Count == 0)
			{
				return new T();
			}
			else
			{
				T newObject = pool[0];
				pool.Remove(newObject);
				return newObject;
			}
		}

		/// <summary>
		/// Adds the given object to the list of inactive objects in the pool.
		/// </summary>
		/// <param name="inactiveObject">The object to mark as inactive.</param>
		public void AddInactive(ref T inactiveObject)
		{
			if (pool.Count < capacity)
			{
				pool.Add(inactiveObject);
			}

			inactiveObject = default(T);
		}
	}
}
