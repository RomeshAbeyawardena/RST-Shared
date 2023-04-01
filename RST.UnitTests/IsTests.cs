using RST.Attributes;
using RST.Contracts;
using RST.Extensions;

namespace RST.UnitTests;

[TestFixture]
public class IsTests
{
    interface IMyHashable : IHashable
    {

    }

    record MyHashableImplementation : IMyHashable
    {
        public string? Hash { get; set; }
    }
    [Test]
    public void Test()
    {
        var implementation = new MyHashableImplementation();

        Assert.That(implementation is IHashable, Is.True);

        var attrib = typeof(MyHashableImplementation).GetUnderliningAttributes<HashColumnAttribute>();
    }
}
