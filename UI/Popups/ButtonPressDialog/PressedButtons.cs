using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Stores and shows all Buttons pressed
/// </summary>
public class PressedButtons : PPBehaviour
{
	[SerializeField]
	private ButtonSettings[] buttonSettings;

	/// <summary>
	/// Gets or sets: The currently pressed keys
	/// </summary>
	public int[] PressedKeys { get; set; }

	/// <summary>
	/// Gets or sets: 0 for keyboard, 1 for Joystick
	/// </summary>
	public int Binding { get; set; }

	/// <summary>
	/// Updates the texts and the images of the buttons
	/// </summary>
	public void UpdateImages()
	{
		foreach (ButtonSettings s in buttonSettings)
		{
			s.Text.text = string.Empty;
			s.Image.sprite = null;
		}
		for (int i = 0; i < PressedKeys.Length; i++)
		{
			KeyValuePair<Sprite, string> k = Sprites.Instance.GetSprite(PressedKeys[i]);
			buttonSettings[i].Image.sprite = k.Key;
			buttonSettings[i].Text.text = k.Value;
		}
	}

	private void Awake()
	{
		PressedKeys = new int[0];
	}
}