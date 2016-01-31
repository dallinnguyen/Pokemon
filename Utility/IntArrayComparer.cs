using System;
using System.Collections.Generic;

/// <summary>
/// Compares int[] arrays not only per reference, but also by value
/// </summary>
public class IntArrayComparer : IEqualityComparer<int[]>
{
	/// <summary>
	/// Checks if the two arrays are equal by reference or value
	/// </summary>
	/// <param name="left">first array</param>
	/// <param name="right">seccond array</param>
	/// <returns>true, if equal</returns>
	public bool Equals(int[] left, int[] right)
	{
		if (ReferenceEquals(left, right))
		{
			return true;
		}

		if ((left == null) || (right == null))
		{
			return false;
		}

		if (left.Length != right.Length)
		{
			return false;
		}

		for (int i = 0; i < left.Length; i++)
		{
			if (left[i] != right[i])
			{
				return false;
			}
		}

		return true;
	}

	/// <summary>
	/// Returns a simple Hash for the array
	/// </summary>
	/// <param name="array">the array to hash</param>
	/// <returns>the hash</returns>
	public int GetHashCode(int[] array)
	{
		int hc = array.Length;
		for (int i = 0; i < array.Length; ++i)
		{
			hc = unchecked((hc * 314159) + array[i]);
		}
		return hc;
	}
}