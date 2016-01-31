using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Base class for all Popups
/// </summary>
public abstract class PopupComponent : PPBehaviour
{
	/// <summary>
	/// Manages the canvas group and initializes the popup
	/// </summary>
	/// <typeparam name="T">the type of the callback</typeparam>
	/// <param name="callback">the callback to call in the popup</param>
	public void OnOpen<T>(ICallback<T> callback)
	{
		OnPopupOpened(callback);
	}

	/// <summary>
	/// Resets the popup
	/// </summary>
	public void OnClose()
	{
		OnPopupClosed();
	}

	/// <summary>
	/// Used for childclasses to initialize
	/// </summary>
	/// <typeparam name="T">the type of the callback</typeparam>
	/// <param name="callback">the callback to call in the popup</param>
	protected abstract void OnPopupOpened<T>(ICallback<T> callback);

	protected abstract void OnPopupClosed();
}