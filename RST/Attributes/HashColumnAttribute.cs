using System.Security.Cryptography;

namespace RST.Attributes;

/// <summary>
/// Represents a hash column
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
public class HashColumnAttribute : Attribute
{
	/// <summary>
	/// 
	/// </summary>
	/// <param name="name"></param>
	public HashColumnAttribute(string name)
	{
		Name = new HashAlgorithmName(name);
	}

	/// <summary>
	/// Gets the hash algorithm name
	/// </summary>
	public HashAlgorithmName Name { get; set; }

}
