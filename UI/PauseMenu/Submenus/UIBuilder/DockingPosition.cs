/// <summary>
/// All positions available
/// </summary>
public enum DockingPosition
{
	BottomLeftCorner,
	BottomLowerCenter,
	BottomLowerMiddleCenter,
	BottomUpperMiddleCenter,
	BottomUpperCenter,
	BottomRightCorner,
	RightLowerSide,
	RightUpperSide,
	TopLeftCorner,
	TopLowerCenter,
	TopLowerMiddleCenter,
	TopUpperMiddleCenter,
	TopUpperCenter,
	TopRightCorner,
	LeftUpperSide,
	LeftLowerSide
}

/// <summary>
/// A helper class for the docking positions
/// </summary>
public static class DockingPositionHelper
{
	/// <summary>
	/// Returns the anchors that the given position would have
	/// </summary>
	/// <returns>float[]{minX, maxX, minY, maxY}</returns>
	public static float[] GetAnchors(DockingPosition dockingPosition)
	{
		float[] f = new float[4];

		switch (dockingPosition)
		{
			case DockingPosition.BottomLeftCorner: f[0] = 0; f[1] = 0.25f; f[2] = 0; f[3] = 0.3f; break;
			case DockingPosition.BottomLowerCenter: f[0] = 0.25f; f[1] = 0.75f; f[2] = 0; f[3] = 0.075f; break;
			case DockingPosition.BottomLowerMiddleCenter: f[0] = 0.25f; f[1] = 0.75f; f[2] = 0.075f; f[3] = 0.15f; break;
			case DockingPosition.BottomUpperMiddleCenter: f[0] = 0.25f; f[1] = 0.75f; f[2] = 0.15f; f[3] = 0.225f; break;
			case DockingPosition.BottomUpperCenter: f[0] = 0.25f; f[1] = 0.75f; f[2] = 0.225f; f[3] = 0.3f; break;
			case DockingPosition.BottomRightCorner: f[0] = 0.75f; f[1] = 1; f[2] = 0; f[3] = 0.3f; break;
			case DockingPosition.RightLowerSide: f[0] = 0.75f; f[1] = 1; f[2] = 0.3f; f[3] = 0.5f; break;
			case DockingPosition.RightUpperSide: f[0] = 0.75f; f[1] = 1; f[2] = 0.5f; f[3] = 0.7f; break;
			case DockingPosition.TopLeftCorner: f[0] = 0; f[1] = 0.25f; f[2] = 0.7f; f[3] = 1; break;
			case DockingPosition.TopLowerCenter: f[0] = 0.25f; f[1] = 0.75f; f[2] = 0.7f; f[3] = 0.775f; break;
			case DockingPosition.TopLowerMiddleCenter: f[0] = 0.25f; f[1] = 0.75f; f[2] = 0.775f; f[3] = 0.85f; break;
			case DockingPosition.TopUpperMiddleCenter: f[0] = 0.25f; f[1] = 0.75f; f[2] = 0.85f; f[3] = 0.925f; break;
			case DockingPosition.TopUpperCenter: f[0] = 0.25f; f[1] = 0.75f; f[2] = 0.925f; f[3] = 1; break;
			case DockingPosition.TopRightCorner: f[0] = 0.75f; f[1] = 1; f[2] = 0.7f; f[3] = 1; break;
			case DockingPosition.LeftUpperSide: f[0] = 0; f[1] = 0.25f; f[2] = 0.5f; f[3] = 0.7f; break;
			case DockingPosition.LeftLowerSide: f[0] = 0; f[1] = 0.25f; f[2] = 0.3f; f[3] = 0.5f; break;
		}
		return f;
	}

	/// <summary>
	/// Returns the DockingPositon of the given anchors
	/// </summary>
	/// <param name="available">false if no docking position</param>
	public static DockingPosition GetDockingPosition(float[] anchors, out bool available)
	{
		available = true;

		if (anchors[0] == 0 && anchors[1] == 0.25f && anchors[2] == 0 && anchors[3] == 0.3f)
		{
			return DockingPosition.BottomLeftCorner;
		}
		if (anchors[0] == 0.25f && anchors[1] == 0.75f && anchors[2] == 0 && anchors[3] == 0.075f)
		{
			return DockingPosition.BottomLowerCenter;
		}
		if (anchors[0] == 0.25f && anchors[1] == 0.75f && anchors[2] == 0.075f && anchors[3] == 0.15f)
		{
			return DockingPosition.BottomLowerMiddleCenter;
		}
		if (anchors[0] == 0.25f && anchors[1] == 0.75f && anchors[2] == 0.15f && anchors[3] == 0.225f)
		{
			return DockingPosition.BottomUpperMiddleCenter;
		}
		if (anchors[0] == 0.25f && anchors[1] == 0.75f && anchors[2] == 0.225f && anchors[3] == 0.3f)
		{
			return DockingPosition.BottomUpperCenter;
		}
		if (anchors[0] == 0.75f && anchors[1] == 1 && anchors[2] == 0 && anchors[3] == 0.3f)
		{
			return DockingPosition.BottomRightCorner;
		} 
		if (anchors[0] == 0.75f && anchors[1] == 1 && anchors[2] == 0.3f && anchors[3] == 0.5f)
		{
			return DockingPosition.RightLowerSide;
		}
		if (anchors[0] == 0.75f && anchors[1] == 1 && anchors[2] == 0.5f && anchors[3] == 0.7f)
		{
			return DockingPosition.RightUpperSide;
		}
		if (anchors[0] == 0 && anchors[1] == 0.25f && anchors[2] == 0.7f && anchors[3] == 1)
		{
			return DockingPosition.TopLeftCorner;
		}
		if (anchors[0] == 0.25f && anchors[1] == 0.75f && anchors[2] == 0.7f && anchors[3] == 0.775f)
		{
			return DockingPosition.TopLowerCenter;
		}
		if (anchors[0] == 0.25f && anchors[1] == 0.75f && anchors[2] == 0.775f && anchors[3] == 0.85f)
		{
			return DockingPosition.TopLowerMiddleCenter;
		}
		if (anchors[0] == 0.25f && anchors[1] == 0.75f && anchors[2] == 0.85f && anchors[3] == 0.925f)
		{
			return DockingPosition.TopUpperMiddleCenter;
		}
		if (anchors[0] == 0.25f && anchors[1] == 0.75f && anchors[2] == 0.925f && anchors[3] == 1)
		{
			return DockingPosition.TopUpperCenter;
		}
		if (anchors[0] == 0.75f && anchors[1] == 1 && anchors[2] == 0.7f && anchors[3] == 1)
		{
			return DockingPosition.TopRightCorner;
		}
		if (anchors[0] == 0 && anchors[1] == 0.25f && anchors[2] == 0.5f && anchors[3] == 0.7f)
		{
			return DockingPosition.LeftUpperSide;
		}
		if (anchors[0] == 0 && anchors[1] == 0.25f && anchors[2] == 0.3f && anchors[3] == 0.5f)
		{
			return DockingPosition.LeftLowerSide;
		}

		available = false;
		return DockingPosition.BottomLeftCorner;
	}
}