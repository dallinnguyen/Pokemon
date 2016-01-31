using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Button, that closes the current submenu and opens the parent
/// </summary>
public class Back : Button
{
	[SerializeField]
	private SubmenuComponent submenu;

	/// <summary>
	/// Called on click, closes the current submenu
	/// </summary>
	protected override void OnActivated()
	{
		submenu.CloseSubmenu();
	}
}
