using System;
using System.IO;
using UnityEngine;

public static class Serialization
{
	private enum ItemIdentifier
	{
		ClothingTexture,
		ClothingMesh,
		Dye,
		Machine,
		PokeBall,
		Recipe,
		Restorative,
		Unique,
		Common
	}

	public static byte[] Serialize(Account account)
	{
		using (MemoryStream stream = new MemoryStream())
		using (BinaryWriter writer = new BinaryWriter(stream))
		{
			try
			{
				writer.Write(account.Email);
				writer.Write(account.Username);
				writer.Write((byte)account.Characters.Length);
				for (int i = 0; i < account.Characters.Length; i++)
				{
					byte[] bytes = Serialize(account.Characters[i]);
					writer.Write(bytes.Length);
					writer.Write(bytes);
				}
				return stream.ToArray();
			}
			catch (Exception e)
			{
				Debug.Log("Exception at Serialization.Serialize: " + e.Message);
				return new byte[0];
			}

		}
	}

	public static byte[] Serialize(Character character)
	{
		using (MemoryStream stream = new MemoryStream())
		using (BinaryWriter writer = new BinaryWriter(stream))
		{
			try
			{
				writer.Write(character.GivenName);
				writer.Write(character.FamilyName);
				writer.Write((byte)character.Gender);
				writer.Write(character.Athletic);
				writer.Write(character.Heavy);
				writer.Write(character.Light);
				Utility.WriteColor(writer, character.SkinColor);
				byte[] bytes = Serialize(character.PrimarySkinDetail);
				writer.Write(bytes.Length);
				writer.Write(bytes);
				bytes = Serialize(character.SecondarySkinDetail);
				writer.Write(bytes.Length);
				writer.Write(bytes);
				bytes = Serialize(character.Hair);
				writer.Write(bytes.Length);
				writer.Write(bytes);
				bytes = Serialize(character.Eyebrows);
				writer.Write(bytes.Length);
				writer.Write(bytes);
				bytes = Serialize(character.Eyes);
				writer.Write(bytes.Length);
				writer.Write(bytes);
				bytes = Serialize(character.FacialHair);
				writer.Write(bytes.Length);
				writer.Write(bytes);
				bytes = Serialize(character.Underclothing);
				writer.Write(bytes.Length);
				writer.Write(bytes);
				bytes = Serialize(character.Torso);
				writer.Write(bytes.Length);
				writer.Write(bytes);
				bytes = Serialize(character.Hands);
				writer.Write(bytes.Length);
				writer.Write(bytes);
				bytes = Serialize(character.Legs);
				writer.Write(bytes.Length);
				writer.Write(bytes);
				bytes = Serialize(character.Feet);
				writer.Write(bytes.Length);
				writer.Write(bytes);
				return stream.ToArray();
			}
			catch (Exception e)
			{
				Debug.Log("Exception at Serialization.Serialize: " + e.Message);
				return new byte[0];
			}
		}
	}

	public static byte[] Serialize(Item item)
	{
		using (MemoryStream stream = new MemoryStream())
		using (BinaryWriter writer = new BinaryWriter(stream))
		{
			try
			{
				if (item is ClothingMesh)
				{
					writer.Write((byte)ItemIdentifier.ClothingMesh);
				}
				else if (item is ClothingTexture)
				{
					writer.Write((byte)ItemIdentifier.ClothingTexture);
				}
				else if (item is Dye)
				{
					writer.Write((byte)ItemIdentifier.Dye);
				}
				else if (item is Machine)
				{
					writer.Write((byte)ItemIdentifier.Machine);
				}
				else if (item is PokeBall)
				{
					writer.Write((byte)ItemIdentifier.PokeBall);
				}
				else if (item is Recipe)
				{
					writer.Write((byte)ItemIdentifier.Recipe);
				}
				else if (item is Restorative)
				{
					writer.Write((byte)ItemIdentifier.Restorative);
				}
				else if (item is Unique)
				{
					writer.Write((byte)ItemIdentifier.Unique);
				}
				else if (item is Common)
				{
					writer.Write((byte)ItemIdentifier.Common);
				}
				writer.Write(item.ID);
				writer.Write(item.Name);
				writer.Write(item.Description);
				writer.Write(item.Image);
				writer.Write(item.Tradeable);
				writer.Write(item.Weight);
				writer.Write(item.Value);
				if (item is ClothingMesh)
				{
					ClothingMesh clothing = (ClothingMesh)item;
					writer.Write((byte)clothing.Category);
					writer.Write(clothing.Mesh);
					writer.Write((byte)clothing.Textures.Length);
					for (int i = 0; i < clothing.Textures.Length; i++)
					{
						writer.Write(clothing.Textures[i].Colorable);
						Utility.WriteColor(writer, clothing.Textures[i].Color);
						writer.Write(clothing.Textures[i].Texture);
					}
				}
				else if (item is ClothingTexture)
				{
					ClothingTexture clothing = (ClothingTexture)item;
					writer.Write((byte)clothing.Category);
					writer.Write((byte)clothing.Textures.Length);
					for (int i = 0; i < clothing.Textures.Length; i++)
					{
						writer.Write(clothing.Textures[i].Colorable);
						Utility.WriteColor(writer, clothing.Textures[i].Color);
						writer.Write(clothing.Textures[i].Texture);
					}
				}
				else if (item is Dye)
				{
					Dye dye = (Dye)item;
					Utility.WriteColor(writer, dye.Color);
				}
				else if (item is Machine)
				{
					Machine machine = (Machine)item;
					byte[] bytes = Serialize(machine.Move);
					writer.Write(bytes.Length);
					writer.Write(bytes);
				}
				else if (item is PokeBall)
				{
					PokeBall pokeBall = (PokeBall)item;
					writer.Write(pokeBall.CatchRate);
					writer.Write(pokeBall.BreakRate);
				}
				else if (item is Recipe)
				{
					Recipe recipe = (Recipe)item;
					writer.Write((ushort)recipe.Ingredients.Length);
					byte[] bytes;
					for (int i = 0; i < recipe.Ingredients.Length; i++)
					{
						bytes = Serialize(recipe.Ingredients[i].Item);
						writer.Write(bytes.Length);
						writer.Write(bytes);
						writer.Write(recipe.Ingredients[i].Amount);
					}
					bytes = Serialize(recipe.Creation);
					writer.Write(bytes.Length);
					writer.Write(bytes);
				}
				else if (item is Restorative)
				{
					Restorative restorative = (Restorative)item;
					writer.Write(restorative.HealthMod);
					writer.Write(restorative.StaminaMod);
					writer.Write((byte)restorative.InflictedStatuses.Length);
					for (int i = 0; i < restorative.InflictedStatuses.Length; i++)
					{
						writer.Write(Serialize(restorative.InflictedStatuses[i]));
					}
					writer.Write((byte)restorative.CuredStatuses.Length);
					for (int i = 0; i < restorative.CuredStatuses.Length; i++)
					{
						writer.Write(Serialize(restorative.CuredStatuses[i]));
					}
				}
				else if (item is Unique)
				{
					Unique unique = (Unique)item;
					writer.Write(unique.Triggers.Length);
					for (int i = 0; i < unique.Triggers.Length; i++)
					{
						writer.Write((int)unique.Triggers[i]);
					}
					writer.Write(unique.Usable);
					writer.Write(unique.Reusable);
					writer.Write(unique.Discardable);
				}
				return stream.ToArray();
			}
			catch (Exception e)
			{
				Debug.Log("Exception at Serialization.Serialize: " + e.Message);
				return new byte[0];
			}
		}
	}

	public static byte[] Serialize(ClothingMesh clothing)
	{
		return Serialize((Item)clothing);
	}

	public static byte[] Serialize(ClothingTexture clothing)
	{
		return Serialize((Item)clothing);
	}

	public static byte[] Serialize(Dye dye)
	{
		return Serialize((Item)dye);
	}

	public static byte[] Serialize(Machine machine)
	{
		return Serialize((Item)machine);
	}

	public static byte[] Serialize(PokeBall pokeBall)
	{
		return Serialize((Item)pokeBall);
	}

	public static byte[] Serialize(Recipe recipe)
	{
		return Serialize((Item)recipe);
	}

	public static byte[] Serialize(Restorative restorative)
	{
		return Serialize((Item)restorative);
	}

	public static byte[] Serialize(Unique unique)
	{
		return Serialize((Item)unique);
	}

	public static byte[] Serialize(Common common)
	{
		return Serialize((Item)common);
	}

	public static byte[] Serialize(Move move)
	{
		// TODO
		return null;
	}

	public static byte[] Serialize(Status status)
	{
		// TODO
		return null;
	}

	public static byte[] Serialize(InflictedStatus status)
	{
		// TODO
		return null;
	}

	public static void Deserialize(byte[] serialized, out Account account)
	{
		account = ObjectPool.Instance.New<Account>();
		using (MemoryStream stream = new MemoryStream(serialized))
		using (BinaryReader reader = new BinaryReader(stream))
		{
			try
			{
				string email = reader.ReadString();
				string username = reader.ReadString();
				Character[] characters = new Character[reader.ReadByte()];
				for (int i = 0; i < characters.Length; i++)
				{
					Deserialize(reader.ReadBytes(reader.ReadInt32()), out characters[i]);
				}
				account = account.Recreate(username, email, characters);
			}
			catch (Exception e)
			{
				Debug.Log("Exception at Serialization.Deserialize: " + e.Message);
			}
		}
	}

	public static void Deserialize(byte[] serialized, out Character character)
	{
		character = ObjectPool.Instance.New<Character>();
		using (MemoryStream stream = new MemoryStream(serialized))
		using (BinaryReader reader = new BinaryReader(stream))
		{
			try
			{
				character.Recreate();
				character.GivenName = reader.ReadString();
				character.FamilyName = reader.ReadString();
				character.Gender = (Gender)reader.ReadByte();
				character.Height = reader.ReadSingle();
				character.Athletic = reader.ReadSingle();
				character.Heavy = reader.ReadSingle();
				character.Light = reader.ReadSingle();
				character.SkinColor = Utility.ReadColor(reader);
				ClothingTexture texture;
				Deserialize(reader.ReadBytes(reader.ReadInt32()), out texture);
				character.PrimarySkinDetail = texture;
				Deserialize(reader.ReadBytes(reader.ReadInt32()), out texture);
				character.SecondarySkinDetail = texture;
				ClothingMesh mesh;
				Deserialize(reader.ReadBytes(reader.ReadInt32()), out mesh);
				character.Hair = mesh;
				Deserialize(reader.ReadBytes(reader.ReadInt32()), out texture);
				character.Eyebrows = texture;
				Deserialize(reader.ReadBytes(reader.ReadInt32()), out texture);
				character.Eyes = texture;
				Deserialize(reader.ReadBytes(reader.ReadInt32()), out mesh);
				character.FacialHair = mesh;
				Deserialize(reader.ReadBytes(reader.ReadInt32()), out texture);
				character.Underclothing = texture;
				Deserialize(reader.ReadBytes(reader.ReadInt32()), out mesh);
				character.Torso = mesh;
				Deserialize(reader.ReadBytes(reader.ReadInt32()), out mesh);
				character.Hands = mesh;
				Deserialize(reader.ReadBytes(reader.ReadInt32()), out mesh);
				character.Legs = mesh;
				Deserialize(reader.ReadBytes(reader.ReadInt32()), out mesh);
				character.Feet = mesh;
			}
			catch (IOException e)
			{
				Debug.Log("IOException at Serialization.Deserialize: " + e.Message);
			}
		}
	}

	public static void Deserialize(byte[] serialized, out Item item)
	{
		using (MemoryStream stream = new MemoryStream(serialized))
		using (BinaryReader reader = new BinaryReader(stream))
		{
			try
			{
				ItemIdentifier type = (ItemIdentifier)reader.ReadByte();
				int id = reader.ReadInt32();
				string name = reader.ReadString();
				string description = reader.ReadString();
				ushort image = reader.ReadUInt16();
				bool tradeable = reader.ReadBoolean();
				float weight = reader.ReadSingle();
				int value = reader.ReadInt32();
				if (type == ItemIdentifier.ClothingMesh)
				{
					ClothingMesh clothing = ObjectPool.Instance.New<ClothingMesh>();
					ClothingCategory category = (ClothingCategory)reader.ReadByte();
					ushort mesh = reader.ReadUInt16();
					ColoredTexture[] textures = new ColoredTexture[reader.ReadByte()];
					for (int i = 0; i < textures.Length; i++)
					{
						textures[i] = ObjectPool.Instance.New<ColoredTexture>();
						bool colorable = reader.ReadBoolean();
						Color color = Utility.ReadColor(reader);
						ushort texture = reader.ReadUInt16();
						textures[i].Recreate(colorable, color, texture);
					}
					clothing.Recreate(id, name, description, image, weight, value, tradeable, category, mesh, textures);
					item = clothing;
				}
				else if (type == ItemIdentifier.ClothingTexture)
				{
					ClothingTexture clothing = ObjectPool.Instance.New<ClothingTexture>();
					ClothingCategory category = (ClothingCategory)reader.ReadByte();
					ColoredTexture[] textures = new ColoredTexture[reader.ReadByte()];
					for (int i = 0; i < textures.Length; i++)
					{
						textures[i] = ObjectPool.Instance.New<ColoredTexture>();
						bool colorable = reader.ReadBoolean();
						Color color = Utility.ReadColor(reader);
						ushort texture = reader.ReadUInt16();
						textures[i].Recreate(colorable, color, texture);
					}
					clothing.Recreate(id, name, description, image, weight, value, tradeable, category, textures);
					item = clothing;
				}
				else if (type == ItemIdentifier.Dye)
				{
					Dye dye = ObjectPool.Instance.New<Dye>();
					Color color = Utility.ReadColor(reader);
					dye.Recreate(id, name, description, image, weight, value, tradeable, color);
					item = dye;
				}
				else if (type == ItemIdentifier.Machine)
				{
					Machine machine = ObjectPool.Instance.New<Machine>();
					Move move;
					Deserialize(reader.ReadBytes(reader.ReadInt32()), out move);
					machine.Recreate(id, name, description, image, weight, value, tradeable, move);
					item = machine;
				}
				else if (type == ItemIdentifier.PokeBall)
				{
					PokeBall pokeBall = ObjectPool.Instance.New<PokeBall>();
					float catchRate = reader.ReadSingle();
					float breakRate = reader.ReadSingle();
					PokeBall.Attribute[] attributes = new PokeBall.Attribute[reader.ReadInt32()];
					for (int i = 0; i < attributes.Length; i++)
					{
						attributes[i] = (PokeBall.Attribute)reader.ReadInt32();
					}
					pokeBall.Recreate(id, name, description, image, weight, value, tradeable, catchRate, breakRate, attributes);
					item = pokeBall;
				}
				else if (type == ItemIdentifier.Recipe)
				{
					Recipe recipe = ObjectPool.Instance.New<Recipe>();
					Ingredient[] ingredients = new Ingredient[reader.ReadUInt16()];
					for (int i = 0; i < ingredients.Length; i++)
					{
						ingredients[i] = ObjectPool.Instance.New<Ingredient>();
						Item ingredientItem;
						Deserialize(reader.ReadBytes(reader.ReadInt32()), out ingredientItem);
						int amount = reader.ReadInt32();
						ingredients[i].Recreate(ingredientItem, amount);
					}
					Item creation;
					Deserialize(reader.ReadBytes(reader.ReadInt32()), out creation);
					recipe.Recreate(id, name, description, image, weight, value, tradeable, ingredients, creation);
					item = recipe;
				}
				else if (type == ItemIdentifier.Restorative)
				{
					Restorative restorative = ObjectPool.Instance.New<Restorative>();
					int healthMod = reader.ReadInt32();
					int staminaMod = reader.ReadInt32();
					InflictedStatus[] inflictedStatuses = new InflictedStatus[reader.ReadByte()];
					for (int i = 0; i < inflictedStatuses.Length; i++)
					{
						inflictedStatuses[i] = ObjectPool.Instance.New<InflictedStatus>();
						Status status;
						Deserialize(reader.ReadBytes(reader.ReadInt32()), out status);
						float duration = reader.ReadSingle();
						inflictedStatuses[i].Recreate(status, duration);
					}
					Status[] curedStatuses = new Status[reader.ReadByte()];
					for (int i = 0; i < curedStatuses.Length; i++)
					{
						Deserialize(reader.ReadBytes(reader.ReadInt32()), out curedStatuses[i]);
					}
					restorative.Recreate(id, name, description, image, weight, value, tradeable, healthMod, staminaMod, inflictedStatuses, curedStatuses);
					item = restorative;
				}
				else if (type == ItemIdentifier.Unique)
				{
					Unique unique = ObjectPool.Instance.New<Unique>();
					Unique.Trigger[] triggers = new Unique.Trigger[reader.ReadInt32()];
					for (int i = 0; i < triggers.Length; i++)
					{
						triggers[i] = (Unique.Trigger)reader.ReadInt32();
					}
					bool usable = reader.ReadBoolean();
					bool reusable = reader.ReadBoolean();
					bool discardable = reader.ReadBoolean();
					unique.Recreate(id, name, description, image, weight, value, tradeable, triggers, usable, reusable, discardable);
					item = unique;
				}
				else
				{
					Common common = ObjectPool.Instance.New<Common>();
					common.Recreate(id, name, description, image, weight, value, tradeable);
					item = common;
				}
			}
			catch (Exception e)
			{
				item = null;
				Debug.Log("Exception at Serialization.Deserialize: " + e.Message);
			}
		}
	}

	public static void Deserialize(byte[] serialized, out ClothingMesh clothing)
	{
		Item item;
		Deserialize(serialized, out item);
		clothing = (ClothingMesh)item;
	}

	public static void Deserialize(byte[] serialized, out ClothingTexture clothing)
	{
		Item item;
		Deserialize(serialized, out item);
		clothing = (ClothingTexture)item;
	}

	public static void Deserialize(byte[] serialized, out Dye dye)
	{
		Item item;
		Deserialize(serialized, out item);
		dye = (Dye)item;
	}

	public static void Deserialize(byte[] serialized, out Machine machine)
	{
		Item item;
		Deserialize(serialized, out item);
		machine = (Machine)item;
	}

	public static void Deserialize(byte[] serialized, out PokeBall pokeBall)
	{
		Item item;
		Deserialize(serialized, out item);
		pokeBall = (PokeBall)item;
	}

	public static void Deserialize(byte[] serialized, out Recipe recipe)
	{
		Item item;
		Deserialize(serialized, out item);
		recipe = (Recipe)item;
	}

	public static void Deserialize(byte[] serialized, out Restorative restorative)
	{
		Item item;
		Deserialize(serialized, out item);
		restorative = (Restorative)item;
	}

	public static void Deserialize(byte[] serialized, out Unique unique)
	{
		Item item;
		Deserialize(serialized, out item);
		unique = (Unique)item;
	}

	public static void Deserialize(byte[] serialized, out Common common)
	{
		Item item;
		Deserialize(serialized, out item);
		common = (Common)item;
	}

	public static void Deserialize(byte[] serialized, out Move move)
	{
		// TODO
		move = null;
	}

	public static void Deserialize(byte[] serialized, out InflictedStatus status)
	{
		// TODO
		status = null;
	}

	public static void Deserialize(byte[] serialized, out Status status)
	{
		// TODO
		status = null;
	}
}
