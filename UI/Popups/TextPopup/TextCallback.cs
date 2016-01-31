/// <summary>
/// The callback method for the text popup
/// </summary>
public class TextCallback : ICallback<bool>
{
	/// <summary>
	/// Initializes a new instance of the <see cref="TextCallback" /> class.
	/// </summary>
	/// <param name="text">The text for the popup</param>
	public TextCallback(string text)
	{
		this.Text = text;
	}

	/// <summary>
	/// Gets: The text for the popup
	/// </summary>
	public string Text { get; private set; }

	/// <summary>
	/// Makes the buttons interactable again
	/// </summary>
	/// <param name="useless">Not used</param>
	public void Method(bool useless)
	{
	}
}