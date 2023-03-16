namespace RST.UnitTests;

[TestFixture]
public class SubstringTests
{
    [Test]
    public void GetLast16()
    {
        var encodedString = "Thisistheencryptedblobthatisneedstobedecryptedbeforebeingaccessedandthenextbitwillbethepublickey.Thisthepublickey";

        Assert.That(encodedString.Substring(encodedString.Length - 16, 16), Is.EqualTo("Thisthepublickey"));

        Assert.That(encodedString[..^16], Is.EqualTo("Thisistheencryptedblobthatisneedstobedecryptedbeforebeingaccessedandthenextbitwillbethepublickey."));
    }
}
