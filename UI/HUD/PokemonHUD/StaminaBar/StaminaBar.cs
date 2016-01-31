using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class for showing the current health
/// </summary>
public class StaminaBar : HUDPrefab
{
	[SerializeField]
	private Text text;
	[SerializeField]
	private Slider slider;

	private void Awake()
	{
		slider.value = 1;
		text.text = "100/100";
	}

	private void Update()
	{
		if (Character.Main.ActivePokemon != null)
		{
			slider.value = Character.Main.ActivePokemon.CurrentStamina / Character.Main.ActivePokemon.MaxStamina;
			text.text = ((int)(slider.value * Character.Main.ActivePokemon.MaxStamina)) + "/" + Character.Main.ActivePokemon.MaxStamina;
		}
	}
}