using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Allows tabbing from one input field to another.
/// </summary>
public class InputFieldTabbing : PPBehaviour, IUsesKeyCode
{
	[SerializeField]
	private InputField target;

	/// <summary>
	/// Switches to the next window
	/// </summary>
	public void OnKeyCodeDown(int key, float strength, InputLayer layer)
	{
		if (key == (int)KeyCode.Tab)
		{
			target.Select();
		}
	}

	public void OnKeyCode(int key, float strength, InputLayer layer)
	{
	}

	private void Awake()
	{
		Input.Instance.SubscribeToKeyCode(new int[] { (int)KeyCode.Tab }, this);
	}
}