using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Enum for giving axis (like the triggers on the gamepad) a keycode
/// </summary>
public enum AxisKey
{
	MouseXp = 510,
	MouseYp = 511,
	MouseXm = 512,
	MouseYm = 513,
	MouseWheelp = 514,
	MouseWheelm = 515,
	LSXp = 516,
	LSXm = 517,
	LSYp = 518,
	LSYm = 519,
	RSXp = 520,
	RSXm = 521,
	RSYp = 522,
	RSYm = 523,
	LT = 524,
	RT = 525,
	HatXp = 526,
	HatXm = 527,
	HatYp = 528,
	HatYm = 529,
}

/// <summary>
/// Class for Translating AxisKeys to Unity Axis
/// </summary>
public static class AxisKeyToAxisMapping
{
	/// <summary>
	/// Returns the name of the Axis for the given keycode
	/// </summary>
	/// <param name="code">AxisKey converted to int</param>
	/// <returns>The name of the Axis and if it should be positive or negative</returns>
	public static KeyValuePair<string, bool> GetAxisFor(int code)
	{
		string originalName = Enum.GetName(typeof(AxisKey), (AxisKey)code);

		if (originalName == "LT" || originalName == "RT")
		{
			return new KeyValuePair<string, bool>(originalName, true);
		}

		string name = originalName.Substring(0, originalName.Length - 1);

		bool positive = originalName.Contains("p");

		return new KeyValuePair<string, bool>(name, positive);
	}

	/// <summary>
	/// Returns all Mouse related AxisKeys
	/// </summary>
	/// <returns>int[] axiskeycodes</returns>
	public static int[] GetMouseKeys()
	{
		return new int[] { 510, 511, 512, 513, 514, 515 };
	}

	/// <summary>
	/// Returns all Joystick related AxisKeys
	/// </summary>
	/// <returns>int[] axiskeycodes</returns>
	public static int[] GetJoystickKeys()
	{
		return new int[] { 516, 517, 518, 519, 520, 521, 522, 523, 524, 525, 526, 527, 528, 529 };
	}

	/// <summary>
	/// Returns the highest value in the enum
	/// </summary>
	/// <returns>the keycode</returns>
	public static int GetMaxKey()
	{
		return 529;
	}

	/// <summary>
	/// Retruns the first element of the enum
	/// </summary>
	/// <returns>the offset</returns>
	public static int GetOffset()
	{
		return 510;
	}
}