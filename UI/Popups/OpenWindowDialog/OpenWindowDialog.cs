using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// The dialog where you can choose your desired window
/// </summary>
public class OpenWindowDialog : PopupComponent
{
	[SerializeField]
	private RectTransform list;
	private ListEntry listEntryPrefab;
	private OpenWindowCallback callback;

	/// <summary>
	/// Creates a list with all available windows
	/// </summary>
	protected override void OnPopupOpened<T>(ICallback<T> callback)
	{
		this.callback = (OpenWindowCallback)callback;

		listEntryPrefab = Resources.Load<ListEntry>("GUI/Prefabs/Popups/OpenWindowDialog/ListEntry");

		list.sizeDelta = new Vector2(0, (this.callback.AvailableWindows.Length * 30) + ((this.callback.AvailableWindows.Length - 1) * 2));
		list.anchoredPosition = new Vector2(0, -(list.sizeDelta.y / 2));

		for (int i = 0; i < this.callback.AvailableWindows.Length; i++)
		{
			ListEntry entry = Instantiate<ListEntry>(listEntryPrefab);
			if(i == 0)
			{
				entry.Initialize(this.callback, -1, list, Localization.Instance.Localize("gui.remove"));
			}
			else
			{
				entry.Initialize(this.callback, i - 1, list, this.callback.AvailableWindows[i]);
			}
		}
	}

	protected override void OnPopupClosed()
	{
	}
}