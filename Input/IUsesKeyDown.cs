/// <summary>
/// Used for subscribing to keyDown events
/// </summary>
public interface IUsesKeyDown
{
	/// <summary>
	/// Fired when a key is pressed once
	/// </summary>
	/// <param name="key">the key that was pressed</param>
	/// <param name="strength">if the key is an AxisKey, the value is the analog strength</param>
	/// <param name="layer">the actual InputLayer</param>
	void OnKeyDown(Key key, float strength, InputLayer layer);
}