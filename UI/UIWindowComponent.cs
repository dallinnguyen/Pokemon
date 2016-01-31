using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// base class for all UIWindows
/// </summary>
public abstract class UIWindowComponent : PPBehaviour
{
	/// <summary>
	/// Called when the window is closed
	/// </summary>
	public abstract void OnWindowClosed();
}