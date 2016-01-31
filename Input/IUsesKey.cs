/// <summary>
/// Used for subscribing to key events
/// </summary>
public interface IUsesKey
{
	/// <summary>
	/// Fired while a key is pressed
	/// </summary>
	/// <param name="key">the key</param>
	/// <param name="strength">the strengths</param>
	/// <param name="layer">the actual layer</param>
	void OnKey(Key key, float strength, InputLayer layer);
}