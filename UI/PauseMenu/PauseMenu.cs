using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// The menu where you can make settings, continue, etc
/// </summary>
public class PauseMenu : UIWindowComponent, IUsesKeyDown, IUsesKeyCode
{
	private Stack<SubmenuComponent> openSubmenus;

	private SubmenuComponent[] submenus;

	public override void OnWindowClosed()
	{
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
		Input.Instance.LockInputLayersExcept(InputLayer.Default);

		UI.Instance.EventSystem.SetSelectedGameObject(null);

		while (openSubmenus.Count > 0)
		{
			SubmenuComponent s = openSubmenus.Pop();
			Destroy(s.gameObject);
		}
	}

	/// <summary>
	/// Opens the given submenu and puts the old one on a stack
	/// </summary>
	/// <param name="submenu">the submenu</param>
	public void OpenSubmenu(Submenu submenu)
	{
		openSubmenus.Peek().FoldLeft();
		openSubmenus.Push(Instantiate<SubmenuComponent>(submenus[(int)submenu]));
		openSubmenus.Peek().Initialize(this);
		openSubmenus.Peek().transform.SetParent(transform, false);

		if (UI.Instance.EventSystem.currentSelectedGameObject != null)
		{
			openSubmenus.Peek().SelectFirstButton();
		}
		openSubmenus.Peek().FlyIn();
	}

	/// <summary>
	/// Closes the active submenu and gets the parent from the stack
	/// </summary>
	public void CloseSubmenu()
	{
		openSubmenus.Peek().FlyOut();
		openSubmenus.Pop();
		if (UI.Instance.EventSystem.currentSelectedGameObject != null)
		{
			openSubmenus.Peek().SelectFirstButton();
		}
		openSubmenus.Peek().Unfold();
	}

	/// <summary>
	/// Checks if the submenu is open
	/// </summary>
	/// <param name="submenu">the submenu to check</param>
	/// <returns>true, if open</returns>
	public bool IsSubmenuOpen(Submenu submenu)
	{
		return openSubmenus.Contains(submenus[(int)submenu]);
	}

	/// <summary>
	/// Selects the first button
	/// </summary>
	/// <param name="key">the key</param>
	/// <param name="strength">the strength</param>
	/// <param name="layer">the layer</param>
	public void OnKeyDown(Key key, float strength, InputLayer layer)
	{
		if (UI.Instance.EventSystem != null && UI.Instance.EventSystem.currentSelectedGameObject == null && (key == Key.Forward || key == Key.Backward || key == Key.Left || key == Key.Right) && layer == InputLayer.UI)
		{
			openSubmenus.Peek().SelectFirstButton();
		}
	}

	/// <summary>
	/// Deselects the currently selected button
	/// </summary>
	/// <param name="axis">the axis</param>
	/// <param name="strength">the strength</param>
	/// <param name="layer">the layer</param>
	public void OnKeyCode(int axis, float strength, InputLayer layer)
	{
		if (UI.Instance.EventSystem != null && UI.Instance.EventSystem.currentSelectedGameObject != null && (axis == (int)AxisKey.MouseXm || axis == (int)AxisKey.MouseXp || axis == (int)AxisKey.MouseYm || axis == (int)AxisKey.MouseYp) && layer == InputLayer.UI)
		{
			UI.Instance.EventSystem.SetSelectedGameObject(null);
		}
	}

	/// <summary>
	/// Not used
	/// </summary>
	/// <param name="axis">the keycode</param>
	/// <param name="strength">the strength for analog axis</param>
	/// <param name="layer">the actual input layer </param>
	public void OnKeyCodeDown(int axis, float strength, InputLayer layer)
	{
	}

	/// <summary>
	/// Locks input, makes cursor visible and opens first submenu
	/// </summary>
	private void Awake()
	{
		Input.Instance.LockInputLayersExcept(InputLayer.UI);
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;

		submenus = Resources.LoadAll<SubmenuComponent>("GUI/Prefabs/PauseMenu");
		openSubmenus = new Stack<SubmenuComponent>();

		Input.Instance.SubscribeToKeyCode(new int[] { (int)AxisKey.MouseXm, (int)AxisKey.MouseXp, (int)AxisKey.MouseYm, (int)AxisKey.MouseYp }, this);
		Input.Instance.SubscribeToKeyDown(new Key[] { Key.Forward, Key.Backward, Key.Left, Key.Right }, this);
	}

	private void Start()
	{
		openSubmenus.Push(Instantiate<SubmenuComponent>(submenus[(int)Submenu.MainMenu]));
		openSubmenus.Peek().Initialize(this);
		openSubmenus.Peek().transform.SetParent(transform, false);

		if (UI.Instance.EventSystem.currentSelectedGameObject != null)
		{
			openSubmenus.Peek().SelectFirstButton();
		}

		openSubmenus.Peek().FlyIn();
	}
}