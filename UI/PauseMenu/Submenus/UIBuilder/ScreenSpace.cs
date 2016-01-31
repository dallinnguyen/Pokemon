using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// The part of the UI Builder, where you see your actual GUI configuration
/// </summary>
public class ScreenSpace : PPBehaviour
{
	[SerializeField]
	private float windowAlpha = 0.2f;
	[SerializeField]
	private CanvasGroup openWindowButton;

	private HUD hud = HUD.PlayerHUD;

	/// <summary>
	/// The container Prefab (screen prefabs will be placed in the container)
	/// </summary>
	private HUDContainer containerPrefab;

	/// <summary>
	/// Contains all active screen windows, the key is the window ID in free UI mode, and the docking position in docking mode
	/// </summary>
	private List<HUDContainer> hudWindows;

	private OpenWindowCallback callback;
	private bool dockingMode = false;

	/// <summary>
	/// The alpha value of windows that are not highlighted, used for mixing with the black background of the containers
	/// </summary>
	public float WindowAlpha
	{
		get
		{
			return windowAlpha;
		}
	}

	public HUD HUD
	{
		get
		{
			return hud;
		}
	}

	public OpenWindowCallback Callback
	{
		get
		{
			return callback;
		}
	}

	/// <summary>
	/// Used with the docking mode toggle
	/// </summary>
	public void ToggleDockingMode()
	{
		if (dockingMode)
		{
			openWindowButton.interactable = true;
			openWindowButton.alpha = 1;
			dockingMode = false;

			containerPrefab = Resources.Load<ResizeableAndDraggableWindow>("GUI/Prefabs/PauseMenu/UIBuilder/ResizeContainer");

			// Check which windows are empty and which are not
			List<HUDContainer> activeWindows = new List<HUDContainer>();
			foreach (HUDContainer w in hudWindows)
			{
				if (w.Content != null)
				{
					activeWindows.Add(w);
					w.Content.transform.SetParent(transform, false);
				}
				Destroy(w.gameObject);
			}

			// Take the content of the old docking windows and attach them to resizeable windows
			hudWindows = new List<HUDContainer>();
			for (int i = 0; i < activeWindows.Count; i++)
			{
				HUDContainer w = activeWindows[i];
				ResizeableAndDraggableWindow window = Instantiate<ResizeableAndDraggableWindow>((ResizeableAndDraggableWindow)containerPrefab);
				window.Initialize(w.Content, this, i, w.RectTransform.anchorMin, w.RectTransform.anchorMax, windowAlpha, HUD);
				w.Content.transform.SetParent(window.transform, false);
				hudWindows.Add(window);
				Destroy(w.gameObject);
			}
		}
		else
		{
			dockingMode = true;
			openWindowButton.interactable = false;
			openWindowButton.alpha = 0.5f;

			containerPrefab = Resources.Load<DockingWindowContainer>("GUI/Prefabs/PauseMenu/UIBuilder/DockingContainer");
			DockingPosition[] dockingPositions = (DockingPosition[])Enum.GetValues(typeof(DockingPosition));

			// Check which windows are at a docking position
			Dictionary<DockingPosition, HUDContainer> activeWindows = new Dictionary<DockingPosition, HUDContainer>();
			foreach (HUDContainer w in hudWindows)
			{
				bool added = false;
				foreach (DockingPosition d in dockingPositions)
				{
					float[] anchors = DockingPositionHelper.GetAnchors(d);
					if (!added && anchors[0] == w.RectTransform.anchorMin.x && anchors[1] == w.RectTransform.anchorMax.x && anchors[2] == w.RectTransform.anchorMin.y && anchors[3] == w.RectTransform.anchorMax.y)
					{
						activeWindows.Add(d, w);
						added = true;
					}
				}
				if (added)
				{
					w.Content.transform.SetParent(transform, false);
				}
				else
				{
					Destroy(w.gameObject);
				}
			}

			hudWindows = new List<HUDContainer>();

			// Instanciates the docking windows
			for (int i = 0; i < dockingPositions.Length; i++)
			{
				DockingPosition d = (DockingPosition)i;
				DockingWindowContainer window = Instantiate<DockingWindowContainer>((DockingWindowContainer)containerPrefab);
				float[] anchors = DockingPositionHelper.GetAnchors(d);

				if (activeWindows.ContainsKey(d))
				{
					activeWindows[d].Content.transform.SetParent(window.transform, false);
					window.Initialize(activeWindows[d].Content, this, i, new Vector2(anchors[0], anchors[2]), new Vector2(anchors[1], anchors[3]), windowAlpha, hud, d, callback);
					Destroy(activeWindows[d].gameObject);
				}
				else
				{
					window.Initialize(null, this, i, new Vector2(anchors[0], anchors[2]), new Vector2(anchors[1], anchors[3]), windowAlpha, hud, d, callback);
				}

				hudWindows.Add(window);
			}
		}
	}

	/// <summary>
	/// Switches between UI modes
	/// </summary>
	/// <returns>The new ui mode</returns>
	public HUD ToggleUiMode()
	{
		string[] s = null;
		if (hud == HUD.PlayerHUD)
		{
			hud = HUD.PokemonHUD;
			s = Enum.GetNames(typeof(PokemonHUD));
		}
		else
		{
			hud = HUD.PlayerHUD;
			s = Enum.GetNames(typeof(PlayerHUD));
		}

		string[] names = new string[s.Length + 1];
		names[0] = Localization.Instance.Localize("gui.remove");
		for (int i = 1; i < names.Length; i++)
		{
			names[i] = Localization.Instance.Localize("gui." + hud.ToString().ToLower() + ".windows." + s[i - 1].ToLower());
		}
		callback = new OpenWindowCallback(this, hud, names);

		Reload();

		return hud;
	}

	public void DeleteWindow(int windowId)
	{
		if(windowId >= 0)
		{
			if(!dockingMode)
			{
				Destroy(hudWindows[windowId].gameObject);
				hudWindows.RemoveAt(windowId);

				for (int i = 0; i < hudWindows.Count; i++)
				{
					hudWindows[i].WindowId = i;
				}
			}
			else
			{
				if(hudWindows[windowId].Content != null)
				{
					Destroy(hudWindows[windowId].Content.gameObject);
				}
			}
		}
	}

	public void OpenWindow(int window, int windowID)
	{
		if(window >= 0)
		{
			HUDPrefab p = Instantiate<HUDPrefab>(HUDController.Instance.HUDPrefabs[(int)hud][window]);
			if (hudWindows.Count - 1 >= windowID)
			{
				if(hudWindows[windowID].Content != null)
				{
					Destroy(hudWindows[windowID].Content.gameObject);
				}
				p.transform.SetParent(hudWindows[windowID].transform, false);
				hudWindows[windowID].SetContent(p, windowID);
			}
			else
			{
				HUDContainer c = Instantiate<HUDContainer>(containerPrefab);
				float[] anchors = DockingPositionHelper.GetAnchors(DockingPosition.TopLowerCenter);
				c.Initialize(p, this, windowID, new Vector2(anchors[0], anchors[2]), new Vector2(anchors[1], anchors[3]), windowAlpha, hud);
				hudWindows.Add(c);
			}
		}
	}

	public void PutWindowInFront(int windowId)
	{
		if(windowId < hudWindows.Count && !dockingMode)
		{
			HUDContainer container = hudWindows[windowId];
			container.transform.SetParent(null, false);
			container.transform.SetParent(transform, false);
			hudWindows.RemoveAt(windowId);
			hudWindows.Add(container);

			for(int i = 0; i < hudWindows.Count; i++)
			{
				hudWindows[i].WindowId = i;
				hudWindows[i].Content.WindowID = i;
			}
		}
	}

	public void Reload()
	{
		HUDPrefab[] prefabs = HUDController.Instance.GetHUDWindows(hud);

		if(!dockingMode)
		{
			DestroyWindows();

			for (int i = 0; i < prefabs.Length; i++)
			{
				HUDContainer c = Instantiate<HUDContainer>(containerPrefab);
				Vector2 min = prefabs[i].RectTransform.anchorMin;
				Vector2 max = prefabs[i].RectTransform.anchorMax;
				prefabs[i].RectTransform.anchorMin = new Vector2(0, 0);
				prefabs[i].RectTransform.anchorMax = new Vector2(1, 1);
				c.Initialize(prefabs[i], this, i, min, max, windowAlpha, hud);
				hudWindows.Add(c);
			}
		}
		else
		{
			DestroyContent();

			for(int i = 0; i < prefabs.Length; i++)
			{
				bool available = false;
				float[] anchors = new float[] { prefabs[i].RectTransform.anchorMin.x, prefabs[i].RectTransform.anchorMax.x, prefabs[i].RectTransform.anchorMin.y, prefabs[i].RectTransform.anchorMax.y };
				DockingPosition p = DockingPositionHelper.GetDockingPosition(anchors, out available);
				prefabs[i].RectTransform.anchorMin = new Vector2(0, 0);
				prefabs[i].RectTransform.anchorMax = new Vector2(1, 1);
				hudWindows[(int)p].SetContent(prefabs[i], (int)p);
			}
		}
	}

	public int GetNewID()
	{
		int i = PlayerPrefs.GetInt("ui.hud." + (int)HUD + ".count");
		PlayerPrefs.SetInt("ui.hud." + (int)HUD + ".count", i + 1);
		PlayerPrefs.Save();
		return i;
	}

	private void Awake()
	{
		hudWindows = new List<HUDContainer>();
		containerPrefab = Resources.Load<ResizeableAndDraggableWindow>("GUI/Prefabs/PauseMenu/UIBuilder/ResizeContainer");

		string[] s = Enum.GetNames(typeof(PlayerHUD));
		string[] names = new string[s.Length + 1];
		names[0] = Localization.Instance.Localize("gui.remove");
		for (int i = 1; i < names.Length; i++)
		{
			names[i] = Localization.Instance.Localize("gui." + hud.ToString().ToLower() + ".windows." + s[i - 1].ToLower());
		}
		callback = new OpenWindowCallback(this, hud, names);

		Reload();
	}

	/// <summary>
	/// Saves the currently opened HUD windows into the PlayerPrefs
	/// </summary>
	private void OnDestroy()
	{
		List<HUDContainer> c = new List<HUDContainer>(hudWindows);
		foreach (HUDContainer h in c)
		{
			if (h.Content == null)
			{
				hudWindows.Remove(h);
			}
		}

		HUDPrefab[] hud = new HUDPrefab[hudWindows.Count];

		for (int i = 0; i < hudWindows.Count; i++)
		{
			hud[i] = hudWindows[i].Content;
			hud[i].RectTransform.anchorMin = hudWindows[i].RectTransform.anchorMin;
			hud[i].RectTransform.anchorMax = hudWindows[i].RectTransform.anchorMax;
			hud[i].WindowID = i;
		}

		HUDController.Instance.ApplyHudChanges(hud, this.hud);
	}

	private void DestroyWindows()
	{
		foreach (HUDContainer c in hudWindows)
		{
			Destroy(c.gameObject);
		}
		hudWindows = new List<HUDContainer>();
	}

	private void DestroyContent()
	{
		foreach (HUDContainer c in hudWindows)
		{
			if(c.Content != null)
			{
				Destroy(c.Content.gameObject);
			}
		}
	}
}