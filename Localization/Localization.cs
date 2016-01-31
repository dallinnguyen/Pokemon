using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// Holds all un-localized keys and the localized strings assigned to them for the
/// current language.
/// </summary>
public class Localization : Singleton<Localization>, IInitializeable
{
	private const string Path = "Localization/";

	private Language language;
	private Dictionary<string, string> file = new Dictionary<string, string>();

	/// <summary>
	/// Gets or sets the currently selected language.
	/// </summary>
	public Language Language
	{
		get
		{
			return language;
		}

		set
		{
			language = value;
			PlayerPrefs.SetInt("language", (int)language);
			ReadFile();
		}
	}

	/// <summary>
	/// Sets the current language to the one stored in the PlayerPrefs, or to English
	/// if one is not set. Should be called before using the Localization class.
	/// </summary>
	public void Initialize()
	{
		if (PlayerPrefs.GetInt("language", -1) == -1)
		{
			Language = Language.English;
		}
		else
		{
			Language = (Language)PlayerPrefs.GetInt("language");
		}
	}

	/// <summary>
	/// Returns the localized string corresponding to the given un-localized identifier.
	/// </summary>
	/// <param name="identifier">The un-localized identifier.</param>
	/// <returns>The localized string.</returns>
	public string Localize(string identifier)
	{
		return file[identifier];
	}

	private void ReadFile()
	{
		file.Clear();
		TextAsset rawFile = Resources.Load<TextAsset>(Path + language);
		string[] lines = rawFile.text.Split('\n');
		foreach (string line in lines)
		{
			if (!line.Contains("#") && line.Contains("="))
			{
				string[] splits = line.Split('=');
				file.Add(splits[0], splits[1].Replace("{n}", "\n"));
			}
		}
	}
}
