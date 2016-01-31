using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// Provides a unique fingerprint for a device.
/// </summary>
public class Fingerprint
{
	private static string uniqueIdentifier;

	/// <summary>
	/// Gets: A string that uniquely identifies a device.
	/// </summary>
	public static string UniqueIdentifier
	{
		get
		{
			if (string.IsNullOrEmpty(uniqueIdentifier))
			{
				uniqueIdentifier = SystemInfo.deviceUniqueIdentifier;
			}
			return uniqueIdentifier;
		}
	}
}