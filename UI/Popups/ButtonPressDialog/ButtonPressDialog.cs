using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// The main class of the button press dialog
/// </summary>
public class ButtonPressDialog : PopupComponent
{
	private IntArrayComparer comparer = new IntArrayComparer();
	private Key key;

	[SerializeField]
	private ButtonPressTimer timer;
	[SerializeField]
	private PressedButtons pressedButtons;

	protected override void OnPopupOpened<T>(ICallback<T> callback)
	{
		timer.Initialize(callback, pressedButtons);
		key = ((ButtonPressCallback)callback).Key;
		timer.OnActivated();
	}

	protected override void OnPopupClosed()
	{
	}

	private void Update()
	{
		int[] pressedKeys = Input.Instance.GetCurrentlyPressedButtons();
		bool containsJoystick = false;
		bool containsKeyboard = false;
		bool containsMouse = false;

		if (pressedKeys.Length == 0)
		{
			return;
		}

		List<int> buttons = new List<int>();

		foreach (int key in pressedKeys)
		{
			if (key < 500)
			{
				if (!containsMouse && !containsKeyboard && Enum.GetName(typeof(KeyCode), (KeyCode)key).StartsWith("JoystickButton"))
				{
					buttons.Add(key);
					containsJoystick = true;
				}
				else if (!containsMouse && !containsJoystick && !Enum.GetName(typeof(KeyCode), (KeyCode)key).StartsWith("Joystick"))
				{
					buttons.Add(key);
					containsKeyboard = true;
				}
			}
			else
			{
				int[] mouseKeys = AxisKeyToAxisMapping.GetMouseKeys();
				bool b = false;
				foreach (int k in mouseKeys)
				{
					if (buttons.Contains(k))
					{
						b = true;
						break;
					}
				}
				if (buttons.Count == 0 && !b && key >= mouseKeys[0] && key <= mouseKeys[mouseKeys.Length - 1])
				{
					buttons.Add(key);
					containsMouse = true;
				}
				int[] controllerKeys = AxisKeyToAxisMapping.GetJoystickKeys();
				if (!b && key >= controllerKeys[0] && key <= controllerKeys[controllerKeys.Length - 1])
				{
					buttons.Add(key);
					containsJoystick = true;
				}
			}
		}

		pressedKeys = new int[buttons.Count];
		buttons.CopyTo(pressedKeys);

		int binding = 0;

		if (containsJoystick)
		{
			binding = 1;
		}
		if (!comparer.Equals(pressedKeys, Input.Instance.GetKeyMapping(this.key, binding)) && Input.Instance.ContainsKeyMapping(pressedKeys, binding))
		{
			return;
		}

		if (!comparer.Equals(pressedKeys, pressedButtons.PressedKeys))
		{
			pressedButtons.Binding = binding;
			pressedButtons.PressedKeys = pressedKeys;
			pressedButtons.UpdateImages();
			timer.ResetTimer();
		}
	}
}