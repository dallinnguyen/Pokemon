/// <summary>
/// A recipe used to craft an item from other items. Object pooled.
/// </summary>
public class Recipe : Item
{
	/// <summary>
	/// Gets the required ingredients.
	/// </summary>
	public Ingredient[] Ingredients
	{
		get;
		private set;
	}

	/// <summary>
	/// Gets the item to be crafted.
	/// </summary>
	public Item Creation
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
	/// <param name="ingredients">The chance of the Poke Ball capturing a Pokemon.</param>
	/// <param name="creation">The chance of the Poke Ball breaking when hitting a surface.</param>
	/// <returns>The new object.</returns>
	public Recipe Recreate(int id, string name, string description, ushort image, float weight, int value, bool tradeable, Ingredient[] ingredients, Item creation)
	{
		this.ID = id;
		this.Name = name;
		this.Description = description;
		this.Image = image;
		this.Weight = weight;
		this.Value = value;
		this.Tradeable = tradeable;
		this.Ingredients = ingredients;
		this.Creation = creation;
		return this;
	}
}
