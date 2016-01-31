using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// The button script for the Key changing, opens a popup
/// </summary>
public class ChangeKeybindings : PPBehaviour
{
	[SerializeField]
	private RectTransform rectTransform;
	[SerializeField]
	private AppliedButtons[] appliedButtons;
	[SerializeField]
	private LocalizedText localizedText;
	private ButtonPressCallback callback;
	private Vector3 scaled = new Vector3(1.02f, 1.02f, 1.02f);
	private Vector3 normal = new Vector3(1f, 1f, 1f);
	private Key key;

	public Key Key
	{
		get
		{
			return key;
		}
	}

	public void Initialize(Key key, string gameObjectName, Transform parent)
	{
		this.key = key;
		callback = new ButtonPressCallback(this, Key);
		gameObject.name = gameObjectName;
		transform.SetParent(parent, false);
		localizedText.UnlocalizedContent = "keys." + gameObjectName.ToLower();
	}

	/// <summary>
	/// Callback method for the end of the timer, saves the keymapping and closes the popup
	/// </summary>
	/// <param name="keys">key combination to store</param>
	/// <param name="binding">0 for normal binding, 1 for joystick</param>
	public void OnTimerEnded(int[] keys, int binding)
	{
		appliedButtons[binding].ApplyKeys(keys);
		Input.Instance.SetKeyMapping(Key, keys, binding);
		UIController.Instance.ClosePopup(Popup.ButtonPressDialog);
	}

	public void OnSelect()
	{
		rectTransform.localScale = scaled;
	}

	public void OnDeselect()
	{
		rectTransform.localScale = normal;
	}

	public void OnClick()
	{
		UIController.Instance.OpenPopup<PressedButtons>(Popup.ButtonPressDialog, callback);
	}
}
