/// <summary>
/// An equippable clothing item.
/// </summary>
public abstract class Clothing : Item
{
	/// <summary>
	/// Gets or sets the type of clothing.
	/// </summary>
	public ClothingCategory Category
	{
		get;
		protected set;
	}
}
