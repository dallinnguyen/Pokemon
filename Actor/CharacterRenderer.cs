using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// Takes a Character object and represents it in the game world.
/// </summary>
public class CharacterRenderer : PPBehaviour
{
	private readonly Color defaultEyeColor = new Color(0.42f, 0.267f, 0.137f);

	private readonly BoneModifier blankModifier = new BoneModifier(null, Vector3.zero, Vector3.zero, Vector3.zero);
	private BoneModifier[] baseBones;
	private BoneModifier[][] boneMods;

	private Character character;
	private Animation anim;
	private SkinnedMeshRenderer bodyObject;
	private SkinnedMeshRenderer facialHairObject;
	private SkinnedMeshRenderer hairObject;
	private SkinnedMeshRenderer torsoObject;
	private SkinnedMeshRenderer handsObject;
	private SkinnedMeshRenderer legsObject;
	private SkinnedMeshRenderer feetObject;
	private GameObject masterBone;

	private bool init;

	/// <summary>
	/// Gets or sets the character to render.
	/// </summary>
	public Character Character
	{
		get
		{
			return character;
		}
		set
		{
			init = true;
			try
			{
				if (character != null)
				{
					character.OnGenderChange -= OnGenderChange;
					character.OnSkinColorChange -= OnSkinColorChange;
					character.OnPrimarySkinDetailChange -= OnPrimarySkinDetailChange;
					character.OnSecondarySkinDetailChange -= OnSecondarySkinDetailChange;
					character.OnHairChange -= OnHairChange;
					character.OnEyebrowsChange -= OnEyebrowsChange;
					character.OnEyesChange -= OnEyesChange;
					character.OnFacialHairChange -= OnFacialHairChange;

					character.OnUnderclothingChange -= OnUnderclothingChange;
					character.OnTorsoChange -= OnTorsoChange;
					character.OnHandsChange -= OnHandsChange;
					character.OnLegsChange -= OnLegsChange;
					character.OnFeetChange -= OnFeetChange;
				}

				character = value;
				character.OnGenderChange += OnGenderChange;
				character.OnSkinColorChange += OnSkinColorChange;
				character.OnPrimarySkinDetailChange += OnPrimarySkinDetailChange;
				character.OnSecondarySkinDetailChange += OnSecondarySkinDetailChange;
				character.OnHairChange += OnHairChange;
				character.OnEyebrowsChange += OnEyebrowsChange;
				character.OnEyesChange += OnEyesChange;
				character.OnFacialHairChange += OnFacialHairChange;

				character.OnUnderclothingChange += OnUnderclothingChange;
				character.OnTorsoChange += OnTorsoChange;
				character.OnHandsChange += OnHandsChange;
				character.OnLegsChange += OnLegsChange;
				character.OnFeetChange += OnFeetChange;

				OnGenderChange(Character.Gender);
				OnSkinColorChange(Character.SkinColor);
				OnPrimarySkinDetailChange(Character.PrimarySkinDetail);
				OnSecondarySkinDetailChange(Character.SecondarySkinDetail);
				OnHairChange(Character.Hair);
				OnEyebrowsChange(Character.Eyebrows);
				OnEyesChange(Character.Eyes);
				OnFacialHairChange(Character.FacialHair);

				OnUnderclothingChange(Character.Underclothing);
				OnTorsoChange(Character.Torso);
				OnHandsChange(Character.Hands);
				OnLegsChange(Character.Legs);
				OnFeetChange(Character.Feet);
			}
			catch (System.ArgumentException e)
			{
				throw new System.ArgumentException("Failed to load character into character renderer", e);
			}
			finally
			{
				init = false;
			}
		}
	}

	/// <summary>
	/// Plays an animation on the character.
	/// </summary>
	/// <param name="animation">The name of the animation.</param>
	/// <param name="loop">If true the animation will loop until another animation is played, otherwise the animation will be played once.</param>
	/// <returns>A Coroutine that ends when the animation has finished.</returns>
	public Coroutine Animate(string animation, bool loop = false)
	{
		return StartCoroutine(AnimateInternal(animation, loop));
	}

	private IEnumerator AnimateInternal(string animation, bool loop = false)
	{
		if (loop)
		{
			anim[animation].wrapMode = WrapMode.Loop;
		}
		else
		{
			anim[animation].wrapMode = WrapMode.Once;
		}

		anim.CrossFade(animation);
		while (anim.IsPlaying(animation) && !loop)
		{
			yield return null;
		}
	}

	private void OnGenderChange(Gender value)
	{
		try
		{
			GameObject obj = Utility.Load<GameObject>("Art/Character/" + value.ToString() + "/Body/Models/Body");
			GameObject instance = Instantiate(obj);
			bodyObject.sharedMesh = instance.GetComponentInChildren<SkinnedMeshRenderer>().sharedMesh;
			Destroy(instance);
			bodyObject.material.SetTexture("_MainTex", Utility.Load<Texture2D>("Art/Character/" + value.ToString() + "/Body/Textures/Body"));
			LoadBoneSetups();
		}
		finally
		{
			Resources.UnloadUnusedAssets();
		}
	}
	private void OnSkinColorChange(Color value)
	{
		bodyObject.material.SetColor("_SkinColor", value);
	}
	private void OnPrimarySkinDetailChange(ClothingTexture value)
	{
		try
		{
			if (value == null)
			{
				if (Character.SecondarySkinDetail == null)
				{
					bodyObject.material.shader = Utility.FindShader("PP/Character (Detail x0)");
				}
				else
				{
					bodyObject.material.shader = Utility.FindShader("PP/Character (Detail x1)");
					bodyObject.material.SetTexture("_PrimarySkinDetailTex", Utility.Load<Texture2D>("Art/Character/" + Character.Gender.ToString() + "/Skin Detail/Overlays/" + Character.SecondarySkinDetail.Textures[0].Texture));
				}
			}
			else
			{
				if (Character.SecondarySkinDetail == null)
				{
					bodyObject.material.shader = Utility.FindShader("PP/Character (Detail x1)");
					bodyObject.material.SetTexture("_PrimarySkinDetailTex", Utility.Load<Texture2D>("Art/Character/" + Character.Gender.ToString() + "/Skin Detail/Overlays/" + value.Textures[0].Texture));
				}
				else
				{
					bodyObject.material.shader = Utility.FindShader("PP/Character (Detail x2)");
					bodyObject.material.SetTexture("_PrimarySkinDetailTex", Utility.Load<Texture2D>("Art/Character/" + Character.Gender.ToString() + "/Skin Detail/Overlays/" + value.Textures[0].Texture));
					bodyObject.material.SetTexture("_SecondarySkinDetailTex", Utility.Load<Texture2D>("Art/Character/" + Character.Gender.ToString() + "/Skin Detail/Overlays/" + Character.SecondarySkinDetail.Textures[0].Texture));
				}
			}
		}
		finally
		{
			Resources.UnloadUnusedAssets();
		}
	}
	private void OnSecondarySkinDetailChange(ClothingTexture value)
	{
		try
		{
			if (value == null)
			{
				if (Character.PrimarySkinDetail == null)
				{
					bodyObject.material.shader = Utility.FindShader("PP/Character (Detail x0)");
				}
				else
				{
					bodyObject.material.shader = Utility.FindShader("PP/Character (Detail x1)");
					bodyObject.material.SetTexture("_PrimarySkinDetailTex", Utility.Load<Texture2D>("Art/Character/" + Character.Gender.ToString() + "/Skin Detail/Overlays/" + Character.PrimarySkinDetail.Textures[0].Texture));
				}
			}
			else
			{
				if (Character.PrimarySkinDetail == null)
				{
					bodyObject.material.shader = Utility.FindShader("PP/Character (Detail x1)");
					bodyObject.material.SetTexture("_PrimarySkinDetailTex", Utility.Load<Texture2D>("Art/Character/" + Character.Gender.ToString() + "/Skin Detail/Overlays/" + value.Textures[0].Texture));
				}
				else
				{
					bodyObject.material.shader = Utility.FindShader("PP/Character (Detail x2)");
					bodyObject.material.SetTexture("_PrimarySkinDetailTex", Utility.Load<Texture2D>("Art/Character/" + Character.Gender.ToString() + "/Skin Detail/Overlays/" + Character.PrimarySkinDetail.Textures[0].Texture));
					bodyObject.material.SetTexture("_SecondarySkinDetailTex", Utility.Load<Texture2D>("Art/Character/" + Character.Gender.ToString() + "/Skin Detail/Overlays/" + value.Textures[0].Texture));
				}
			}
		}
		finally
		{
			Resources.UnloadUnusedAssets();
		}
	}
	private void OnHairChange(ClothingMesh value)
	{
		try
		{
			if (value == null)
			{
				hairObject.sharedMesh = null;
			}
			else
			{
				hairObject.sharedMesh = ExtractMesh(Utility.Load<GameObject>("Art/Character/" + Character.Gender.ToString() + "/Hair/Models/" + value.Mesh));

				if (value.Textures[0].Texture == 0)
				{
					hairObject.material.SetTexture("_MainTex", Utility.Load<Texture2D>("Art/General/Textures/Clear"));
				}
				else if (value.Textures[0].Texture == 1)
				{
					hairObject.material.SetTexture("_MainTex", Utility.Load<Texture2D>("Art/General/Textures/White"));
				}
				else
				{
					hairObject.material.SetTexture("_MainTex", Utility.Load<Texture2D>("Art/Character/" + Character.Gender.ToString() + "/Hair/Textures/" + value.Textures[0].Texture));
				}
				hairObject.material.SetColor("_MainColor", value.Textures[0].Color);

				hairObject.material.shader = Utility.FindShader("PP/Toon Outline (Overlay x" + (value.Textures.Length - 1) + ")");
				for (int i = 1; i < value.Textures.Length; i++)
				{
					if (value.Textures[i].Texture == 0)
					{
						hairObject.material.SetTexture("_OverlayTex" + i, Utility.Load<Texture2D>("Art/General/Textures/Clear"));
					}
					else if (value.Textures[i].Texture == 1)
					{
						hairObject.material.SetTexture("_OverlayTex" + i, Utility.Load<Texture2D>("Art/General/Textures/White"));
					}
					else
					{
						hairObject.material.SetTexture("_OverlayTex" + i, Utility.Load<Texture2D>("Art/Character/" + Character.Gender.ToString() + "/Hair/Overlays (" + i + ")/" + value.Textures[i].Texture));
					}
					hairObject.material.SetColor("_OverlayColor" + i, value.Textures[i].Color);
				}
			}
		}
		finally
		{
			Resources.UnloadUnusedAssets();
		}
	}
	private void OnEyebrowsChange(ClothingTexture value)
	{
		try
		{
			if (value == null)
			{
				bodyObject.material.SetTexture("_EyebrowTex", Utility.Load<Texture2D>("Art/General/Textures/Clear"));
			}
			else
			{
				if (value.Textures[0].Texture == 0)
				{
					bodyObject.material.SetTexture("_EyebrowTex", Utility.Load<Texture2D>("Art/General/Textures/Clear"));
				}
				else if (value.Textures[0].Texture == 1)
				{
					bodyObject.material.SetTexture("_EyebrowTex", Utility.Load<Texture2D>("Art/General/Textures/White"));
				}
				else
				{
					bodyObject.material.SetTexture("_EyebrowTex", Utility.Load<Texture2D>("Art/Character/" + Character.Gender.ToString() + "/Eyebrows/Overlays/" + value.Textures[0].Texture));
				}
				bodyObject.material.SetColor("_EyebrowColor", value.Textures[0].Color);
			}
		}
		finally
		{
			Resources.UnloadUnusedAssets();
		}
	}
	private void OnEyesChange(ClothingTexture value)
	{
		try
		{
			if (value == null)
			{
				bodyObject.material.SetTexture("_EyeTex", Utility.Load<Texture2D>("Art/Character/" + Character.Gender.ToString() + "/Eyes/Overlays (1)/2"));
				bodyObject.material.SetTexture("_IrisTex", Utility.Load<Texture2D>("Art/Character/" + Character.Gender.ToString() + "/Eyes/Overlays (2)/2"));
				bodyObject.material.SetColor("_IrisColor1", defaultEyeColor);
				bodyObject.material.SetColor("_IrisColor2", defaultEyeColor);
			}
			else
			{
				if (value.Textures[0].Texture == 0)
				{
					bodyObject.material.SetTexture("_EyeTex", Utility.Load<Texture2D>("Art/General/Textures/Clear"));
				}
				else if (value.Textures[0].Texture == 1)
				{
					bodyObject.material.SetTexture("_EyeTex", Utility.Load<Texture2D>("Art/General/Textures/White"));
				}
				else
				{
					bodyObject.material.SetTexture("_EyeTex", Utility.Load<Texture2D>("Art/Character/" + Character.Gender.ToString() + "/Eyes/Overlays (1)/" + value.Textures[0].Texture));
				}
				bodyObject.material.SetColor("_EyeColor", value.Textures[0].Color);

				if (value.Textures[1].Texture == 0)
				{
					bodyObject.material.SetTexture("_IrisTex", Utility.Load<Texture2D>("Art/General/Textures/Clear"));
				}
				else if (value.Textures[1].Texture == 1)
				{
					bodyObject.material.SetTexture("_IrisTex", Utility.Load<Texture2D>("Art/General/Textures/White"));
				}
				else
				{
					bodyObject.material.SetTexture("_IrisTex", Utility.Load<Texture2D>("Art/Character/" + Character.Gender.ToString() + "/Eyes/Overlays (2)/" + value.Textures[1].Texture));
				}
				bodyObject.material.SetColor("_IrisColor1", value.Textures[1].Color);
				bodyObject.material.SetColor("_IrisColor2", value.Textures[2].Color);
			}
		}
		finally
		{
			Resources.UnloadUnusedAssets();
		}
	}
	private void OnFacialHairChange(ClothingMesh value)
	{
		try
		{
			if (value == null)
			{
				facialHairObject.sharedMesh = null;
			}
			else
			{
				facialHairObject.sharedMesh = ExtractMesh(Utility.Load<GameObject>("Art/Character/" + Character.Gender.ToString() + "/Facial Hair/Models/" + value.Mesh));

				if (value.Textures[0].Texture == 0)
				{
					facialHairObject.material.SetTexture("_MainTex", Utility.Load<Texture2D>("Art/General/Textures/Clear"));
				}
				else if (value.Textures[0].Texture == 1)
				{
					facialHairObject.material.SetTexture("_MainTex", Utility.Load<Texture2D>("Art/General/Textures/White"));
				}
				else
				{
					facialHairObject.material.SetTexture("_MainTex", Utility.Load<Texture2D>("Art/Character/" + Character.Gender.ToString() + "/Facial Hair/Textures/" + value.Textures[0].Texture));
				}
				facialHairObject.material.SetColor("_MainColor", value.Textures[0].Color);
			}
		}
		finally
		{
			Resources.UnloadUnusedAssets();
		}
	}

	private void OnUnderclothingChange(ClothingTexture value)
	{
		try
		{
			if (value == null)
			{
				bodyObject.material.SetTexture("_UnderclothingTex", Utility.Load<Texture2D>("Art/General/Textures/Clear"));
			}
			else
			{
				if (value.Textures[0].Texture == 0)
				{
					bodyObject.material.SetTexture("_UnderclothingTex", Utility.Load<Texture2D>("Art/General/Textures/Clear"));
				}
				else if (value.Textures[0].Texture == 1)
				{
					bodyObject.material.SetTexture("_UnderclothingTex", Utility.Load<Texture2D>("Art/General/Textures/White"));
				}
				else
				{
					bodyObject.material.SetTexture("_UnderclothingTex", Utility.Load<Texture2D>("Art/Character/" + Character.Gender.ToString() + "/Underclothing/Overlays/" + value.Textures[0].Texture));
				}
				bodyObject.material.SetColor("_UnderclothingColor", value.Textures[0].Color);
			}
		}
		finally
		{
			Resources.UnloadUnusedAssets();
		}
	}
	private void OnTorsoChange(ClothingMesh value)
	{
		try
		{
			if (value == null)
			{
				torsoObject.sharedMesh = null;
			}
			else
			{
				torsoObject.sharedMesh = ExtractMesh(Utility.Load<GameObject>("Art/Character/" + Character.Gender.ToString() + "/Torso/Models/" + value.Mesh));

				if (value.Textures[0].Texture == 0)
				{
					torsoObject.material.SetTexture("_MainTex", Utility.Load<Texture2D>("Art/General/Textures/Clear"));
				}
				else if (value.Textures[0].Texture == 1)
				{
					torsoObject.material.SetTexture("_MainTex", Utility.Load<Texture2D>("Art/General/Textures/White"));
				}
				else
				{
					torsoObject.material.SetTexture("_MainTex", Utility.Load<Texture2D>("Art/Character/" + Character.Gender.ToString() + "/Torso/Textures/" + value.Textures[0].Texture));
				}
				torsoObject.material.SetColor("_MainColor", value.Textures[0].Color);

				torsoObject.material.shader = Utility.FindShader("PP/Toon Outline (Overlay x" + (value.Textures.Length - 1) + ")");
				for (int i = 1; i < value.Textures.Length; i++)
				{
					if (value.Textures[i].Texture == 0)
					{
						torsoObject.material.SetTexture("_OverlayTex" + i, Utility.Load<Texture2D>("Art/General/Textures/Clear"));
					}
					else if (value.Textures[i].Texture == 1)
					{
						torsoObject.material.SetTexture("_OverlayTex" + i, Utility.Load<Texture2D>("Art/General/Textures/White"));
					}
					else
					{
						torsoObject.material.SetTexture("_OverlayTex" + i, Utility.Load<Texture2D>("Art/Character/" + Character.Gender.ToString() + "/Torso/Overlays (" + i + ")/" + value.Textures[i].Texture));
					}
					torsoObject.material.SetColor("_OverlayColor" + i, value.Textures[i].Color);
				}
			}
		}
		finally
		{
			Resources.UnloadUnusedAssets();
		}
	}
	private void OnHandsChange(ClothingMesh value)
	{
		try
		{
			if (value == null)
			{
				handsObject.sharedMesh = null;
			}
			else
			{
				handsObject.sharedMesh = ExtractMesh(Utility.Load<GameObject>("Art/Character/" + Character.Gender.ToString() + "/Hands/Models/" + value.Mesh));

				if (value.Textures[0].Texture == 0)
				{
					handsObject.material.SetTexture("_MainTex", Utility.Load<Texture2D>("Art/General/Textures/Clear"));
				}
				else if (value.Textures[0].Texture == 1)
				{
					handsObject.material.SetTexture("_MainTex", Utility.Load<Texture2D>("Art/General/Textures/White"));
				}
				else
				{
					handsObject.material.SetTexture("_MainTex", Utility.Load<Texture2D>("Art/Character/" + Character.Gender.ToString() + "/Hands/Textures/" + value.Textures[0].Texture));
				}
				handsObject.material.SetColor("_MainColor", value.Textures[0].Color);

				handsObject.material.shader = Utility.FindShader("PP/Toon Outline (Overlay x" + (value.Textures.Length - 1) + ")");
				for (int i = 1; i < value.Textures.Length; i++)
				{
					if (value.Textures[i].Texture == 0)
					{
						handsObject.material.SetTexture("_OverlayTex" + i, Utility.Load<Texture2D>("Art/General/Textures/Clear"));
					}
					else if (value.Textures[i].Texture == 1)
					{
						handsObject.material.SetTexture("_OverlayTex" + i, Utility.Load<Texture2D>("Art/General/Textures/White"));
					}
					else
					{
						handsObject.material.SetTexture("_OverlayTex" + i, Utility.Load<Texture2D>("Art/Character/" + Character.Gender.ToString() + "/Hands/Overlays (" + i + ")/" + value.Textures[i].Texture));
					}
					handsObject.material.SetColor("_OverlayColor" + i, value.Textures[i].Color);
				}
			}
		}
		finally
		{
			Resources.UnloadUnusedAssets();
		}
	}
	private void OnLegsChange(ClothingMesh value)
	{
		try
		{
			if (value == null)
			{
				legsObject.sharedMesh = null;
			}
			else
			{
				legsObject.sharedMesh = ExtractMesh(Utility.Load<GameObject>("Art/Character/" + Character.Gender.ToString() + "/Legs/Models/" + value.Mesh));

				if (value.Textures[0].Texture == 0)
				{
					legsObject.material.SetTexture("_MainTex", Utility.Load<Texture2D>("Art/General/Textures/Clear"));
				}
				else if (value.Textures[0].Texture == 1)
				{
					legsObject.material.SetTexture("_MainTex", Utility.Load<Texture2D>("Art/General/Textures/White"));
				}
				else
				{
					legsObject.material.SetTexture("_MainTex", Utility.Load<Texture2D>("Art/Character/" + Character.Gender.ToString() + "/Legs/Textures/" + value.Textures[0].Texture));
				}
				legsObject.material.SetColor("_MainColor", value.Textures[0].Color);

				legsObject.material.shader = Utility.FindShader("PP/Toon Outline (Overlay x" + (value.Textures.Length - 1) + ")");
				for (int i = 1; i < value.Textures.Length; i++)
				{
					if (value.Textures[i].Texture == 0)
					{
						legsObject.material.SetTexture("_OverlayTex" + i, Utility.Load<Texture2D>("Art/General/Textures/Clear"));
					}
					else if (value.Textures[i].Texture == 1)
					{
						legsObject.material.SetTexture("_OverlayTex" + i, Utility.Load<Texture2D>("Art/General/Textures/White"));
					}
					else
					{
						legsObject.material.SetTexture("_OverlayTex" + i, Utility.Load<Texture2D>("Art/Character/" + Character.Gender.ToString() + "/Legs/Overlays (" + i + ")/" + value.Textures[i].Texture));
					}
					legsObject.material.SetColor("_OverlayColor" + i, value.Textures[i].Color);
				}
			}
		}
		finally
		{
			Resources.UnloadUnusedAssets();
		}
	}
	private void OnFeetChange(ClothingMesh value)
	{
		try
		{
			if (value == null)
			{
				feetObject.sharedMesh = null;
			}
			else
			{
				feetObject.sharedMesh = ExtractMesh(Utility.Load<GameObject>("Art/Character/" + Character.Gender.ToString() + "/Feet/Models/" + value.Mesh));

				if (value.Textures[0].Texture == 0)
				{
					feetObject.material.SetTexture("_MainTex", Utility.Load<Texture2D>("Art/General/Textures/Clear"));
				}
				else if (value.Textures[0].Texture == 1)
				{
					feetObject.material.SetTexture("_MainTex", Utility.Load<Texture2D>("Art/General/Textures/White"));
				}
				else
				{
					feetObject.material.SetTexture("_MainTex", Utility.Load<Texture2D>("Art/Character/" + Character.Gender.ToString() + "/Feet/Textures/" + value.Textures[0].Texture));
				}
				feetObject.material.SetColor("_MainColor", value.Textures[0].Color);

				feetObject.material.shader = Utility.FindShader("PP/Toon Outline (Overlay x" + (value.Textures.Length - 1) + ")");
				for (int i = 1; i < value.Textures.Length; i++)
				{
					if (value.Textures[i].Texture == 0)
					{
						feetObject.material.SetTexture("_OverlayTex" + i, Utility.Load<Texture2D>("Art/General/Textures/Clear"));
					}
					else if (value.Textures[i].Texture == 1)
					{
						feetObject.material.SetTexture("_OverlayTex" + i, Utility.Load<Texture2D>("Art/General/Textures/White"));
					}
					else
					{
						feetObject.material.SetTexture("_OverlayTex" + i, Utility.Load<Texture2D>("Art/Character/" + Character.Gender.ToString() + "/Feet/Overlays (" + i + ")/" + value.Textures[i].Texture));
					}
					feetObject.material.SetColor("_OverlayColor" + i, value.Textures[i].Color);
				}
			}
		}
		finally
		{
			Resources.UnloadUnusedAssets();
		}
	}

	private void Awake()
	{
		anim = GetComponent<Animation>();
		masterBone = FindChild("master");
		bodyObject = FindChild("Body").GetComponent<SkinnedMeshRenderer>();
		hairObject = FindChild("Hair").GetComponent<SkinnedMeshRenderer>();
		facialHairObject = FindChild("Facial Hair").GetComponent<SkinnedMeshRenderer>();
		torsoObject = FindChild("Torso").GetComponent<SkinnedMeshRenderer>();
		handsObject = FindChild("Hands").GetComponent<SkinnedMeshRenderer>();
		legsObject = FindChild("Legs").GetComponent<SkinnedMeshRenderer>();
		feetObject = FindChild("Feet").GetComponent<SkinnedMeshRenderer>();

		torsoObject.rootBone = handsObject.rootBone = legsObject.rootBone = feetObject.rootBone = bodyObject.rootBone;
		torsoObject.bones = handsObject.bones = legsObject.bones = feetObject.bones = bodyObject.bones;
	}

	private void LoadBoneSetups()
	{
		baseBones = LoadBoneModifiers(Utility.Load<TextAsset>("Art/Character/" + Character.Gender.ToString() + "/Body/Bone Setups/Base"), true);
		boneMods = new BoneModifier[(int)BoneSetup.Length][];
		for (int i = 0; i < (int)BoneSetup.Length; i++)
		{
			boneMods[i] = LoadBoneModifiers(Utility.Load<TextAsset>("Art/Character/" + Character.Gender.ToString() + "/Body/Bone Setups/" + ((BoneSetup)i).ToString()));
		}
	}

	private BoneModifier[] LoadBoneModifiers(TextAsset boneSetup, bool isBase = false)
	{
		List<BoneModifier> list = new List<BoneModifier>();
		using (MemoryStream stream = new MemoryStream(boneSetup.bytes))
		using (BinaryReader reader = new BinaryReader(stream))
		{
			int boneCount = reader.ReadInt32();
			for (int i = 0; i < boneCount; i++)
			{
				string name = reader.ReadString();
				GameObject bone = FindBone(masterBone, name);
				Vector3 position = ReadVector3(reader);
				Vector3 rotation = ReadVector3(reader);
				Vector3 scale = ReadVector3(reader);
				if (bone != null)
				{
					if (!isBase)
					{
						BoneModifier baseBone = blankModifier;
						for (int j = 0; j < baseBones.Length; j++)
						{
							if (baseBones[j].Bone.name.Equals(name))
							{
								baseBone = baseBones[j];
							}
						}
						if (baseBone.Bone != null)
						{
							if (true)
							{
								position -= baseBone.Position;
								rotation -= baseBone.Rotation;
								scale = new Vector3(scale.x / baseBone.Scale.x, scale.y / baseBone.Scale.y, scale.z / baseBone.Scale.z);
							}
						}
					}

					list.Add(new BoneModifier(bone.transform, position, rotation, scale));
				}
			}
		}
		return list.ToArray();
	}

	private Vector3 ReadVector3(BinaryReader reader)
	{
		return new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
	}

	private GameObject FindBone(GameObject master, string name)
	{
		if (master.name.Equals(name))
		{
			return master;
		}
		foreach (Transform child in master.transform)
		{
			GameObject returnedObject = FindBone(child.gameObject, name);
			if (returnedObject != null)
			{
				return returnedObject;
			}
		}
		return null;
	}

	private void LateUpdate()
	{
		if (Character != null)
		{
			if (Character.Height >= 0.5f)
			{
				ApplyBoneModifiers(BoneSetup.Tall, (Character.Height - 0.5f) * 2);
			}
			else
			{
				ApplyBoneModifiers(BoneSetup.Short, 1 - (Character.Height * 2));
			}

			ApplyBoneModifiers(BoneSetup.Athletic, Character.Athletic);
			ApplyBoneModifiers(BoneSetup.Heavy, Character.Heavy);
			ApplyBoneModifiers(BoneSetup.Light, Character.Light);
		}
	}

	private void ApplyBoneModifiers(BoneSetup boneSetup, float strength)
	{
		BoneModifier[] mods = boneMods[(int)boneSetup];
		for (int i = 0; i < mods.Length; i++)
		{
			mods[i].Bone.position += Quaternion.Euler(transform.rotation.eulerAngles) * Vector3.Lerp(Vector3.zero, mods[i].Position, strength);
			mods[i].Bone.rotation = Quaternion.Euler(mods[i].Bone.rotation.eulerAngles + Vector3.Lerp(Vector3.zero, mods[i].Rotation, strength));
			mods[i].Bone.localScale = Vector3.Lerp(mods[i].Bone.localScale, new Vector3(mods[i].Bone.localScale.x * mods[i].Scale.x, mods[i].Bone.localScale.y * mods[i].Scale.y, mods[i].Bone.localScale.z * mods[i].Scale.z), strength);
		}
	}

	private Mesh ExtractMesh(GameObject prefab)
	{
		GameObject instance = Instantiate(prefab);
		MeshFilter meshFilter = null;
		SkinnedMeshRenderer skinnedFilter = null;
		Mesh mesh = null;

		if ((meshFilter = instance.GetComponent<MeshFilter>()) != null)
		{
			mesh = meshFilter.sharedMesh;
		}
		else if ((meshFilter = instance.GetComponentInChildren<MeshFilter>()) != null)
		{
			mesh = meshFilter.sharedMesh;
		}
		else if ((skinnedFilter = instance.GetComponent<SkinnedMeshRenderer>()) != null)
		{
			mesh = skinnedFilter.sharedMesh;
		}
		else if ((skinnedFilter = instance.GetComponentInChildren<SkinnedMeshRenderer>()) != null)
		{
			mesh = skinnedFilter.sharedMesh;
		}

		Destroy(instance);
		return mesh;
	}

	private struct BoneModifier
	{
		public readonly Transform Bone;
		public readonly Vector3 Position;
		public readonly Vector3 Rotation;
		public readonly Vector3 Scale;

		public BoneModifier(Transform bone, Vector3 position, Vector3 rotation, Vector3 scale)
		{
			this.Bone = bone;
			this.Position = position;
			this.Rotation = rotation;
			this.Scale = scale;
		}
	}
}
