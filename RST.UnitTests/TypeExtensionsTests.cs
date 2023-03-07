using RST.Extensions;

namespace RST.UnitTests;

[TestFixture]
public class TypeExtensionsTests
{

    [Test]
    public void GetInheritedTypes()
    {
        var inheritedTypes = typeof(MySecondoryInheritedType).GetInheritedTypes();
        Assert.That(inheritedTypes.Count, Is.EqualTo(3));
    }
}
