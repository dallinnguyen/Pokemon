/// <summary>
/// An item that teaches a move to a Pokemon. Object pooled.
/// </summary>
public class Machine : Item
{
	/// <summary>
	/// Gets the move to teach.
	/// </summary>
	public Move Move
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
	/// <param name="move">The move to teach.</param>
	/// <returns>The new object.</returns>
	public Machine Recreate(int id, string name, string description, ushort image, float weight, int value, bool tradeable, Move move)
	{
		this.ID = id;
		this.Name = name;
		this.Description = description;
		this.Image = image;
		this.Weight = weight;
		this.Value = value;
		this.Tradeable = tradeable;
		this.Move = move;
		return this;
	}
}
