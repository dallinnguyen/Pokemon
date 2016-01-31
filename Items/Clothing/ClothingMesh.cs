/// <summary>
/// An equippable clothing item represented by a mesh and textures. Object pooled.
/// </summary>
public class ClothingMesh : Clothing
{
	/// <summary>
	/// Gets the asset IDs of the meshes used by the clothing.
	/// </summary>
	public ushort Mesh
	{
		get;
		private set;
	}

	/// <summary>
	/// Gets the textures used by the clothing.
	/// </summary>
	public ColoredTexture[] Textures
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
	/// <param name="category">The type of clothing.</param>
	/// <param name="mesh">The asset IDs of the meshes used by the clothing.</param>
	/// <param name="textures">The textures used by the clothing.</param>
	/// <returns>The new object.</returns>
	public ClothingMesh Recreate(int id, string name, string description, ushort image, float weight, int value, bool tradeable, ClothingCategory category, ushort mesh, ColoredTexture[] textures)
	{
		this.ID = id;
		this.Name = name;
		this.Description = description;
		this.Image = image;
		this.Weight = weight;
		this.Value = value;
		this.Tradeable = tradeable;
		this.Category = category;
		this.Mesh = mesh;
		this.Textures = textures;
		return this;
	}
}
