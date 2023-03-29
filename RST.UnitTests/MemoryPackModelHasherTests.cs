using Moq;
using RST.Attributes;
using RST.Contracts;
using RST.Defaults;
using RST.UnitTests.TestEntities;
using System.Security.Cryptography;

namespace RST.UnitTests;

[TestFixture]
public class MemoryPackModelHasherTests
{
    private Mock<IHashAlgorithmProvider> hashAlgorithmProviderMock;
    private MemoryPackModelHasher? memoryPackModelHasher;
    [SetUp]
    public void SetUp()
    {
        hashAlgorithmProviderMock = new Mock<IHashAlgorithmProvider>();
        memoryPackModelHasher = new MemoryPackModelHasher(hashAlgorithmProviderMock.Object);
    }

    [Test]
    public void HashColumnAttribute()
    {
        var a = new HashColumnAttribute(HashAlgorithms.SHA512);
        Assert.That(a.Name, Is.EqualTo(HashAlgorithmName.SHA512));
    }

    [Test]
    public void CalculateHash()
    {
        using var sha512 = SHA512.Create();
        hashAlgorithmProviderMock.Setup(h => h.GetHashAlgorithm(HashAlgorithmName.SHA512))
            .Returns(sha512);
        var idGuid = Guid.Parse("F36ECB19-D4BD-4F9F-925F-D937F2365110");
        
        var date = new DateTimeOffset(2023,03,09,12,14,30, TimeSpan.FromHours(1));
        var hash = memoryPackModelHasher!.CalculateHash(new Customer { 
            Firstname = "John",
            Middlename = "Henry",
            Lastname = "Doe",
            Id = idGuid,
            PopulatedDate = date
        }, new MemoryPackModelHasherOptions
        {
            HashAlgorithmName = HashAlgorithmName.SHA512
        });

        Assert.That(hash, Is.EqualTo("zvuR/egU61jUP1m6M8LAswk/ohEAJ6OlaIU/hoJcC61QdX5Vcko7dQDUo6Wnw3yAKRvz81A9vtlBKXqJnd6XBw=="));
    }
}
