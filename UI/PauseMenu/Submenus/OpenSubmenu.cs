using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// A button that opens a certain submenu child
/// </summary>
public class OpenSubmenu : Button
{
	[SerializeField]
	private SubmenuComponent submenu;
	[SerializeField]
	public Submenu submenuToOpen;

	protected override void OnActivated()
	{
		submenu.OpenSubmenu(submenuToOpen);
	}
}
