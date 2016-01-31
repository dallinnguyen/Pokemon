/// <summary>
/// An ingredient used to craft an item. Object pooled.
/// </summary>
public class Ingredient
{
	/// <summary>
	/// Gets the ingredient item.
	/// </summary>
	public Item Item
	{
		get;
		private set;
	}

	/// <summary>
	/// Gets the amount of items for the ingredient.
	/// </summary>
	public int Amount
	{
		get;
		private set;
	}

	/// <summary>
	/// Recreates this object with new values.
	/// </summary>
	/// <param name="item">The ingredient item.</param>
	/// <param name="amount">The amount of items for the ingredient.</param>
	/// <returns>The new object.</returns>
	public Ingredient Recreate(Item item, int amount)
	{
		this.Item = item;
		this.Amount = amount;
		return this;
	}
}
