using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// The base class for all submenus
/// </summary>
public abstract class SubmenuComponent : PPBehaviour
{
	[SerializeField]
	private Submenu submenu;
	[SerializeField]
	private Animator containerAnimator;
	[SerializeField]
	private Animator menuAnimator;
	[SerializeField]
	private CanvasGroup canvasGroup;
	[SerializeField]
	private UnityEngine.UI.Button firstButton; // The button that will be highlighted

	private PauseMenu pauseMenu;

	public void Initialize(PauseMenu pauseMenu)
	{
		this.pauseMenu = pauseMenu;
	}

	public void FlyIn()
	{
		menuAnimator.SetBool("SlideIn", true);
		OnFlyIn();
	}

	public void FlyOut()
	{
		menuAnimator.SetBool("SlideIn", false);
		OnFlyOut();
	}

	public void FoldLeft()
	{
		containerAnimator.SetBool("RotateContainer", true);
		menuAnimator.SetBool("TranslateLeft", true);
		canvasGroup.interactable = false;
		OnFoldLeft();
	}

	public void Unfold()
	{
		containerAnimator.SetBool("RotateContainer", false);
		menuAnimator.SetBool("TranslateLeft", false);
		canvasGroup.interactable = true;
		OnUnfold();
	}

	public void SelectFirstButton()
	{
		UI.Instance.EventSystem.SetSelectedGameObject(firstButton.gameObject);
	}

	public void OpenSubmenu(Submenu submenuToOpen)
	{
		pauseMenu.OpenSubmenu(submenuToOpen);
	}

	public void CloseSubmenu()
	{
		pauseMenu.CloseSubmenu();
	}

	protected abstract void OnFlyIn();

	protected abstract void OnFlyOut();

	protected abstract void OnFoldLeft();

	protected abstract void OnUnfold();
}