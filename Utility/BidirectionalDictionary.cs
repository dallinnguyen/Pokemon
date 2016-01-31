using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// A dictionary that works in both dirctions
/// </summary>
/// <typeparam name="T">the type of the key</typeparam>
/// <typeparam name="S">the type of the value</typeparam>
public class BidirectionalDictionary<T, S>
{
	private Dictionary<T, S> keyDictionary;
	private Dictionary<S, T> valueDictionary;

	private IEqualityComparer<T> equalT;
	private IEqualityComparer<S> equalS;

	/// <summary>
	/// Initializes a new instance of the <see cref="BidirectionalDictionary{T,S}" /> class.
	/// </summary>
	public BidirectionalDictionary()
	{
		equalT = EqualityComparer<T>.Default;
		equalS = EqualityComparer<S>.Default;

		keyDictionary = new Dictionary<T, S>(equalT);
		valueDictionary = new Dictionary<S, T>(equalS);
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="BidirectionalDictionary{T,S}" /> class.
	/// </summary>
	/// <param name="equalT">the comparer to be used with the key</param>
	/// <param name="equalS">the comparer to be used with the value</param>
	public BidirectionalDictionary(IEqualityComparer<T> equalT, IEqualityComparer<S> equalS)
	{
		this.equalT = equalT;
		this.equalS = equalS;

		keyDictionary = new Dictionary<T, S>(equalT);
		valueDictionary = new Dictionary<S, T>(equalS);
	}

	/// <summary>
	/// Returns the Value for the given Key
	/// </summary>
	/// <param name="t">the key</param>
	/// <returns>the value</returns>
	public S this[T t]
	{
		get
		{
			return keyDictionary[t];
		}
		set
		{
			keyDictionary.Add(t, value);
			valueDictionary.Add(value, t);
		}
	}

	/// <summary>
	/// Returns the Key for the given Value
	/// </summary>
	/// <param name="s">the value</param>
	/// <returns>the key</returns>
	public T this[S s]
	{
		get
		{
			return valueDictionary[s];
		}
		set
		{
			keyDictionary.Add(value, s);
			valueDictionary.Add(s, value);
		}
	}

	/// <summary>
	/// Adds the given arguments to the dictionary
	/// </summary>
	/// <param name="key">the key to add</param>
	/// <param name="value">the value to add</param>
	public void Add(T key, S value)
	{
		keyDictionary.Add(key, value);
		valueDictionary.Add(value, key);
	}

	/// <summary>
	/// Returns the value for the given key
	/// </summary>
	/// <param name="key">keyto search for</param>
	/// <returns>value to return</returns>
	public S GetValue(T key)
	{
		return keyDictionary[key];
	}

	/// <summary>
	/// Returns the key for the given value
	/// </summary>
	/// <param name="value">value to search for</param>
	/// <returns>key to return</returns>
	public T GetKey(S value)
	{
		return valueDictionary[value];
	}

	/// <summary>
	/// Checks if the dictionary contains the given key
	/// </summary>
	/// <param name="key">key to search for</param>
	/// <returns>true, if key is already in the dictionary</returns>
	public bool ContainsKey(T key)
	{
		return keyDictionary.ContainsKey(key);
	}

	/// <summary>
	/// Checks if the dictionary contains the given value
	/// </summary>
	/// <param name="value">value to search for</param>
	/// <returns>true, if value is already in the dictionary</returns>
	public bool ContainsValue(S value)
	{
		return valueDictionary.ContainsKey(value);
	}

	/// <summary>
	/// Removes the given key (and the corresponding value) from the dictionary
	/// </summary>
	/// <param name="key">key to remove</param>
	public void Remove(T key)
	{
		S value = keyDictionary[key];
		keyDictionary.Remove(key);
		valueDictionary.Remove(value);
	}

	/// <summary>
	/// Removes the given value (and the corresponding key) from the dictionary
	/// </summary>
	/// <param name="value">value to remove</param>
	public void Remove(S value)
	{
		T key = valueDictionary[value];
		keyDictionary.Remove(key);
		valueDictionary.Remove(value);
	}

	/// <summary>
	/// Returns the enumerator of the keyDictionary
	/// </summary>
	/// <returns>the key enumerator</returns>
	public IEnumerator GetEnumerator()
	{
		return keyDictionary.GetEnumerator();
	}
}