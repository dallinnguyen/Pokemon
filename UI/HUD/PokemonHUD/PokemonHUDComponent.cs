using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// The top class for the PokemonHUD prefab
/// </summary>
public class PokemonHUDComponent : HUDComponent
{
	protected override string GetHUDName()
	{
		return "PokemonHUD";
	}
}