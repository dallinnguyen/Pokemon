using System.Collections;
using UnityEngine;

/// <summary>
/// Controls map settings and map chunk loading/unloading.
/// </summary>
public class MapController : Singleton<MapController>, IInitializeable
{
	/// <summary>
	/// Gets the 5x5 array of currently loaded map chunks.
	/// </summary>
	public MapChunk[,] Chunks
	{
		get;
		private set;
	}

	/// <summary>
	/// Initializes the map object.
	/// </summary>
	public void Initialize()
	{
	}

	/// <summary>
	/// Shifts the loaded region of the game map in a direction, loading and unloading necessary map chunks.
	/// </summary>
	/// <param name="direction">The direction to shift the game map.</param>
	public void Shift(Direction direction)
	{
	}

	/// <summary>
	/// Centers the loaded region of the game map to a map chunk.
	/// </summary>
	/// <param name="x">The world x coordinate of the map chunk.</param>
	/// <param name="y">The world y coordinate of the map chunk.</param>
	/// <param name="layer">The world layer of the map chunk.</param>
	public void Center(int x, int y, int layer)
	{
	}

	private MapChunk LoadChunk(int x, int y)
	{
		return null;
	}
}
