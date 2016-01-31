using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Holds character information. Object pooled.
/// </summary>
public class Character
{
	private string name = string.Empty;
	private Gender gender = Gender.Male;
	private float height = 0.5f;
	private float athletic = 0;
	private float heavy = 0;
	private float light = 0;
	private Color skinColor = new Color(0.745f, 0.569f, 0.412f);
	private ClothingTexture primarySkinDetail;
	private ClothingTexture secondarySkinDetail;
	private ClothingMesh hair;
	private ClothingTexture eyebrows;
	private ClothingTexture eyes;
	private ClothingMesh facialHair;

	private ClothingTexture underclothing;
	private ClothingMesh torso;
	private ClothingMesh hands;
	private ClothingMesh legs;
	private ClothingMesh feet;

	/// <summary>
	/// Handles a changed string value.
	/// </summary>
	/// <param name="value">The new string value.</param>
	public delegate void StringChangeHandler(string value);

	/// <summary>
	/// Handles a changed Gender value.
	/// </summary>
	/// <param name="value">The new Gender value.</param>
	public delegate void GenderChangeHandler(Gender value);

	/// <summary>
	/// Handles a changed float value.
	/// </summary>
	/// <param name="value">The new float value.</param>
	public delegate void FloatChangeHandler(float value);

	/// <summary>
	/// Handles a changed integer value.
	/// </summary>
	/// <param name="value">The new integer value.</param>
	public delegate void IntegerChangeHandler(int value);

	/// <summary>
	/// Handles a changed Color value.
	/// </summary>
	/// <param name="value">The new Color value.</param>
	public delegate void ColorChangeHandler(Color value);

	/// <summary>
	/// Handles a changed ClothingMesh value.
	/// </summary>
	/// <param name="value">The new ClothingMesh value.</param>
	public delegate void MeshChangeHandler(ClothingMesh value);

	/// <summary>
	/// Handles a new ClothingTexture value.
	/// </summary>
	/// <param name="value">The new ClothingTexture value.</param>
	public delegate void TextureChangeHandler(ClothingTexture value);

	/// <summary>
	/// Shot when the character's given name has changed.
	/// </summary>
	public event StringChangeHandler OnGivenNameChange;

	/// <summary>
	/// Shot when the character's family name has changed.
	/// </summary>
	public event StringChangeHandler OnFamilyNameChange;

	/// <summary>
	/// Shot when the character's gender has changed.
	/// </summary>
	public event GenderChangeHandler OnGenderChange;

	/// <summary>
	/// Shot when the character's height has changed.
	/// </summary>
	public event FloatChangeHandler OnHeightChange;

	/// <summary>
	/// Shot when the character's athletic factor has changed.
	/// </summary>
	public event FloatChangeHandler OnAthleticChange;

	/// <summary>
	/// Shot when the character's heavy factor has changed.
	/// </summary>
	public event FloatChangeHandler OnHeavyChange;

	/// <summary>
	/// Shot when the character's light factor has changed.
	/// </summary>
	public event FloatChangeHandler OnLightChange;

	/// <summary>
	/// Shot when the character's skin color has changed.
	/// </summary>
	public event ColorChangeHandler OnSkinColorChange;

	/// <summary>
	/// Shot when the character's primary skin detail has changed.
	/// </summary>
	public event TextureChangeHandler OnPrimarySkinDetailChange;
	
	/// <summary>
	/// Shot when the character's secondary skin detail has changed.
	/// </summary>
	public event TextureChangeHandler OnSecondarySkinDetailChange;

	/// <summary>
	/// Shot when the character's hair has changed.
	/// </summary>
	public event MeshChangeHandler OnHairChange;

	/// <summary>
	/// Shot when the character's eyebrows have changed.
	/// </summary>
	public event TextureChangeHandler OnEyebrowsChange;

	/// <summary>
	/// Shot when the character's eyes have changed.
	/// </summary>
	public event TextureChangeHandler OnEyesChange;

	/// <summary>
	/// Shot when the character's facial hair has changed.
	/// </summary>
	public event MeshChangeHandler OnFacialHairChange;

	/// <summary>
	/// Shot when the character's underclothing has changed.
	/// </summary>
	public event TextureChangeHandler OnUnderclothingChange;

	/// <summary>
	/// Shot when the character's torso clothing has changed.
	/// </summary>
	public event MeshChangeHandler OnTorsoChange;

	/// <summary>
	/// Shot when the character's hand clothing has changed.
	/// </summary>
	public event MeshChangeHandler OnHandsChange;

	/// <summary>
	/// Shot when the character's leg clothing has changed.
	/// </summary>
	public event MeshChangeHandler OnLegsChange;

	/// <summary>
	/// Shot when the character's foot clothing has changed.
	/// </summary>
	public event MeshChangeHandler OnFeetChange;

	/// <summary>
	/// Gets or sets the collection of currently loaded game characters.
	/// </summary>
	public static Dictionary<int, Character> Characters
	{
		get;
		set;
	}

	/// <summary>
	/// Gets or sets the main player's character.
	/// </summary>
	public static Character Main
	{
		get;
		set;
	}

	/// <summary>
	/// Gets or sets a value indicating whether all changes to character customization options will trigger update events even if the value of the options have not changed.
	/// </summary>
	public bool ForceChanges
	{
		get;
		set;
	}

	/// <summary>
	/// Gets or sets the character's given name.
	/// </summary>
	public string GivenName
	{
		get
		{
			return name;
		}
		set
		{
			name = value;
			OnGivenNameChange(value);
		}
	}

	/// <summary>
	/// Gets or sets the character's family name.
	/// </summary>
	public string FamilyName
	{
		get
		{
			return name;
		}
		set
		{
			name = value;
			OnFamilyNameChange(value);
		}
	}
	
	/// <summary>
	/// Gets or sets the character's gender.
	/// </summary>
	public Gender Gender
	{
		get
		{
			return gender;
		}
		set
		{
			if (ForceChanges || value != gender)
			{
				gender = value;
				if (OnGenderChange != null)
				{
					OnGenderChange(value);
				}
			}
		}
	}

	/// <summary>
	/// Gets or sets the character's height factor, from 0 to 1.
	/// </summary>
	public float Height
	{
		get
		{
			return height;
		}
		set
		{
			height = value;
			if (OnHeightChange != null)
			{
				OnHeightChange(height);
			}
		}
	}

	/// <summary>
	/// Gets or sets the character's athletic factor, from 0 to 1.
	/// </summary>
	public float Athletic
	{
		get
		{
			return athletic;
		}
		set
		{
			athletic = value;
			if (OnAthleticChange != null)
			{
				OnAthleticChange(value);
			}
		}
	}

	/// <summary>
	/// Gets or sets the character's heavy factor, from 0 to 1.
	/// </summary>
	public float Heavy
	{
		get
		{
			return heavy;
		}
		set
		{
			heavy = value;
			if (OnHeavyChange != null)
			{
				OnHeavyChange(value);
			}
		}
	}

	/// <summary>
	/// Gets or sets the character's light factor, from 0 to 1.
	/// </summary>
	public float Light
	{
		get
		{
			return light;
		}
		set
		{
			light = value;
			if (OnLightChange != null)
			{
				OnLightChange(value);
			}
		}
	}

	/// <summary>
	/// Gets or sets the character's skin color.
	/// </summary>
	public Color SkinColor
	{
		get
		{
			return skinColor;
		}
		set
		{
			if (ForceChanges || value != skinColor)
			{
				skinColor = value;
				if (OnSkinColorChange != null)
				{
					OnSkinColorChange(value);
				}
			}
		}
	}
	
	/// <summary>
	/// Gets or sets the character's primary skin detail.
	/// </summary>
	public ClothingTexture PrimarySkinDetail
	{
		get
		{
			return primarySkinDetail;
		}
		set
		{
			if (ForceChanges || value != primarySkinDetail)
			{
				primarySkinDetail = value;
				if (OnPrimarySkinDetailChange != null)
				{
					OnPrimarySkinDetailChange(value);
				}
			}
		}
	}

	/// <summary>
	/// Gets or sets the character's secondary skin detail.
	/// </summary>
	public ClothingTexture SecondarySkinDetail
	{
		get
		{
			return secondarySkinDetail;
		}
		set
		{
			if (ForceChanges || value != secondarySkinDetail)
			{
				secondarySkinDetail = value;
				if (OnSecondarySkinDetailChange != null)
				{
					OnSecondarySkinDetailChange(value);
				}
			}
		}
	}

	/// <summary>
	/// Gets or sets the character's hair.
	/// </summary>
	public ClothingMesh Hair
	{
		get
		{
			return hair;
		}
		set
		{
			if (ForceChanges || value != hair)
			{
				hair = value;
				if (OnHairChange != null)
				{
					OnHairChange(value);
				}
			}
		}
	}

	/// <summary>
	/// Gets or sets the character's eyebrows.
	/// </summary>
	public ClothingTexture Eyebrows
	{
		get
		{
			return eyebrows;
		}
		set
		{
			if (ForceChanges || value != eyebrows)
			{
				eyebrows = value;
				if (OnEyebrowsChange != null)
				{
					OnEyebrowsChange(value);
				}
			}
		}
	}

	/// <summary>
	/// Gets or sets the character's eyes.
	/// </summary>
	public ClothingTexture Eyes
	{
		get
		{
			return eyes;
		}
		set
		{
			if (ForceChanges || value != eyes)
			{
				eyes = value;
				if (OnEyesChange != null)
				{
					OnEyesChange(value);
				}
			}
		}
	}

	/// <summary>
	/// Gets or sets the character's facial hair.
	/// </summary>
	public ClothingMesh FacialHair
	{
		get
		{
			return facialHair;
		}
		set
		{
			if (ForceChanges || value != facialHair)
			{
				facialHair = value;
				if (OnFacialHairChange != null)
				{
					OnFacialHairChange(value);
				}
			}
		}
	}

	/// <summary>
	/// Gets or sets the character's underclothing.
	/// </summary>
	public ClothingTexture Underclothing
	{
		get
		{
			return underclothing;
		}
		set
		{
			if (ForceChanges || value != underclothing)
			{
				underclothing = value;
				if (OnUnderclothingChange != null)
				{
					OnUnderclothingChange(value);
				}
			}
		}
	}

	/// <summary>
	/// Gets or sets the character's torso clothing.
	/// </summary>
	public ClothingMesh Torso
	{
		get
		{
			return torso;
		}
		set
		{
			if (ForceChanges || value != torso)
			{
				torso = value;
				if (OnTorsoChange != null)
				{
					OnTorsoChange(value);
				}
			}
		}
	}

	/// <summary>
	/// Gets or sets the character's hand clothing.
	/// </summary>
	public ClothingMesh Hands
	{
		get
		{
			return hands;
		}
		set
		{
			if (ForceChanges || value != hands)
			{
				hands = value;
				if (OnHandsChange != null)
				{
					OnHandsChange(value);
				}
			}
		}
	}

	/// <summary>
	/// Gets or sets the character's leg clothing.
	/// </summary>
	public ClothingMesh Legs
	{
		get
		{
			return legs;
		}
		set
		{
			if (ForceChanges || value != legs)
			{
				legs = value;
				if (OnLegsChange != null)
				{
					OnLegsChange(value);
				}
			}
		}
	}

	/// <summary>
	/// Gets or sets the character's foot clothing.
	/// </summary>
	public ClothingMesh Feet
	{
		get
		{
			return feet;
		}
		set
		{
			if (ForceChanges || value != feet)
			{
				feet = value;
				if (OnFeetChange != null)
				{
					OnFeetChange(value);
				}
			}
		}
	}

	/// <summary>
	/// Gets or sets the character's inventory.
	/// </summary>
	public List<Item> Inventory
	{
		get;
		set;
	}

	/// <summary>
	/// Gets or sets the character's currently active Pokemon.
	/// </summary>
	public Pokemon ActivePokemon
	{
		get;
		set;
    }

	/// <summary>
	/// Gets or sets the character's Pokemon.
	/// </summary>
	public Pokemon[] Pokemon
	{
		get;
		set;
	}

	/// <summary>
	/// Clears all customization option update event subscriptions.
	/// </summary>
	public void ClearCustomizationEvents()
	{
		OnGivenNameChange = null;
		OnFamilyNameChange = null;
		OnGenderChange = null;
		OnHeightChange = null;
		OnAthleticChange = null;
		OnHeavyChange = null;
		OnLightChange = null;
		OnSkinColorChange = null;
		OnPrimarySkinDetailChange = null;
		OnSecondarySkinDetailChange = null;
		OnHairChange = null;
		OnEyebrowsChange = null;
		OnEyesChange = null;
		OnFacialHairChange = null;

		OnUnderclothingChange = null;
		OnTorsoChange = null;
		OnHandsChange = null;
		OnLegsChange = null;
		OnFeetChange = null;
	}

	/// <summary>
	/// Recreates this object with new values.
	/// </summary>
	/// <returns>The new object.</returns>
	public Character Recreate()
	{
		ClearCustomizationEvents();

		name = string.Empty;
		gender = Gender.Male;
		height = 0.5f;
		athletic = 0;
		heavy = 0;
		light = 0;
		skinColor = new Color(0.745f, 0.569f, 0.412f);
		primarySkinDetail = null;
		secondarySkinDetail = null;
		hair = null;
		eyebrows = null;
		eyes = null;
		facialHair = null;

		underclothing = null;
		torso = null;
		hands = null;
		legs = null;
		feet = null;
		return this;
	}
}
