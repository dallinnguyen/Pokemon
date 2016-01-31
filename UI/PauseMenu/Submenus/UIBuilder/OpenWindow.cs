using UnityEngine;

public class OpenWindow : Button
{
	[SerializeField]
	private ScreenSpace screenSpace;

	protected override void OnActivated()
	{
		screenSpace.Callback.WindowID = -1;
		UIController.Instance.OpenPopup<int>(Popup.OpenWindowDialog, screenSpace.Callback);
	}
}