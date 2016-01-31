using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// used to scroll with the mouse wheel
/// </summary>
public class Scroller : PPBehaviour, IUsesKeyCode
{
	[SerializeField]
	private float scrollFactor;
	[SerializeField]
	private Scrollbar scrollbar;
	[SerializeField]
	private InputLayer inputLayer;

	/// <summary>
	/// Used for scrolling through the keys
	/// </summary>
	public void OnKeyCode(int axis, float strength, InputLayer layer)
	{
		if (axis == (int)AxisKey.MouseWheelp && scrollbar.value < 1 && layer == inputLayer)
		{
			scrollbar.value += 0.1f * scrollFactor * strength;
		}
		else if (axis == (int)AxisKey.MouseWheelm && scrollbar.value > 0 && layer == inputLayer)
		{
			scrollbar.value -= 0.1f * scrollFactor * strength;
		}
	}

	public void OnKeyCodeDown(int key, float strength, InputLayer layer)
	{
	}

	private void Awake()
	{
		Input.Instance.SubscribeToKeyCode(new int[] { (int)AxisKey.MouseWheelm, (int)AxisKey.MouseWheelp }, this);
	}
}
