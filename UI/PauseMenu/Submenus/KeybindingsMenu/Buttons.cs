using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class for setting up all keys in the keybindings menu
/// </summary>
public class Buttons : PPBehaviour
{
	private ChangeKeybindings[] changeKeybindings;

	private void Awake()
	{
		string[] names = Enum.GetNames(typeof(Key));

		changeKeybindings = new ChangeKeybindings[names.Length];

		ChangeKeybindings temp = Resources.Load<ChangeKeybindings>("GUI/Prefabs/PauseMenu/KeybindingsMenu/Button");

		int height = (names.Length * 40) + ((names.Length + 1) * 2);
		((RectTransform)transform).sizeDelta = new Vector2(0, height);
		transform.localPosition = new Vector3(0, -(height / 2), 0);

		for (int i = 0; i < names.Length; i++)
		{
			changeKeybindings[i] = Instantiate<ChangeKeybindings>(temp);
			changeKeybindings[i].Initialize((Key)i, names[i], transform);
		}
	}
}