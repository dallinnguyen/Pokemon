/// <summary>
/// A unique item meant for a specialized purpose. Object pooled.
/// </summary>
public class Unique : Item
{
	/// <summary>
	/// Trigger that causes specialized actions in the game.
	/// </summary>
	public enum Trigger
	{
	}

	/// <summary>
	/// Gets the triggers of the item.
	/// </summary>
	public Trigger[] Triggers
	{
		get;
		private set;
	}

	/// <summary>
	/// Gets a value indicating whether the item can be used.
	/// </summary>
	public bool Usable
	{
		get;
		private set;
	}

	/// <summary>
	/// Gets a value indicating whether the item can be used indefinitely.
	/// </summary>
	public bool Reusable
	{
		get;
		private set;
	}

	/// <summary>
	/// Gets a value indicating whether the item can be discarded.
	/// </summary>
	public bool Discardable
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
	/// <param name="triggers">The triggers of the item.</param>
	/// <param name="usable">A value indicating whether the item can be used.</param>
	/// <param name="reusable">A value indicating whether the item can be used indefinitely.</param>
	/// <param name="discardable">A value indicating whether the item can be discarded.</param>
	/// <returns>The new object.</returns>
	public Unique Recreate(int id, string name, string description, ushort image, float weight, int value, bool tradeable, Trigger[] triggers, bool usable, bool reusable, bool discardable)
	{
		this.ID = id;
		this.Name = name;
		this.Description = description;
		this.Image = image;
		this.Weight = weight;
		this.Value = value;
		this.Tradeable = tradeable;
		this.Triggers = triggers;
		this.Usable = usable;
		this.Reusable = reusable;
		this.Discardable = discardable;
		return this;
	}
}
