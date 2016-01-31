using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class to store the sprites for the buttons
/// </summary>
public class Sprites : PPBehaviourSingleton<Sprites>, IInitializeable
{
	private Sprite[] sprites;

	/// <summary>
	/// Returns the Sprite for the given keycode
	/// </summary>
	/// <param name="keycode">the keycode</param>
	/// <returns>the Sprite and optional the text to be on the image (used for keyboard keys)</returns>
	public KeyValuePair<Sprite, string> GetSprite(int keycode)
	{
		Sprite s = null;
		string str = string.Empty;

		if (keycode < 500)
		{
			string name = Enum.GetName(typeof(KeyCode), (KeyCode)keycode);
			if (name.StartsWith("Joystick"))
			{
				str = string.Empty;
				int joystickButton = int.Parse(name.Substring(name.Length - 1));
				switch (joystickButton)
				{
					case 0: s = sprites[9]; break;
					case 1: s = sprites[10]; break;
					case 2: s = sprites[11]; break;
					case 3: s = sprites[12]; break;
					case 4: s = sprites[24]; break;
					case 5: s = sprites[17]; break;
					case 6: s = sprites[8]; break;
					case 7: s = sprites[31]; break;
					case 8: s = sprites[19]; break;
					case 9: s = sprites[26]; break;
				}
			}
			else
			{
				s = sprites[0];
				string keyName = Enum.GetName(typeof(KeyCode), (KeyCode)keycode);
				if (keyName.StartsWith("Keypad"))
				{
					keyName = keyName.Substring(6);
					if (keyName.Length == 1)
					{
						str = keyName;
					}
					else
					{
						str = Localization.Instance.Localize("keyboard." + keyName);
					}
				}
				else if (name.Length == 1)
				{
					str = keyName;
				}
				else if (name == "Mouse0")
				{
					s = sprites[2];
					str = string.Empty;
				}
				else if (name == "Mouse1")
				{
					s = sprites[3];
					str = string.Empty;
				}
				else
				{
					s = sprites[1];
					str = Localization.Instance.Localize("keyboard." + keyName.ToLower());
				}
			}
		}
		else // AxisKeys
		{
			string name = Enum.GetName(typeof(AxisKey), (AxisKey)keycode);
			switch (name)
			{
				case "HatYp": s = sprites[16]; break;
				case "HatXm": s = sprites[14]; break;
				case "HatXp": s = sprites[15]; break;
				case "HatYm": s = sprites[13]; break;
				case "LSYp": s = sprites[19]; break;
				case "LSXm": s = sprites[20]; break;
				case "LSXp": s = sprites[21]; break;
				case "LSYm": s = sprites[22]; break;
				case "LT": s = sprites[23]; break;
				case "RSYp": s = sprites[29]; break;
				case "RSXm": s = sprites[27]; break;
				case "RSXp": s = sprites[28]; break;
				case "RSYm": s = sprites[26]; break;
				case "RT": s = sprites[30]; break;
				case "MouseXp": s = sprites[6]; break;
				case "MouseYp": s = sprites[7]; break;
				case "MouseXm": s = sprites[5]; break;
				case "MouseYm": s = sprites[4]; break;
			}
		}
		return new KeyValuePair<Sprite, string>(s, str);
	}

	/// <summary>
	/// Loads the Sprites from the folder
	/// </summary>
	public void Initialize()
	{
		sprites = Resources.LoadAll<Sprite>("GUI/Images/Buttons");
	}
}