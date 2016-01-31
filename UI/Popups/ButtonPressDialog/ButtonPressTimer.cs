using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The timer for the button press dialog
/// </summary>
public class ButtonPressTimer : Timer
{
	private PressedButtons pressedButtons;
	private ButtonPressCallback callback;

	/// <summary>
	/// Initializes the Timer
	/// </summary>
	/// <typeparam name="T">the type of the callback</typeparam>
	/// <param name="callback">the callback to call after end of the timer</param>
	/// <param name="pressedButtons">the pressed buttons to store the keys</param>
	public void Initialize<T>(ICallback<T> callback, PressedButtons pressedButtons)
	{
		this.callback = (ButtonPressCallback)callback;
		this.pressedButtons = pressedButtons;
	}

	/// <summary>
	/// Waits for Input before starting the timer
	/// </summary>
	public new void OnActivated()
	{
		StartCoroutine(WaitForInput());
	}

	/// <summary>
	/// calls the callback
	/// </summary>
	protected override void OnFinished()
	{
		callback.Method(pressedButtons);
	}

	private IEnumerator WaitForInput()
	{
		while (pressedButtons.PressedKeys.Length == 0)
		{
			yield return Wait;
		}
		base.OnActivated();
	}
}