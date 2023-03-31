using RST.Defaults;
using RST.Extensions;

namespace RST.UnitTests;

[TestFixture]
public class PropertyInfoExtensionTests
{
    [Test]
    public void GetInheritedProperties()
    {
        var t = typeof(MySecondoryInheritedType);

        var properties = t.GetAllProperties();
        var count = properties.Count();
        Assert.That(count, Is.EqualTo(4));


        var myCache = new PropertyTypeProviderCache();
        properties = t.GetAllProperties(myCache);
        count = properties.Count();
        Assert.That(count, Is.EqualTo(4));

        Assert.That(myCache, Contains.Key(t));
    }
}
