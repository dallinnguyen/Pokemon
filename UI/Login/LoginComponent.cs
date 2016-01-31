using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles the login UI and controls the login sequence.
/// </summary>
public class LoginComponent : UIWindowComponent
{
	[SerializeField]
	private InputField username;
	[SerializeField]
	private InputField password;
	[SerializeField]
	private CanvasGroup canvasGroup;

	private Client client;
	private bool awaitingRedirect;
	
	public override void OnWindowClosed()
	{
	}

	public void SubmitForm()
	{
		TextCallback callback = new TextCallback(Localization.Instance.Localize("login.text.loggingin"));
		UIController.Instance.OpenPopup<bool>(Popup.StatusPopup, callback);
		client.Login(username.text, password.text);
		UIController.Instance.CloseUIWindow(UIWindow.Login);
	}

	private void Awake()
	{
		client = Client.Instance;
		client.OnConnect += Client_OnConnect;
		client.OnDisconnect += Client_OnDisconnect;
		client.OnLoginFailure += Client_OnLoginFailure;
		client.OnAccount += Client_OnAccount;
		client.OnReceiveRedirect += Client_OnReceiveRedirect;
		client.OnDisconnecting += Client_OnDisconnecting;
	}

	private void Client_OnReceiveRedirect()
	{
		awaitingRedirect = true;
	}

	private void Client_OnLoginFailure(string message)
	{
		UIController.Instance.ClosePopup(Popup.StatusPopup);
		TextCallback callback = new TextCallback(message);
		UIController.Instance.OpenPopup<bool>(Popup.TextPopup, callback);
		UIController.Instance.OpenUIWindow(UIWindow.Login);

		Debug.Log("Login failed: " + message);
	}

	private void Client_OnDisconnecting()
	{
	}

	private void Client_OnDisconnect(string message)
	{
		/*UIController.Instance.ClosePopup(Popup.StatusPopup);
		UIController.Instance.OpenUIWindow(UIWindow.Login);
		TextCallback callback = new TextCallback(message);
		UIController.Instance.OpenPopup<bool>(Popup.TextPopup, callback);*/

		Debug.Log("OnDisconnect: " + message);

		if (awaitingRedirect && client.RedirectTarget != null && client.Token != null && client.Username != null && client.CharacterId > 0)
		{
			client.Connect(client.RedirectTarget, client.Token, client.Username, client.CharacterId);
		}
		else if (awaitingRedirect)
		{
			Debug.Log("Error on connecting to node, parameters not set?");
		}
	}

	private void Client_OnConnect()
	{
		Debug.Log("Connected!");
		if (awaitingRedirect) awaitingRedirect = false;
	}

	private void Client_OnAccount(Account account)
	{
		UIController.Instance.ClosePopup(Popup.StatusPopup);

		// TODO: UIController.Instance.OpenUIWindow(UIWindow.CharacterSelection);
		Debug.Log("Received account with " + account.Characters.Length + " characters");

		// TODO: remove
		client.RequestRedirect(1, 1);
	}
}