﻿using RST.Extensions;

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
        Assert.That(count, Is.EqualTo(3));
    }
}
