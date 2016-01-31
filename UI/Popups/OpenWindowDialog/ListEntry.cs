using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Selects the window chosen from the list and closes the popup
/// </summary>
public class ListEntry : Button
{
	[SerializeField]
	private Text text;
	private int window;
	private OpenWindowCallback callback;

	public void Initialize(OpenWindowCallback callback, int window, Transform parent, string text)
	{
		this.window = window;
		this.callback = callback;
		transform.SetParent(parent, false);
		this.text.text = text;
	}

	/// <summary>
	/// Uses the callback and closes the window
	/// </summary>
	protected override void OnActivated()
	{
		callback.Method(window);
		UIController.Instance.ClosePopup(Popup.OpenWindowDialog);
	}
}