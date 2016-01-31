using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class for managing input, features polling and events.
/// </summary>
public class Input : PPBehaviourSingleton<Input>, IInitializeable
{
	private BidirectionalDictionary<Key, int[]>[] keyMapping = new BidirectionalDictionary<Key, int[]>[2];
	private Dictionary<Key, List<IUsesKey>> keySubscribers = new Dictionary<Key, List<IUsesKey>>();
	private Dictionary<Key, List<IUsesKeyDown>> keyDownSubscribers = new Dictionary<Key, List<IUsesKeyDown>>();
	private Dictionary<Key, List<IUsesKeyUp>> keyUpSubscribers = new Dictionary<Key, List<IUsesKeyUp>>();
	private Dictionary<int, List<IUsesKeyCode>> keyCodeSubscribers = new Dictionary<int, List<IUsesKeyCode>>();
	private List<int> mappedKeys = new List<int>();
	private WaitForEndOfFrame wait = new WaitForEndOfFrame();

	private float deadZone = 0.2f;
	private int offset = AxisKeyToAxisMapping.GetOffset();

	private bool[] released;
	private bool[,] keys = new bool[AxisKeyToAxisMapping.GetMaxKey() + 1, 3]; // 0: OnKeyDown  1: OnKey 2: OnKeyUp

	/// <summary>
	/// Gets: currently released layer
	/// </summary>
	public InputLayer ReleasedLayer { get; private set; }

	/// <summary>
	/// Reads the custom keybindings or creates default ones
	/// </summary>
	public void Initialize()
	{
		for (int i = 0; i < 2; i++)
		{
			keyMapping[i] = new BidirectionalDictionary<Key, int[]>(EqualityComparer<Key>.Default, new IntArrayComparer());
		}
		released = new bool[Enum.GetValues(typeof(AxisKey)).Length];
		ReleasedLayer = InputLayer.Default;

		for (int i = 0; i < released.Length; i++)
		{
			released[i] = true;
		}

		for (int i = 0; i < 2; i++)
		{
			foreach (Key key in (Key[])Enum.GetValues(typeof(Key)))
			{
				if (PlayerPrefs.HasKey("key." + i + "." + ((int)key)))
				{
					string array = PlayerPrefs.GetString("key." + i + "." + ((int)key));
					string[] splits = array.Split(' ');
					int[] keys = new int[splits.Length];
					for (int j = 0; j < keys.Length; j++)
					{
						keys[j] = int.Parse(splits[j]);
					}
					keyMapping[i].Add(key, keys);
				}
				else
				{
					int[] keys = null;
					if (i == 0)
					{
						keys = new int[1];
						switch (key) // Keyboard
						{
							case Key.Forward: keys[0] = (int)KeyCode.W; break;
							case Key.Backward: keys[0] = (int)KeyCode.S; break;
							case Key.Left: keys[0] = (int)KeyCode.A; break;
							case Key.Right: keys[0] = (int)KeyCode.D; break;
							case Key.LookLeft: keys[0] = (int)AxisKey.MouseXm; break;
							case Key.LookRight: keys[0] = (int)AxisKey.MouseXp; break;
							case Key.LookUp: keys[0] = (int)AxisKey.MouseYp; break;
							case Key.LookDown: keys[0] = (int)AxisKey.MouseYm; break;
							case Key.Accept: keys[0] = (int)KeyCode.Return; break;
							case Key.Slot1: keys[0] = (int)KeyCode.Keypad1; break;
							case Key.Slot2: keys[0] = (int)KeyCode.Keypad2; break;
							case Key.Slot3: keys[0] = (int)KeyCode.Keypad3; break;
							case Key.Slot4: keys[0] = (int)KeyCode.Keypad4; break;
							case Key.Slot5: keys[0] = (int)KeyCode.Keypad5; break;
							case Key.Slot6: keys[0] = (int)KeyCode.Keypad6; break;
							case Key.Slot7: keys[0] = (int)KeyCode.Keypad7; break;
							case Key.Slot8: keys[0] = (int)KeyCode.Keypad8; break;
							case Key.Hit: keys[0] = (int)KeyCode.Mouse0; break;
							case Key.Guard: keys[0] = (int)KeyCode.Mouse1; break;
							case Key.Menu: keys[0] = (int)KeyCode.Escape; break;
							case Key.Map: keys[0] = (int)KeyCode.M; break;
							case Key.SendPokemon: keys[0] = (int)KeyCode.L; break;
							case Key.ReturnPokemon: keys[0] = (int)KeyCode.Backspace; break;
						}
					}
					else
					{
						switch (key) // Controller
						{
							case Key.Forward: keys = new int[1]; keys[0] = (int)AxisKey.LSYm; break;
							case Key.Backward: keys = new int[1]; keys[0] = (int)AxisKey.LSYp; break;
							case Key.Left: keys = new int[1]; keys[0] = (int)AxisKey.LSXm; break;
							case Key.Right: keys = new int[1]; keys[0] = (int)AxisKey.LSXp; break;
							case Key.LookLeft: keys = new int[1]; keys[0] = (int)AxisKey.RSXm; break;
							case Key.LookRight: keys = new int[1]; keys[0] = (int)AxisKey.RSXp; break;
							case Key.LookUp: keys = new int[1]; keys[0] = (int)AxisKey.RSYp; break;
							case Key.LookDown: keys = new int[1]; keys[0] = (int)AxisKey.RSYm; break;
							case Key.Accept: keys = new int[1]; keys[0] = (int)KeyCode.JoystickButton0; break;
							case Key.Slot1: keys = new int[1]; keys[0] = (int)KeyCode.JoystickButton5; break;
							case Key.Slot2: keys = new int[1]; keys[0] = (int)KeyCode.JoystickButton4; break;
							case Key.Slot3: keys = new int[2]; keys[0] = (int)AxisKey.LT; keys[1] = (int)KeyCode.JoystickButton5; break;
							case Key.Slot4: keys = new int[2]; keys[0] = (int)AxisKey.LT; keys[1] = (int)KeyCode.JoystickButton4; break;
							case Key.Slot5: keys = new int[2]; keys[0] = (int)AxisKey.LT; keys[1] = (int)KeyCode.JoystickButton0; break;
							case Key.Slot6: keys = new int[2]; keys[0] = (int)AxisKey.LT; keys[1] = (int)KeyCode.JoystickButton2; break;
							case Key.Slot7: keys = new int[2]; keys[0] = (int)AxisKey.LT; keys[1] = (int)KeyCode.JoystickButton1; break;
							case Key.Slot8: keys = new int[2]; keys[0] = (int)AxisKey.LT; keys[1] = (int)KeyCode.JoystickButton3; break;
							case Key.Hit: keys = new int[1]; keys[0] = (int)KeyCode.JoystickButton2; break;
							case Key.Guard: keys = new int[1]; keys[0] = (int)AxisKey.RT; break;
							case Key.Menu: keys = new int[1]; keys[0] = (int)KeyCode.JoystickButton7; break;
							case Key.Map: keys = new int[1]; keys[0] = (int)KeyCode.JoystickButton6; break;
							case Key.SendPokemon: keys = new int[1]; keys[0] = (int)AxisKey.HatYp; break;
							case Key.ReturnPokemon: keys = new int[1]; keys[0] = (int)AxisKey.HatYm; break;
						}
					}
					keyMapping[i].Add(key, keys);
					string s = string.Empty;
					for (int a = 0; a < keys.Length; a++)
					{
						s += keys[a];
						s += " ";
					}
					s = s.Substring(0, s.Length - 1);
					PlayerPrefs.SetString("key." + i + "." + ((int)key), s);
				}
			}
		}
		UpdateMappedKeys();
	}

	/// <summary>
	/// Adds a subscriber to the specified keys
	/// </summary>
	/// <param name="keys">The keys to subscribe to</param>
	/// <param name="subscriber">Whichever object wants to unsubscribe</param>
	public void SubscribeToKey(Key[] keys, IUsesKey subscriber)
	{
		foreach (Key key in keys)
		{
			if (!keySubscribers.ContainsKey(key))
			{
				keySubscribers.Add(key, new List<IUsesKey>());
			}
			keySubscribers[key].Add(subscriber);
		}
	}

	/// <summary>
	/// Adds a subscriber to the specified keys
	/// </summary>
	/// <param name="keys">The keys to subscribe to</param>
	/// <param name="subscriber">Whichever object wants to unsubscribe</param>
	public void SubscribeToKeyUp(Key[] keys, IUsesKeyUp subscriber)
	{
		foreach (Key key in keys)
		{
			if (!keyUpSubscribers.ContainsKey(key))
			{
				keyUpSubscribers.Add(key, new List<IUsesKeyUp>());
			}
			keyUpSubscribers[key].Add(subscriber);
		}
	}

	/// <summary>
	/// Adds a subscriber to the specified keys
	/// </summary>
	/// <param name="keys">The keys to subscribe to</param>
	/// <param name="subscriber">Whichever object wants to unsubscribe</param>
	public void SubscribeToKeyDown(Key[] keys, IUsesKeyDown subscriber)
	{
		foreach (Key key in keys)
		{
			if (!keyDownSubscribers.ContainsKey(key))
			{
				keyDownSubscribers.Add(key, new List<IUsesKeyDown>());
			}
			keyDownSubscribers[key].Add(subscriber);
		}
	}

	/// <summary>
	/// Adds a subscriber to the specified axisKey
	/// </summary>
	/// <param name="keys">The keys to subscribe to</param>
	/// <param name="subscriber">Whichever object wants to unsubscribe</param>
	public void SubscribeToKeyCode(int[] keys, IUsesKeyCode subscriber)
	{
		foreach (int key in keys)
		{
			if (!keyCodeSubscribers.ContainsKey(key))
			{
				keyCodeSubscribers.Add(key, new List<IUsesKeyCode>());
			}
			keyCodeSubscribers[key].Add(subscriber);
		}
		UpdateMappedKeys();
	}

	/// <summary>
	/// Removes a subscriber from the specified keys.
	/// </summary>
	/// <param name="keys">The keys to unsubscribe from</param>
	/// <param name="subscriber">Whichever object wants to subscribe</param>
	public void UnsubscribeFromKey(Key[] keys, IUsesKey subscriber)
	{
		foreach (Key key in keys)
		{
			if (keySubscribers.ContainsKey(key) && keySubscribers[key].Contains(subscriber))
			{
				keySubscribers[key].Remove(subscriber);
			}
			if (keySubscribers[key].Count == 0)
			{
				keySubscribers.Remove(key);
			}
		}
	}

	/// <summary>
	/// Removes a subscriber from the specified keys.
	/// </summary>
	/// <param name="keys">The keys to unsubscribe from</param>
	/// <param name="subscriber">Whichever object wants to subscribe</param>
	public void UnsubscribeFromKeyDown(Key[] keys, IUsesKeyDown subscriber)
	{
		foreach (Key key in keys)
		{
			if (keyDownSubscribers.ContainsKey(key) && keyDownSubscribers[key].Contains(subscriber))
			{
				keyDownSubscribers[key].Remove(subscriber);
			}
			if (keyDownSubscribers[key].Count == 0)
			{
				keyDownSubscribers.Remove(key);
			}
		}
	}

	/// <summary>
	/// Removes a subscriber from the specified keys.
	/// </summary>
	/// <param name="keys">The keys to unsubscribe from</param>
	/// <param name="subscriber">Whichever object wants to subscribe</param>
	public void UnsubscribeFromKeyUp(Key[] keys, IUsesKeyUp subscriber)
	{
		foreach (Key key in keys)
		{
			if (keyUpSubscribers.ContainsKey(key) && keyUpSubscribers[key].Contains(subscriber))
			{
				keyUpSubscribers[key].Remove(subscriber);
			}
			if (keyUpSubscribers[key].Count == 0)
			{
				keyUpSubscribers.Remove(key);
			}
		}
	}

	/// <summary>
	/// Removes a subscriber from the specified keys.
	/// </summary>
	/// <param name="keys">The keys to unsubscribe from</param>
	/// <param name="subscriber">Whichever object wants to subscribe</param>
	public void UnsubscribeFromKeyCode(int[] keys, IUsesKeyCode subscriber)
	{
		foreach (int key in keys)
		{
			if (keyCodeSubscribers.ContainsKey(key) && keyCodeSubscribers[key].Contains(subscriber))
			{
				keyCodeSubscribers[key].Remove(subscriber);
			}
			if (keyCodeSubscribers[key].Count == 0)
			{
				keyCodeSubscribers.Remove(key);
			}
		}
		UpdateMappedKeys();
	}

	/// <summary>
	/// Disables the Input for all layers except the given layer
	/// </summary>
	/// <param name="layer">layer to enable</param>
	public void LockInputLayersExcept(InputLayer layer)
	{
		ReleasedLayer = layer;
	}

	/// <summary>
	/// checks if the keymappings dictionary already contains the given key combination
	/// </summary>
	/// <param name="keyCodes">the key combination to check</param>
	/// <param name="keybinding">0 for normal binding, 1 for alternate</param>
	/// <returns>true, if there is already one</returns>
	public bool ContainsKeyMapping(int[] keyCodes, int keybinding)
	{
		if (keyMapping[keybinding].ContainsValue(keyCodes))
		{
			return true;
		}
		return false;
	}

	/// <summary>
	/// Sets the KeyMapping for the given Key
	/// </summary>
	/// <param name="key">key to set</param>
	/// <param name="keyCodes">array of keycodes to bind</param>
	/// <param name="keybinding">The normal (=0) or the alternate (=1)</param>
	public void SetKeyMapping(Key key, int[] keyCodes, int keybinding)
	{
		string value = string.Empty;

		foreach (int keyCode in keyCodes)
		{
			value += keyCode + " ";
		}
		value = value.Substring(0, value.Length - 1); // Cut last space

		keyMapping[keybinding][key] = keyCodes;
		PlayerPrefs.SetString("key." + keybinding + "." + ((int)key), value);
	}

	/// <summary>
	/// Returns the binded key combination
	/// </summary>
	/// <param name="key">The Key where you want the binding</param>
	/// <param name="binding">The normal (=0) or the alternate (=1)</param>
	/// <returns>Array out of keycodes</returns>
	public int[] GetKeyMapping(Key key, int binding)
	{
		return keyMapping[binding].GetValue(key);
	}

	/// <summary>
	/// Return all currently pressed buttons
	/// </summary>
	/// <returns>Array of Keycodes</returns>
	public int[] GetCurrentlyPressedButtons()
	{
		List<int> activatedKeys = new List<int>();

		for (int i = 0; i < AxisKeyToAxisMapping.GetMaxKey() + 1; i++)
		{
			if (this.keys[i, 1])
			{
				activatedKeys.Add(i);
			}
		}

		int[] keys = new int[activatedKeys.Count];
		activatedKeys.CopyTo(keys, 0);
		return keys;
	}

	private void Update()
	{
		keys = new bool[AxisKeyToAxisMapping.GetMaxKey() + 1, 3];
		float[] strengths = new float[AxisKeyToAxisMapping.GetMaxKey() - offset + 1];

		// Get raw data
		foreach (int keycode in mappedKeys)
		{
			if (keycode < offset) // Unity Keycode
			{
				if (UnityEngine.Input.GetKeyDown((KeyCode)keycode))
				{
					keys[keycode, 0] = true;
				}
				if (UnityEngine.Input.GetKey((KeyCode)keycode))
				{
					keys[keycode, 1] = true;
				}
				if (UnityEngine.Input.GetKeyUp((KeyCode)keycode))
				{
					keys[keycode, 2] = true;
				}
			}
			else // AxisKey
			{
				KeyValuePair<string, bool> axisName = AxisKeyToAxisMapping.GetAxisFor(keycode);
				float f = UnityEngine.Input.GetAxis(axisName.Key);
				if ((f > deadZone && axisName.Value) || (f < -deadZone && !axisName.Value))
				{
					strengths[keycode - offset] = f;
					keys[keycode, 1] = true;
					if (released[keycode - offset])
					{
						keys[keycode, 0] = true;
						released[keycode - offset] = false;
						StartCoroutine(SetReleased(keycode));
					}
				}
				if (!((f > deadZone && axisName.Value) || (f < -deadZone && !axisName.Value)))
				{
					keys[keycode, 2] = true;
				}
			}
		}

		// Use data to check for keymappings
		for (int i = 0; i < 2; i++)
		{
			foreach (KeyValuePair<Key, int[]> entry in keyMapping[i])
			{
				bool[] all = new bool[] { false, true, true };
				float strength = 1;

				foreach (int key in entry.Value)
				{
					if (keys[key, 0])
					{
						all[0] = true;
					}
					if (!keys[key, 1])
					{
						all[1] = false;
					}
					if (!keys[key, 2])
					{
						all[2] = false;
					}
					if (key >= offset)
					{
						strength = Math.Abs(strengths[key - offset]);
					}
				}

				if (all[0] && all[1] && keyDownSubscribers.ContainsKey(entry.Key)) // min one newly pressed
				{
					foreach (IUsesKeyDown subscriber in keyDownSubscribers[entry.Key])
					{
						subscriber.OnKeyDown(entry.Key, strength, ReleasedLayer);
					}
				}
				if (all[1] && keySubscribers.ContainsKey(entry.Key))
				{
					foreach (IUsesKey subscriber in keySubscribers[entry.Key])
					{
						subscriber.OnKey(entry.Key, strength, ReleasedLayer);
					}
				}
				if (all[2] && keyUpSubscribers.ContainsKey(entry.Key))
				{
					foreach (IUsesKeyUp subscriber in keyUpSubscribers[entry.Key])
					{
						subscriber.OnKeyUp(entry.Key, strength, ReleasedLayer);
					}
				}
			}
		}

		// Check for KeyCodes
		foreach (KeyValuePair<int, List<IUsesKeyCode>> entry in keyCodeSubscribers)
		{
			float strength = 1;

			if (entry.Key >= offset)
			{
				strength = Math.Abs(strengths[entry.Key - offset]);
			}

			if (keys[entry.Key, 0])
			{
				foreach (IUsesKeyCode subscriber in entry.Value)
				{
					subscriber.OnKeyCodeDown(entry.Key, strength, ReleasedLayer);
				}
			}
			if (keys[entry.Key, 1])
			{
				foreach (IUsesKeyCode subscriber in entry.Value)
				{
					subscriber.OnKeyCode(entry.Key, strength, ReleasedLayer);
				}
			}
		}
	}

	private void UpdateMappedKeys()
	{
		List<int> list = new List<int>();

		foreach (BidirectionalDictionary<Key, int[]> dictionary in keyMapping)
		{
			foreach (KeyValuePair<Key, int[]> entry in dictionary)
			{
				foreach (int keycode in entry.Value)
				{
					if (!list.Contains(keycode))
					{
						list.Add(keycode);
					}
				}
			}
		}

		foreach (int key in keyCodeSubscribers.Keys)
		{
			if (!list.Contains(key))
			{
				list.Add(key);
			}
		}

		mappedKeys = list;
	}

	private IEnumerator SetReleased(int i)
	{
		KeyValuePair<string, bool> res = AxisKeyToAxisMapping.GetAxisFor(i);
		float f;

		while (!released[i - offset])
		{
			yield return wait;
			f = UnityEngine.Input.GetAxis(res.Key);
			if (!((f > deadZone && res.Value) || (f < -deadZone && !res.Value)))
			{
				released[i - offset] = true;
			}
		}
	}
}