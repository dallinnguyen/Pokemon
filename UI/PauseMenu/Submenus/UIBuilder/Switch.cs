using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Button, that closes the current submenu and opens the parent
/// </summary>
public class Switch : Button
{
	[SerializeField]
	private ScreenSpace screenSpace;
	[SerializeField]
	private LocalizedText text;

	/// <summary>
	/// Called on click, switches the menu
	/// </summary>
	protected override void OnActivated()
	{
		if (screenSpace.ToggleUiMode() == HUD.PlayerHUD)
		{
			text.UnlocalizedContent = "gui.pausemenu.uibuilder.switch.0";
		}
		else
		{
			text.UnlocalizedContent = "gui.pausemenu.uibuilder.switch.1";
		}
	}
}