using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// The class for controlling the Login UI
/// </summary>
public class LoginButton : Button
{
	[SerializeField]
	private LoginComponent component;

	protected override void OnActivated()
	{
		component.SubmitForm();
	}
}