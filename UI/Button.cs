using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Base class for button scripts
/// </summary>
public abstract class Button : PPBehaviour
{
	[SerializeField]
	private Animator animator;

	/// <summary>
	/// The callback of the click, have to be set in the editor
	/// </summary>
	public void OnClick()
	{
		OnActivated();
	}

	/// <summary>
	/// Have to be called on highlight (buttonEnter and onSelect)
	/// </summary>
	public void OnSelect()
	{
		animator.SetBool("Selected", true);
	}

	/// <summary>
	/// Have to be called on buttonExit or onDeselect
	/// </summary>
	public void OnDeselect()
	{
		animator.SetBool("Selected", false);
	}

	/// <summary>
	/// Callback for activation of the Button (when clicked)
	/// </summary>
	protected abstract void OnActivated();
}