using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A static class for handling default positions
/// </summary>
public static class DefaultWindowPositions
{
	public static Dictionary<int, float[]> GetDefaultWindowPositons(HUD hud)
	{
		Dictionary<int, float[]> windows = new Dictionary<int, float[]>();

		if (hud == HUD.PokemonHUD)
		{
			windows.Add((int)PokemonHUD.HealthBarHorizontal, DockingPositionHelper.GetAnchors(DockingPosition.BottomLowerMiddleCenter));
			windows.Add((int)PokemonHUD.StaminaBarHorizontal, DockingPositionHelper.GetAnchors(DockingPosition.BottomLowerCenter));
		}
		else if (hud == HUD.PlayerHUD)
		{
			// Nothing yet
		}

		return windows;
	}
}