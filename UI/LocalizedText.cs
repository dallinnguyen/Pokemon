using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Text component, that localizes himself
/// </summary>
public class LocalizedText : PPBehaviour
{
	[SerializeField]
	private Text text;
	private Language language = Language.English;
	[SerializeField]
	private string unlocalizedContent;

	/// <summary>
	/// Gets or sets: The key to localize
	/// </summary>
	public string UnlocalizedContent 
	{
		get
		{
			return unlocalizedContent;
		}
		set
		{
			unlocalizedContent = value;
			text.text = Localization.Instance.Localize(UnlocalizedContent);
		}
	}

	private void Start()
	{
		text.text = Localization.Instance.Localize(UnlocalizedContent);
	}

	private void Update()
	{
		if (language != Localization.Instance.Language)
		{
			text.text = Localization.Instance.Localize(UnlocalizedContent);
			language = Localization.Instance.Language;
		}
	}
}
