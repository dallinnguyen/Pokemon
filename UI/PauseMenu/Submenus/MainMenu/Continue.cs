using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Button that continues the game
/// </summary>
public class Continue : Button
{
	protected override void OnActivated()
	{
		UIController.Instance.CloseUIWindow(UIWindow.PauseMenu);
	}
}
