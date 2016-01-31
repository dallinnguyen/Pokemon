using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Stores Text and Image of a Button
/// </summary>
public class ButtonSettings : PPBehaviour
{
	[SerializeField]
	private Image image;
	[SerializeField]
	private Text text;

	public Image Image
	{
		get
		{
			return image;
		}
	}

	public Text Text
	{
		get
		{
			return text;
		}
	}
}