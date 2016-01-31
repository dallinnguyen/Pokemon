using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// All diffenent HUD types have to derive from this class (like PokemonHUD, PlayerHUD, etc)
/// </summary>
public abstract class HUDComponent : PPBehaviour
{
	/// <summary>
	/// Used to get the Folder of the Prefabs
	/// </summary>
	/// <returns>must return the name of the folder under GUI/Prefabs where to search</returns>
	protected abstract string GetHUDName();
}