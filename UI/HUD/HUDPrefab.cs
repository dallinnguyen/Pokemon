using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// The class for all UI elemets like health bar, stamina bar, etc
/// </summary>
public class HUDPrefab : PPBehaviour
{
	public int WindowID { get; set; }

	[SerializeField]
	private int window; //  The type of the window (the enum value casted to an int)
	[SerializeField]
	private RectTransform rectTransform;
	[SerializeField]
	private CanvasGroup canvasGroup;

	public RectTransform RectTransform
	{
		get
		{
			return rectTransform;
		}
	}

	public float WindowAlpha
	{
		get
		{
			return canvasGroup.alpha;
		}
		set
		{
			canvasGroup.alpha = value;
		}
	}

	public KeyValuePair<int, float[]> WindowInformation
	{
		get
		{
			float[] f = new float[4];
			f[0] = rectTransform.anchorMin.x;
			f[1] = rectTransform.anchorMax.x;
			f[2] = rectTransform.anchorMin.y;
			f[3] = rectTransform.anchorMax.y;
			return new KeyValuePair<int, float[]>(window, f);
		}
	}

	public void Initialize(int windowID, float[] anchors, RectTransform parent, int window)
	{
		WindowID = windowID;
		rectTransform.anchorMin = new Vector2(anchors[0], anchors[2]);
		rectTransform.anchorMax = new Vector2(anchors[1], anchors[3]);
		this.window = window;
		transform.SetParent(parent, false);
	}

	public void ApplyHUDChanges(Transform parent, float alpha)
	{
		transform.SetParent(parent, false);
		canvasGroup.alpha = alpha;
	}
}