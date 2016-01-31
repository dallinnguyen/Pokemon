using System.Collections;
using UnityEngine;

/// <summary>
/// Entry point of the application, meant for initialization.
/// </summary>
public class EntryPoint : PPBehaviour, IUsesKeyDown
{
	private static EntryPoint instance;

	private bool showMovie;

	/// <summary>
	/// Gets or sets splash textures to be shown when the game is started/
	/// </summary>
	public Texture2D[] SplashScreens
	{
		get;
		set;
	}

	/// <summary>
	/// Gets or sets the introduction movie to be shown when the game is started/
	/// </summary>
	public MovieTexture IntroVideo
	{
		get;
		set;
	}

	/// <summary>
	/// Let other classes start coroutines
	/// </summary>
	/// <param name="function">the coroutine to start</param>
	public static void BeginCoroutine(IEnumerator function)
	{
		instance.StartCoroutine(function);
	}

	/// <summary>
	/// Opens and closes the pausemenu
	/// </summary>
	/// <param name="key">the key that was pressed</param>
	/// <param name="strength">if the key is an AxisKey, the value is the analog strength</param>
	/// <param name="layer">the actual InputLayer</param>
	public void OnKeyDown(Key key, float strength, InputLayer layer)
	{
		if (!UIController.Instance.IsUIWindowOpen(UIWindow.PauseMenu) && layer == InputLayer.Default && key == Key.Menu)
		{
			UIController.Instance.OpenUIWindow(UIWindow.PauseMenu);
		}
		else if (UIController.Instance.IsUIWindowOpen(UIWindow.PauseMenu) && layer == InputLayer.UI && key == Key.Menu)
		{
			UIController.Instance.CloseUIWindow(UIWindow.PauseMenu);
		}
	}

	/// <summary>
	/// Sets the instance
	/// </summary>
	private void Awake()
	{
		instance = this;

		Config.Instance.OnConfigFileNotFound += (string path) =>
		{
			Config.Instance.Save();
		};
		Config.Instance.Load();

		Input.Instance.SubscribeToKeyDown(new Key[] { Key.Menu }, this);

		UIController.Instance.OpenUIWindow(UIWindow.Login);
		HUDController.Instance.HUD = HUD.PlayerHUD; // Move later to spawn of player

		/*UIController.Instance.LoadingScreen = true;

		for (int i = 0; i < SplashScreens.Length; i++)
		{
			yield return UIController.Instance.PlaySplash(SplashScreens[i]);
		}
		yield return UIController.Instance.PlayMovie(IntroVideo);
		
		UIController.Instance.Open(UIWindow.Tools);*/
	}
}
