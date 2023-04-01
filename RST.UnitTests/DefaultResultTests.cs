using Moq;
using RST.Defaults;

namespace RST.UnitTests;

[TestFixture]
public class DefaultResultTests
{
    private DefaultResult<MySecondoryInheritedType> result;
    [SetUp]
    public void SetUp()
    {
        result = new DefaultResult<MySecondoryInheritedType>();
    }
    
    [Test]
    public void ImplcitConversion()
    {
        result.IsSuccessful = true;
        result.Value = new MySecondoryInheritedType
        {
            A = 1,
            C = true
        };
        var implicitConvertedResult = (DefaultResult)result;
        
        Assert.That(implicitConvertedResult.IsSuccessful, Is.True);
        Assert.That(implicitConvertedResult.Value, Is.InstanceOf<MySecondoryInheritedType>());
    }
}
