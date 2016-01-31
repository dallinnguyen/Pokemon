using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Allows pressing enter to login instead of manually clicking the login button.
/// </summary>
public class LoginSubmitEnter : PPBehaviour
{
	[SerializeField]
	private LoginComponent login;

	private void Update()
	{
		if (UnityEngine.Input.GetKeyDown(KeyCode.Return))
		{
			login.SubmitForm();
		}
	}
}