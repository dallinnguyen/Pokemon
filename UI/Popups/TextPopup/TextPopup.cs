using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Text popup that can be closed by the user (e.g. error messages)
/// </summary>
public class TextPopup : PopupComponent
{
	[SerializeField]
	private Text text;
	private TextCallback textCallback;

	/// <summary>
	/// Used to reset the popup
	/// </summary>
	protected override void OnPopupClosed()
	{
		textCallback.Method(true);
	}

	/// <summary>
	/// Callback when the popup is open
	/// </summary>
	/// <typeparam name="T">the type of the Callback</typeparam>
	/// <param name="callback">the callback to be called in the popup</param>
	protected override void OnPopupOpened<T>(ICallback<T> callback)
	{
		textCallback = (TextCallback)callback;
		text.text = textCallback.Text;
	}
}