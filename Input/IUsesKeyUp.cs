/// <summary>
/// Used for subscribing to keyUp events
/// </summary>
public interface IUsesKeyUp
{
	/// <summary>
	/// Fired when a key is up
	/// </summary>
	/// <param name="key">the key that was pressed</param>
	/// <param name="strength">if the key is an AxisKey, the value is the analog strength</param>
	/// <param name="layer">the actual InputLayer</param>
	void OnKeyUp(Key key, float strength, InputLayer layer);
}