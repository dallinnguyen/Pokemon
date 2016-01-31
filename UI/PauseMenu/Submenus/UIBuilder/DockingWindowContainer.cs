using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// A window when in docking mode
/// </summary>
public class DockingWindowContainer : HUDContainer
{
	[SerializeField]
	private new Image image;
	private DockingPosition dockingPosition;
	private OpenWindowCallback callback;

	protected override void OnClick()
	{
		callback.DockingPosition = dockingPosition;
		callback.WindowID = WindowId;
		UIController.Instance.OpenPopup<int>(Popup.OpenWindowDialog, callback);
	}

	public void Initialize(HUDPrefab content, ScreenSpace screenSpace, int windowId, Vector2 anchorMin, Vector2 anchorMax, float windowAlpha, HUD hud, DockingPosition dockingPosition, OpenWindowCallback callback)
	{
		base.Initialize(content, screenSpace, windowId, anchorMin, anchorMax, windowAlpha, hud);
		this.dockingPosition = dockingPosition;
		this.callback = callback;
	}
}