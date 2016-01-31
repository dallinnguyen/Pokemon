using System;
using UnityEngine;

/// <summary>
/// The callback method for the button press dialog
/// </summary>
public class ButtonPressCallback : ICallback<PressedButtons>
{
	private ChangeKeybindings changeKeybindings;

	/// <summary>
	/// Initializes a new instance of the <see cref="ButtonPressCallback" /> class.
	/// </summary>
	/// <param name="changeKeybindings">the button who is opening the popup</param>
	/// <param name="key">the key of the button</param>
	public ButtonPressCallback(ChangeKeybindings changeKeybindings, Key key)
	{
		this.changeKeybindings = changeKeybindings;
		this.Key = key;
	}

	/// <summary>
	/// Gets: The key
	/// </summary>
	public Key Key { get; private set; }

	/// <summary>
	/// Calls the callback method in the ChangeKeybindings class
	/// </summary>
	/// <param name="arg">the pressedButtons with the wanted information</param>
	public void Method(PressedButtons arg)
	{
		changeKeybindings.OnTimerEnded(arg.PressedKeys, arg.Binding);
	}
}