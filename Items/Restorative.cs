/// <summary>
/// An item that causes effects on a Pokemon. Object pooled.
/// </summary>
public class Restorative : Item
{
	/// <summary>
	/// Gets the modification to the Pokemon's health by this item.
	/// </summary>
	public int HealthMod
	{
		get;
		private set;
	}

	/// <summary>
	/// Gets the modification to the Pokemon's stamina by this item.
	/// </summary>
	public int StaminaMod
	{
		get;
		private set;
	}

	/// <summary>
	/// Gets the status conditions inflicted by this item.
	/// </summary>
	public InflictedStatus[] InflictedStatuses
	{
		get;
		private set;
	}

	/// <summary>
	/// Gets the status conditions cured by this item.
	/// </summary>
	public Status[] CuredStatuses
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
	/// <param name="healthMod">The modification to the Pokemon's health by this item.</param>
	/// <param name="staminaMod">The modification to the Pokemon's stamina by this item.</param>
	/// <param name="inflictedStatuses">The status conditions inflicted by this item.</param>
	/// <param name="curedStatuses">The status conditions cured by this item.</param>
	/// <returns>The new object.</returns>
	public Restorative Recreate(int id, string name, string description, ushort image, float weight, int value, bool tradeable, int healthMod, int staminaMod, InflictedStatus[] inflictedStatuses, Status[] curedStatuses)
	{
		this.ID = id;
		this.Name = name;
		this.Description = description;
		this.Image = image;
		this.Weight = weight;
		this.Value = value;
		this.Tradeable = tradeable;
		this.HealthMod = healthMod;
		this.StaminaMod = staminaMod;
		this.InflictedStatuses = inflictedStatuses;
		this.CuredStatuses = curedStatuses;
		return this;
	}
}
