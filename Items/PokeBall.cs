/// <summary>
/// A Poke Ball used to capture wild Pokemon. Object pooled.
/// </summary>
public class PokeBall : Item
{
	/// <summary>
	/// An attribute that causes certain actions when a Poke Ball is used.
	/// </summary>
	public enum Attribute
	{
	}

	/// <summary>
	/// Gets the chance of the Poke Ball capturing a Pokemon.
	/// </summary>
	public float CatchRate
	{
		get;
		private set;
	}

	/// <summary>
	/// Gets the chance of the Poke Ball breaking when hitting a surface.
	/// </summary>
	public float BreakRate
	{
		get;
		private set;
	}

	/// <summary>
	/// Gets the Poke Ball's attributes.
	/// </summary>
	public Attribute[] Attributes
	{
		get;
		private set;
	}

	/// <summary>
	/// Recreates this object with new values.
	/// </summary>
	/// <param name="id">The game ID of the item.</param>
	/// <param name="name">The name of the item.</param>
	/// <param name="description">The description of the item.</param>
	/// <param name="image">The asset ID of the thumbnail image of the item.</param>
	/// <param name="weight">The weight of the item.</param>
	/// <param name="value">The item's value in Poke Dollars.</param>
	/// <param name="tradeable">A value indicating whether the item is tradeable.</param>
	/// <param name="catchRate">The chance of the Poke Ball capturing a Pokemon.</param>
	/// <param name="breakRate">The chance of the Poke Ball breaking when hitting a surface.</param>
	/// <param name="attributes">The Poke Ball's attributes.</param>
	/// <returns>The new object.</returns>
	public PokeBall Recreate(int id, string name, string description, ushort image, float weight, int value, bool tradeable, float catchRate, float breakRate, Attribute[] attributes)
	{
		this.ID = id;
		this.Name = name;
		this.Description = description;
		this.Image = image;
		this.Weight = weight;
		this.Value = value;
		this.Tradeable = tradeable;
		this.CatchRate = catchRate;
		this.BreakRate = breakRate;
		this.Attributes = attributes;
		return this;
	}
}
