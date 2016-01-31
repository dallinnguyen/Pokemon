using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// A callbak for opening windows in the UI Builder
/// </summary>
public class OpenWindowCallback : ICallback<int>
{
	private ScreenSpace screenSpace;

	/// <summary>
	/// Initializes a new instance of the <see cref="OpenWindowCallback" /> class.
	/// </summary>
	public OpenWindowCallback(ScreenSpace screenSpace, HUD hud, string[] availableWindows)
	{
		this.screenSpace = screenSpace;
		DockingPosition = DockingPosition.TopLowerCenter;
		AvailableWindows = availableWindows;
		UiMode = hud;
		WindowID = -1;
	}

	public DockingPosition DockingPosition { get; set; }

	public int WindowID { get; set; }

	/// <summary>
	/// Gets: An array with the names of all available screen prefabs (health bar, stamina bar, etc)
	/// </summary>
	public string[] AvailableWindows { get; private set; }

	public HUD UiMode { get; private set; }

	/// <summary>
	/// Opens a new container in the screen space
	/// </summary>
	/// <param name="window">the enum value of pokemonUI or playerUI casted to an int</param>
	public void Method(int window)
	{
		if(WindowID == -1)
		{
			screenSpace.OpenWindow(window, screenSpace.GetNewID());
		}
		else if(window >= 0)
		{
			screenSpace.OpenWindow(window, WindowID);
		}
		else
		{
			screenSpace.DeleteWindow(WindowID);
		}
	}
}