using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A class to store and show the currently selected buttons
/// </summary>
public class AppliedButtons : PPBehaviour
{
	[SerializeField]
	private ButtonSettings[] buttonSettings;
	[SerializeField]
	private int keybinding;
	[SerializeField]
	private ChangeKeybindings changeKeybindings;

	/// <summary>
	/// Applies the given key combination to the buttons (store and show them)
	/// </summary>
	public void ApplyKeys(int[] keys)
	{
		foreach (ButtonSettings s in buttonSettings)
		{
			s.Text.text = string.Empty;
			s.Image.sprite = null;
		}
		for (int i = 0; i < keys.Length; i++)
		{
			KeyValuePair<Sprite, string> k = Sprites.Instance.GetSprite(keys[i]);
			buttonSettings[i].Image.sprite = k.Key;
			buttonSettings[i].Text.text = k.Value;
		}
	}

	private void Start()
	{
		int[] keys = Input.Instance.GetKeyMapping(changeKeybindings.Key, keybinding);

		ApplyKeys(keys);
	}
}