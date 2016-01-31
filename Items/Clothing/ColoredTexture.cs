using UnityEngine;

/// <summary>
/// A texture that can be colored. Object pooled.
/// </summary>
public class ColoredTexture
{
	/// <summary>
	/// Gets a value indicating whether the texture can be colored.
	/// </summary>
	public bool Colorable
	{
		get;
		private set;
	}

	/// <summary>
	/// Gets the color of the texture.
	/// </summary>
	public Color Color
	{
		get;
		private set;
	}

	/// <summary>
	/// Gets the asset ID of the texture.
	/// </summary>
	public ushort Texture
	{
		get;
		private set;
	}

	/// <summary>
	/// Recreates this object with new values.
	/// </summary>
	/// <param name="colorable">A value indicating whether the texture can be colored.</param>
	/// <param name="color">The color of the texture.</param>
	/// <param name="texture">The asset ID of the texture.</param>
	/// <returns>The new object.</returns>
	public ColoredTexture Recreate(bool colorable, Color color, ushort texture)
	{
		this.Colorable = colorable;
		this.Color = color;
		this.Texture = texture;
		return this;
	}
}
