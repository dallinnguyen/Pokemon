using System;
using System.Collections;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

/// <summary>
/// Container for utility methods.
/// </summary>
public static class Utility
{
	/// <summary>
	/// Formats a string to PascalCase
	/// </summary>
	/// <param name="pascalString">The original string</param>
	/// <returns>The formatted string</returns>
	public static string PascalSpaces(string pascalString)
	{
		return System.Text.RegularExpressions.Regex.Replace(pascalString, "([a-z](?=[A-Z])|[A-Z](?=[A-Z][a-z]))", "$1 ");
	}

	/// <summary>
	/// Extracts the hash (i.e. the first 29 characters) of a bcrypt hash
	/// </summary>
	/// <param name="bcryptHash">The complete bcrypt hash</param>
	/// <returns>The salt of the hash</returns>
	public static string GetSaltFromBCryptHash(string bcryptHash)
	{
		if (!string.IsNullOrEmpty(bcryptHash))
		{
			return bcryptHash.Substring(0, 29);
		}
		else
		{
			return string.Empty;
		}
	}

	/// <summary>
	/// Generates an MD5 hash for the given file.
	/// </summary>
	/// <param name="path">File path</param>
	/// <returns>The MD5 hash as a hex-string</returns>
	public static string HashMD5(string path)
	{
		using (var md5 = System.Security.Cryptography.MD5.Create())
		{
			using (var stream = System.IO.File.OpenRead(path))
			{
				return BitConverter.ToString(md5.ComputeHash(stream)).Replace("-", string.Empty).ToLower();
			}
		}
	}

	/// <summary>
	/// Generates a unix timestamp of the current time.
	/// </summary>
	/// <returns>The unix timestamp of the current time.</returns>
	public static int UnixTimestamp()
	{
		return (int)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds;
	}

	/// <summary>
	/// Generates a bcrypt hash of a text using the given salt
	/// </summary>
	/// <param name="plain">The plaintext to be hashed</param>
	/// <param name="salt">The salt to be used by bcrypt</param>
	/// <returns>The bcrypt hash of the string</returns>
	public static string HashBCrypt(string plain, string salt)
	{
		return CryptSharp.BlowfishCrypter.Blowfish.Crypt(plain, salt);
	}

	/// <summary>
	/// Generates a hash using the SHA512 encryption algorithim.
	/// </summary>
	/// <param name="plaintext">The string to be hashed.</param>
	/// <returns>The SHA512 hash.</returns>
	public static string HashSHA512(string plaintext)
	{
		using (SHA512Managed sha = new SHA512Managed())
		{
			string hash = BitConverter.ToString(sha.ComputeHash(Encoding.UTF8.GetBytes(plaintext)));
			return hash.Replace("-", string.Empty);
		}
	}

	/// <summary>
	/// Finds the child of this GameObject with the given name.
	/// </summary>
	/// <param name="start">The root.</param>
	/// <param name="name">The name the child.</param>
	/// <returns>The child.</returns>
	public static GameObject FindChild(this GameObject start, string name)
	{
		if (start.name.Equals(name))
		{
			return start;
		}
		foreach (Transform child in start.transform)
		{
			GameObject returnedObject = FindChild(child.gameObject, name);
			if (returnedObject != null)
			{
				return returnedObject;
			}
		}
		return null;
	}

	/// <summary>
	/// Yields while an animation clip is playing.
	/// </summary>
	/// <param name="animation">The animation component.</param>
	/// <param name="animationName">The name of the animation clip.</param>
	/// <returns>The enumerator.</returns>
	public static IEnumerator WhilePlaying(this Animation animation, string animationName)
	{
		while (animation.IsPlaying(animationName))
		{
			yield return null;
		}
	}

	/// <summary>
	/// Loads an asset from the resources folder.
	/// </summary>
	/// <typeparam name="T">The asset type to load.</typeparam>
	/// <param name="path">The path to the asset.</param>
	/// <returns>The loaded asset.</returns>
	public static T Load<T>(string path) where T : UnityEngine.Object
	{
        T asset = Resources.Load<T>(path);
        if (asset == null)
        {
            throw new System.ArgumentException("Could not find asset at " + path);
        }
        return asset;
	}

	/// <summary>
	/// Finds a shader.
	/// </summary>
	/// <param name="name">The name of the shader.</param>
	/// <returns>The shader.</returns>
	public static Shader FindShader(string name)
	{
		Shader shader = Shader.Find(name);
		if (shader == null)
		{
			throw new System.ArgumentException("Could not find shader " + name);
		}
		return shader;
	}

	/// <summary>
	/// Writes a Vector3 object to a stream using a BinaryWriter.
	/// </summary>
	/// <param name="writer">The BinaryWriter object.</param>
	/// <param name="vector">The Vector3 to write.</param>
	public static void WriteVector3(BinaryWriter writer, Vector3 vector)
	{
		writer.Write(vector.x);
		writer.Write(vector.y);
		writer.Write(vector.z);
	}

	/// <summary>
	/// Reads a Vector3 object from a stream using a BinaryReader.
	/// </summary>
	/// <param name="reader">The BinaryReader object.</param>
	/// <returns>The Vector3 object that was read.</returns>
	public static Vector3 ReadVector3(BinaryReader reader)
	{
		return new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
	}

	/// <summary>
	/// Writes a Color object to a stream using a BinaryWriter.
	/// </summary>
	/// <param name="writer">The BinaryWriter object.</param>
	/// <param name="color">The Color to write.</param>
	public static void WriteColor(BinaryWriter writer, Color color)
	{
		writer.Write(color.r);
		writer.Write(color.g);
		writer.Write(color.b);
		writer.Write(color.a);
	}

	/// <summary>
	/// Reads a Color object from a stream using a BinaryReader.
	/// </summary>
	/// <param name="reader">The BinaryReader object.</param>
	/// <returns>The Color object that was read.</returns>
	public static Color ReadColor(BinaryReader reader)
	{
		return new Color(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
	}
}
