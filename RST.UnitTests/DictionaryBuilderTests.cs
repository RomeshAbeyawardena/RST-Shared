using RST.Defaults;

namespace RST.UnitTests;

[TestFixture]
public class DictionaryBuilderTests
{
    private Dictionary<string, int> underliningDictionary;
    private DefaultDictionaryBuilder<string, int> dictionaryBuilder;
    [SetUp]
    public void SetUp()
    {
        underliningDictionary = new Dictionary<string, int>();
        dictionaryBuilder = new DefaultDictionaryBuilder<string, int>(underliningDictionary);
    }


    [Test]
    public void Ensure_underlining_dictionary_is_updated()
    {
        dictionaryBuilder.Add("banana", 1234);

        Assert.That(dictionaryBuilder, Does.Contain(KeyValuePair.Create("banana", 1234)));
    }
}
