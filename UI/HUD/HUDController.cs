using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Used for handling all HUD elements 
/// </summary>
public class HUDController : PPBehaviourSingleton<HUDController>, IInitializeable
{
	private HUD hud = HUD.None;
	private List<string[]> availableWindows;

	/// <summary>
	/// All possible HUD elements like health bar, map, etc
	/// </summary>
	private List<HUDPrefab[]> hudPrefabs;

	/// <summary>
	/// Contains all active hud windows, the index is the window ID
	/// </summary>
	private HUDPrefab[] hudWindows;

	public HUD HUD
	{
		get
		{
			return hud;
		}
		set
		{
			hud = value;
			LoadWindows();
			SaveHUD();
		}
	}

	public List<HUDPrefab[]> HUDPrefabs
	{
		get
		{
			return hudPrefabs;
		}
	}

	public void ReinstanciateWindowsInDefaultPositons(HUD hud)
	{
		Dictionary<int, float[]> windows = DefaultWindowPositions.GetDefaultWindowPositons(hud);
		PlayerPrefs.SetInt("ui.hud." + (int)hud + ".edited", 1);

		if (hud == this.hud)
		{
			DestroyWindows();
			int counter = 0;
			hudWindows = new HUDPrefab[windows.Count];

			foreach (KeyValuePair<int, float[]> entry in windows)
			{
				HUDPrefab hudWindow = Instantiate<HUDPrefab>(hudPrefabs[(int)hud][entry.Key]);
				hudWindow.Initialize(counter, entry.Value, UI.Instance.HUDParent, entry.Key);
				hudWindows[counter] = hudWindow;

				counter++;
			}

			SaveHUD();
		}
		else
		{
			SaveHUD(hud, windows);
		}
	}

	/// <summary>
	/// Refreshes the GUI layout and saves it
	/// Important: use this only on close, it will move every HUD window in the dictionary, so you won't be able to see them on the old location any more!
	/// </summary>
	/// <param name="hudWindows">The new Dictionary of HUDWindows (the index has to be the windowID)</param>
	public void ApplyHudChanges(HUDPrefab[] hudWindows, HUD hud)
	{
		PlayerPrefs.SetInt("ui.hud." + (int)hud + ".edited", 1);
		// Check for mistakes in the windowIDs
		for (int i = 0; i < hudWindows.Length; i++)
		{
			if (hudWindows[i] == null)
			{
				throw new Exception("Error: No consecutive window ids");
			}
		}

		if (hud == this.hud)
		{
			DestroyWindows();

			this.hudWindows = hudWindows;
			foreach (HUDPrefab w in hudWindows)
			{
				w.ApplyHUDChanges(UI.Instance.HUDParent, 1);
			}

			SaveHUD();
		}
		else
		{
			SaveHUD(hud, hudWindows);
		}
	}

	/// <summary>
	/// Retruns an array of freshly instanciated windows in the right position and scale. Assign a new parent afterwards!
	/// </summary>
	public HUDPrefab[] GetHUDWindows(HUD hud)
	{
		HUDPrefab[] windows = null;

		if (!PlayerPrefs.HasKey("ui.hud." + (int)hud + ".edited"))
		{
			Dictionary<int, float[]> p = DefaultWindowPositions.GetDefaultWindowPositons(hud);
			windows = new HUDPrefab[p.Count];
			int counter = 0;

			foreach (KeyValuePair<int, float[]> entry in p)
			{
				windows[counter] = Instantiate<HUDPrefab>(hudPrefabs[(int)hud][entry.Key]);
				windows[counter].Initialize(counter, entry.Value, UI.Instance.HUDParent, entry.Key);

				counter++;
			}
		}
		else
		{
			int length = PlayerPrefs.GetInt("ui.hud." + (int)hud + ".count");
			windows = new HUDPrefab[length];

			for (int i = 0; i < windows.Length; i++)
			{
				int window = PlayerPrefs.GetInt("ui.hud." + (int)hud + "." + i);
				windows[i] = Instantiate<HUDPrefab>(hudPrefabs[(int)hud][window]);

				float[] anchors = new float[4];

				anchors[0] = PlayerPrefs.GetFloat("ui.hud." + (int)hud + "." + i + ".minX");
				anchors[1] = PlayerPrefs.GetFloat("ui.hud." + (int)hud + "." + i + ".maxX");
				anchors[2] = PlayerPrefs.GetFloat("ui.hud." + (int)hud + "." + i + ".minY");
				anchors[3] = PlayerPrefs.GetFloat("ui.hud." + (int)hud + "." + i + ".maxY");

				windows[i].Initialize(i, anchors, UI.Instance.HUDParent, window);
			}
		}

		return windows;
	}

	public void Initialize()
	{
		hudWindows = new HUDPrefab[0];
		hudPrefabs = new List<HUDPrefab[]>();

		foreach (string s in Enum.GetNames(typeof(HUD)))
		{
			hudPrefabs.Add(Resources.LoadAll<HUDPrefab>("GUI/Prefabs/" + s));
		}

		availableWindows = new List<string[]>();
		availableWindows.Add(null);
		availableWindows.Add(Enum.GetNames(typeof(PlayerHUD)));
		availableWindows.Add(Enum.GetNames(typeof(PokemonHUD)));
	}

	private void SaveHUD()
	{
		PlayerPrefs.SetInt("ui.hud." + (int)hud + ".count", hudWindows.Length);

		for (int i = 0; i < hudWindows.Length; i++)
		{
			HUDPrefab p = hudWindows[i];
			KeyValuePair<int, float[]> windowInformation = p.WindowInformation;
			PlayerPrefs.SetInt("ui.hud." + (int)hud + "." + i, windowInformation.Key);
			PlayerPrefs.SetFloat("ui.hud." + (int)hud + "." + i + ".minX", windowInformation.Value[0]);
			PlayerPrefs.SetFloat("ui.hud." + (int)hud + "." + i + ".maxX", windowInformation.Value[1]);
			PlayerPrefs.SetFloat("ui.hud." + (int)hud + "." + i + ".minY", windowInformation.Value[2]);
			PlayerPrefs.SetFloat("ui.hud." + (int)hud + "." + i + ".maxY", windowInformation.Value[3]);
		}
		PlayerPrefs.Save();
	}

	private void SaveHUD(HUD hud, HUDPrefab[] prefabs)
	{
		PlayerPrefs.SetInt("ui.hud." + (int)hud + ".count", prefabs.Length);

		for (int i = 0; i < prefabs.Length; i++)
		{
			HUDPrefab p = prefabs[i];
			KeyValuePair<int, float[]> windowInformation = p.WindowInformation;
			PlayerPrefs.SetInt("ui.hud." + (int)hud + "." + i, windowInformation.Key);
			PlayerPrefs.SetFloat("ui.hud." + (int)hud + "." + i + ".minX", windowInformation.Value[0]);
			PlayerPrefs.SetFloat("ui.hud." + (int)hud + "." + i + ".maxX", windowInformation.Value[1]);
			PlayerPrefs.SetFloat("ui.hud." + (int)hud + "." + i + ".minY", windowInformation.Value[2]);
			PlayerPrefs.SetFloat("ui.hud." + (int)hud + "." + i + ".maxY", windowInformation.Value[3]);
		}
		PlayerPrefs.Save();
	}

	private void SaveHUD(HUD hud, Dictionary<int, float[]> windows)
	{
		PlayerPrefs.SetInt("ui.hud." + (int)hud + ".count", windows.Count);
		int i = 0;

		foreach (KeyValuePair<int, float[]> entry in windows)
		{
			PlayerPrefs.SetInt("ui.hud." + (int)hud + "." + i, entry.Key);
			PlayerPrefs.SetFloat("ui.hud." + (int)hud + "." + i + ".minX", entry.Value[0]);
			PlayerPrefs.SetFloat("ui.hud." + (int)hud + "." + i + ".maxX", entry.Value[1]);
			PlayerPrefs.SetFloat("ui.hud." + (int)hud + "." + i + ".minY", entry.Value[2]);
			PlayerPrefs.SetFloat("ui.hud." + (int)hud + "." + i + ".maxY", entry.Value[3]);

			i++;
		}
		PlayerPrefs.Save();
	}

	private void LoadWindows()
	{
		DestroyWindows();

		if (!PlayerPrefs.HasKey("ui.hud." + (int)hud + ".edited"))
		{
			ReinstanciateWindowsInDefaultPositons(hud);
		}
		else
		{
			int length = PlayerPrefs.GetInt("ui.hud." + (int)hud + ".count");
			hudWindows = new HUDPrefab[length];
			for (int i = 0; i < length; i++)
			{
				float[] anchors = new float[4];

				anchors[0] = PlayerPrefs.GetFloat("ui.hud." + (int)hud + "." + i + ".minX");
				anchors[1] = PlayerPrefs.GetFloat("ui.hud." + (int)hud + "." + i + ".maxX");
				anchors[2] = PlayerPrefs.GetFloat("ui.hud." + (int)hud + "." + i + ".minY");
				anchors[3] = PlayerPrefs.GetFloat("ui.hud." + (int)hud + "." + i + ".maxY");
				int window = PlayerPrefs.GetInt("ui.hud." + (int)hud + "." + i);
				HUDPrefab hudWindow = Instantiate<HUDPrefab>(hudPrefabs[(int)hud][window]);
				hudWindow.Initialize(i, anchors, UI.Instance.HUDParent, window);
				hudWindows[i] = hudWindow;
			}
		}
	}

	private void DestroyWindows()
	{
		foreach (HUDPrefab p in hudWindows)
		{
			Destroy(p.gameObject);
		}
		hudWindows = new HUDPrefab[0];
	}
}