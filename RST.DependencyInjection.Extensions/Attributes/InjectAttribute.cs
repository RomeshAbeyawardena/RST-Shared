namespace RST.DependencyInjection.Extensions.Attributes;

/// <summary>
/// Marks a field or property as injectable
/// </summary>
[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = true)]
public sealed class InjectAttribute : Attribute
{

}
