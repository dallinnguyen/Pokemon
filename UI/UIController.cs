using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that controls the overall UI.
/// </summary>
public class UIController : PPBehaviourSingleton<UIController>, IInitializeable
{
	private UIWindowComponent[] uiWindows;
	private PopupComponent[] popups;
	private UIWindowComponent[] activatedUIWindows;
	private PopupComponent[] activatedPopups;
	private UI ui;

	private InputLayer lastLayer;

	/// <summary>
	/// Gets or sets a value indicating whether the loading screen is currently playing
	/// </summary>
	public bool LoadingScreen
	{
		get;
		set;
	}

	/// <summary>
	/// Initializes the UI controller singleton.
	/// </summary>
	public void Initialize()
	{
		ui = Instantiate<UI>(Resources.Load<UI>("GUI/Prefabs/UI"));

		uiWindows = Resources.LoadAll<UIWindowComponent>("GUI/Prefabs");
		popups = Resources.LoadAll<PopupComponent>("GUI/Prefabs/Popups");

		activatedUIWindows = new UIWindowComponent[uiWindows.Length];
		activatedPopups = new PopupComponent[popups.Length];
	}

	/// <summary>
	/// Fades a splash texture in and out on the screen.
	/// </summary>
	/// <param name="splash">The splash texture to be displayed.</param>
	/// <returns>Enumerator for a Unity Coroutine.</returns>
	public IEnumerator PlaySplash(Texture2D splash)
	{
		yield break;
	}

	/// <summary>
	/// Plays a movie on the screen.
	/// </summary>
	/// <param name="movie">The movie texture to be displayed.</param>
	/// <returns>Enumerator for a Unity Coroutine.</returns>
	public IEnumerator PlayMovie(MovieTexture movie)
	{
		yield break;
	}

	/// <summary>
	/// Opens a UI window.
	/// </summary>
	/// <param name="window">The UIWindow to open.</param>
	public void OpenUIWindow(UIWindow window)
	{
		int i = (int)window;
		if (activatedUIWindows[i] == null)
		{
			activatedUIWindows[i] = Instantiate<UIWindowComponent>(uiWindows[i]);
			activatedUIWindows[i].transform.SetParent(UI.Instance.Canvas.transform, false);
		}
	}

	/// <summary>
	/// Closes a UI window.
	/// </summary>
	/// <param name="window">The UI Window to close.</param>
	public void CloseUIWindow(UIWindow window)
	{
		int i = (int)window;
		if (activatedUIWindows[i] != null)
		{
			activatedUIWindows[i].OnWindowClosed();
			Destroy(activatedUIWindows[i].gameObject);
			activatedUIWindows[i] = null;
		}
	}

	/// <summary>
	/// Checks if the given window is opened
	/// </summary>
	/// <param name="window">the window to check</param>
	/// <returns>true if open</returns>
	public bool IsUIWindowOpen(UIWindow window)
	{
		return activatedUIWindows[(int)window] != null;
	}

	/// <summary>
	/// The Method to Open a popup
	/// </summary>
	/// <typeparam name="T">the type of the callback</typeparam>
	/// <param name="popup">The Popup to open</param>
	/// <param name="callback">A callback to be called in the popup</param>
	public void OpenPopup<T>(Popup popup, ICallback<T> callback)
	{
		lastLayer = Input.Instance.ReleasedLayer;
		Input.Instance.LockInputLayersExcept(InputLayer.Popup);
		int i = (int)popup;
		if (activatedPopups[i] == null)
		{
			activatedPopups[i] = Instantiate<PopupComponent>(popups[i]);
			activatedPopups[i].transform.SetParent(ui.Canvas.transform, false);
			activatedPopups[i].OnOpen<T>(callback);
		}
	}

	/// <summary>
	/// Closes the Popup
	/// </summary>
	/// <param name="popup">the popup to close</param>
	public void ClosePopup(Popup popup)
	{
		Input.Instance.LockInputLayersExcept(lastLayer);
		int i = (int)popup;

		if (activatedPopups[i] != null)
		{
			activatedPopups[i].OnClose();
			Destroy(activatedPopups[i].gameObject);
			activatedPopups[i] = null;
		}
	}
}
