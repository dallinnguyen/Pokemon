using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

/// <summary>
/// Manages loading and saving of a configuration file with defined settings.
/// Supports strings, primitive types, and enums.
/// </summary>
public class Config : Singleton<Config>, IInitializeable
{
	private const string StandardPath = "Config.ini";
	private ISettingData[] settings;

	/// <summary>
	/// Handles a setting successfully loading
	/// </summary>
	/// <param name="setting">The configuration setting.</param>
	/// <param name="value">The value of the configuration setting.</param>
	public delegate void SettingLoadedHandler(Setting setting, object value);

	/// <summary>
	/// Handles a setting failing to load.
	/// </summary>
	/// <param name="setting">The configuration setting.</param>
	/// <param name="value">The value of the configuration setting.</param>
	/// <param name="defaultValue">The default value of the configuration setting.</param>
	public delegate void SettingLoadingFailedHandler(Setting setting, string value, object defaultValue);

	/// <summary>
	/// Handles an unknown setting that tried to be parsed.
	/// </summary>
	/// <param name="key">The name of the configuration setting.</param>
	/// <param name="value">The value of the configuration setting.</param>
	public delegate void UnknownSettingHandler(string key, string value);

	/// <summary>
	/// Handles the configuration file not being able to be found.
	/// </summary>
	/// <param name="path">The path used to find the config file.</param>
	public delegate void ConfigFileNotFoundHandler(string path);

	/// <summary>
	/// Shot when a setting has been successfully loaded.
	/// </summary>
	public event SettingLoadedHandler OnSettingLoaded;

	/// <summary>
	/// Shot when a setting has failed to load.
	/// </summary>
	public event SettingLoadingFailedHandler OnSettingLoadingFailed;

	/// <summary>
	/// Shot when an unknown setting tried to be parsed.
	/// </summary>
	public event UnknownSettingHandler OnUnknownSetting;

	/// <summary>
	/// Shot when the configuration file could not be found using the current path.
	/// </summary>
	public event ConfigFileNotFoundHandler OnConfigFileNotFound;

	/// <summary>
	/// Container interface for generic SettingData objects.
	/// </summary>
	public interface ISettingData
	{
		/// <summary>
		/// Gets the text describing the configuration setting.
		/// </summary>
		string Description
		{
			get;
		}

		/// <summary>
		/// Gets the setting's content as an object.
		/// </summary>
		object UntypedContent
		{
			get;
		}

		/// <summary>
		/// Gets the data type of the setting's content.
		/// </summary>
		Type DataType
		{
			get;
		}
	}

	/// <summary>
	/// Initializes the Config singleton with the hardcoded settings and default values.
	/// </summary>
	public void Initialize()
	{
		this.settings = new Config.ISettingData[(int)Setting.Length];
		this.settings[(int)Setting.Fullscreen] = new Config.SettingData<bool>("Whether the game is fullscreen or not.", false);
		this.settings[(int)Setting.ResolutionWidth] = new Config.SettingData<int>("The game resolution width.", 800);
		this.settings[(int)Setting.ResolutionHeight] = new Config.SettingData<int>("The game resolution height.", 600);
	}

	/// <summary>
	/// Saves the current settings to disk.
	/// </summary>
	/// <param name="path">The path to save to.</param>
	public void Save(string path = StandardPath)
	{
		string[] newLines = new string[settings.Length * 3];
		int line = 0;
		for (int i = 0; i < settings.Length; i++)
		{
			ISettingData setting = settings[i];
			string dataType = setting.DataType.ToString();
			if (dataType.Length > 7 && dataType.Substring(7, dataType.Length - 7).Equals("System."))
			{
				dataType = dataType.Substring(7, dataType.Length - 7);
			}

			newLines[line++] = "# (" + dataType + ") " + setting.Description;
			newLines[line++] = ((Setting)i).ToString() + " = " + setting.UntypedContent.ToString();
			newLines[line++] = string.Empty;
		}
		File.WriteAllLines(path, newLines);
	}

	/// <summary>
	/// Loads the current settings from disk.
	/// </summary>
	/// <param name="path">The path to the configuration file.</param>
	public void Load(string path = StandardPath)
	{
		if (File.Exists(path))
		{
			string[] lines = File.ReadAllLines(path);
			for (int i = 0; i < lines.Length; i++)
			{
				if (lines[i].Length > 0 && lines[i][0] != '#' && lines[i].Contains("="))
				{
					string[] parts = lines[i].Split(new string[] { "=" }, 2, StringSplitOptions.RemoveEmptyEntries);
					if (parts.Length >= 2)
					{
						string key = parts[0].Trim();
						string value = parts[1].Trim();
						try
						{
							Setting setting = (Setting)Enum.Parse(typeof(Setting), key);
							ISettingData data = settings[(int)setting];
							MethodInfo method = this.GetType().GetMethod("ParseValue", BindingFlags.NonPublic | BindingFlags.Instance).MakeGenericMethod(data.DataType);
							method.Invoke(this, new object[] { settings, setting, value });
							if (OnSettingLoaded != null)
							{
								OnSettingLoaded(setting, data.UntypedContent.ToString());
							}
						}
						catch (ArgumentException)
						{
							if (OnUnknownSetting != null)
							{
								OnUnknownSetting(key, value);
							}
						}
					}
				}
			}
		}
		else
		{
			if (OnConfigFileNotFound != null)
			{
				OnConfigFileNotFound(path);
			}
		}
	}

	/// <summary>
	/// Gets the content of a setting.
	/// </summary>
	/// <typeparam name="T">The data type to return the content in.</typeparam>
	/// <param name="setting">The setting.</param>
	/// <returns>The setting's content.</returns>
	public T Get<T>(Setting setting)
	{
		MethodInfo method = this.GetType().GetMethod("GetValue", BindingFlags.NonPublic | BindingFlags.Instance).MakeGenericMethod(settings[(int)setting].DataType, typeof(T));
		return (T)method.Invoke(this, new object[] { this.settings, setting });
	}

	/// <summary>
	/// Sets the content of a setting.
	/// </summary>
	/// <typeparam name="T">The data type of the setting's content.</typeparam>
	/// <param name="setting">The setting.</param>
	/// <param name="newValue">The new content of the setting.</param>
	public void Set<T>(Setting setting, T newValue)
	{
		SetValue<T>(this.settings, setting, newValue);
	}

	/// <summary>
	/// Gets the description of a setting.
	/// </summary>
	/// <param name="setting">The setting.</param>
	/// <returns>The setting's description.</returns>
	public string GetDescription(Setting setting)
	{
		return settings[(int)setting].Description;
	}

	private SettingData<T> GetData<T>(ISettingData[] settings, Setting setting)
	{
		return (SettingData<T>)settings[(int)setting];
	}

	private void SetValue<T>(ISettingData[] settings, Setting setting, object newValue)
	{
		GetData<T>(settings, setting).Content = (T)newValue;
	}

	private TOutput GetValue<TSetting, TOutput>(ISettingData[] settings, Setting setting)
	{
		return (TOutput)Convert.ChangeType(GetData<TSetting>(settings, setting).Content, typeof(TOutput));
	}

	private void ParseValue<T>(ISettingData[] settings, Setting setting, string serializedValue)
	{
		try
		{
			object value = serializedValue;
			if (typeof(T).IsEnum)
			{
				value = Enum.Parse(typeof(T), serializedValue);
			}
			else if (typeof(T) != typeof(string))
			{
				value = Convert.ChangeType(serializedValue, typeof(T));
			}

			SetValue<T>(settings, setting, value);
		}
		catch (FormatException)
		{
			OnSettingLoadingFailed(setting, serializedValue, settings[(int)setting].UntypedContent.ToString());
		}
	}

	/// <summary>
	/// Configuration setting.
	/// </summary>
	/// <typeparam name="T">The data type of the setting's content.</typeparam>
	public class SettingData<T> : ISettingData
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="SettingData{T}" /> class.
		/// </summary>
		/// <param name="description">Description of the configuration setting.</param>
		/// <param name="content">The default value of the configuration setting.</param>
		public SettingData(string description, T content)
		{
			if (typeof(T) != typeof(string) && !typeof(T).IsPrimitive && !typeof(T).IsEnum)
			{
				throw new ArgumentException("Settings must be a string, primitive, or enum.");
			}

			this.Description = description;
			this.Content = content;
		}

		/// <summary>
		/// Gets the text describing the configuration setting.
		/// </summary>
		public string Description
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets or sets the value of the configuration setting.
		/// </summary>
		public T Content
		{
			get;
			set;
		}

		/// <summary>
		/// Gets the setting's content as an object.
		/// </summary>
		public object UntypedContent
		{
			get
			{
				return (object)Content;
			}
		}

		/// <summary>
		/// Gets the data type of the setting's content.
		/// </summary>
		public Type DataType
		{
			get
			{
				return typeof(T);
			}
		}
	}
}
