using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// The top class for the PlayerHUD prefab
/// </summary>
public class PlayerHUDComponent : HUDComponent
{
	protected override string GetHUDName()
	{
		return "PlayerHUD";
	}
}