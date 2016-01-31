using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// A Button that triggers default positioning
/// </summary>
public class DefaultPositions : Button
{
	[SerializeField]
	private ScreenSpace screenSpace;

	/// <summary>
	/// Toggles the default window placements
	/// </summary>
	protected override void OnActivated()
	{
		HUDController.Instance.ReinstanciateWindowsInDefaultPositons(screenSpace.HUD);
		screenSpace.Reload();
	}
}