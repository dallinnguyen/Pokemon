using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Testing class for a player
/// </summary>
public class Player : PPBehaviour, IUsesKeyDown
{
	private bool isOut = false;

	/// <summary>
	/// Sends and Returns the pokemon
	/// </summary>
	/// <param name="key">the key</param>
	/// <param name="strength">the strength</param>
	/// <param name="layer">the actual layer</param>
	public void OnKeyDown(Key key, float strength, InputLayer layer)
	{
		if (!isOut && key == Key.SendPokemon && layer == InputLayer.Default)
		{
			isOut = true;
			Character.Main.ActivePokemon = new Pokemon();
			HUDController.Instance.HUD = HUD.PokemonHUD;
		}
		else if (isOut && key == Key.ReturnPokemon && layer == InputLayer.Default)
		{
			isOut = false;
			HUDController.Instance.HUD = HUD.PlayerHUD;
			Character.Main.ActivePokemon = null;
		}
	}

	private void Awake()
	{
		Character character = new Character();
		Character.Main = character;
		Input.Instance.SubscribeToKeyDown(new Key[] { Key.SendPokemon, Key.ReturnPokemon }, this);
	}
}