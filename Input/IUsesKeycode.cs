/// <summary>
/// Used for subscribtion to certain keycodes (unity keycodes or axis keys)
/// </summary>
public interface IUsesKeyCode
{
	/// <summary>
	/// Fired when a keycode is pressed once
	/// </summary>
	/// <param name="key">the key that was pressed</param>
	/// <param name="strength">if the key is an AxisKey, the value is the analog strength</param>
	/// <param name="layer">the actual InputLayer</param>
	void OnKeyCodeDown(int key, float strength, InputLayer layer);

	/// <summary>
	/// Fired while a keycode is pressed
	/// </summary>
	/// <param name="key">the key that was pressed</param>
	/// <param name="strength">if the key is an AxisKey, the value is the analog strength</param>
	/// <param name="layer">the actual InputLayer</param>
	void OnKeyCode(int key, float strength, InputLayer layer);
}