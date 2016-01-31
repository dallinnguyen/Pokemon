using System;
using UnityEngine;

/// <summary>
/// The button that confirms/closes a popup
/// </summary>
public class ConfirmButton : Button
{
	/// <summary>
	/// Closes the popup
	/// </summary>
	protected override void OnActivated()
	{
		UIController.Instance.ClosePopup(Popup.TextPopup);
	}
}